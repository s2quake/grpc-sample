// MIT License
// 
// Copyright (c) 2019 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Logging;
using Ntreev.Library.Threading;

namespace Ntreev.Crema.Communication.Grpc
{
    class AdaptorServerHost : IAdaptorHost, IContextInvoker
    {
        private readonly Dictionary<string, IService> serviceByName = new Dictionary<string, IService>();
        private readonly Dictionary<Type, IExceptionSerializer> exceptionSerializerByType = new Dictionary<Type, IExceptionSerializer>();
        private readonly Dictionary<string, MethodDescriptor> methodDescriptorByName = new Dictionary<string, MethodDescriptor>();
        //private readonly Dictionary<string, CallbackCollection> callbacksByName = new Dictionary<string, CallbackCollection>();
        private readonly HashSet<string> peerHashes = new HashSet<string>();
        private Dispatcher dispatcher;
        private ILogger logger;
        private CancellationTokenSource cancellation;
        private Server server;
        private AdaptorServerImpl adaptor;
        private ServiceInstanceBuilder instanceBuilder = new ServiceInstanceBuilder();
        private readonly PeerCollection peers = new PeerCollection();

        public AdaptorServerHost(IEnumerable<IService> services, IEnumerable<IExceptionSerializer> exceptionSerializers)
        {
            this.serviceByName = services.ToDictionary(item => item.Name);
            this.exceptionSerializerByType = exceptionSerializers.ToDictionary(item => item.ExceptionType);
            this.logger = GrpcEnvironment.Logger;
            this.dispatcher = new Dispatcher(this);
            foreach (var item in services)
            {
                RegisterMethod(this.methodDescriptorByName, item);
            }
        }

        internal async Task<OpenReply> Open(OpenRequest request, ServerCallContext context)
        {
            var token = await this.dispatcher.InvokeAsync(() =>
            {
                var peerDescriptor = this.CreatePeerDescriptor(context.Peer, request.ServiceNames);
                this.peers.Add(peerDescriptor);
                return peerDescriptor.Token;
            });
            this.logger.Debug($"Connected: {context.Peer}");
            return new OpenReply() { Token = $"{token}" };
        }

        internal async Task<CloseReply> Close(CloseRequest request, ServerCallContext context)
        {
            await this.dispatcher.InvokeAsync(() =>
            {
                var peerDescriptor = this.peers[context.Peer];
                peerDescriptor.Dispose();
            });
            this.logger.Debug($"Disconnected: {context.Peer}");
            return new CloseReply();
        }

        internal Task<PingReply> Ping(PingRequest request, ServerCallContext context)
        {
            return this.dispatcher.InvokeAsync(() =>
            {
                var peerDescriptor = this.peers[context.Peer];
                peerDescriptor.Ping = DateTime.UtcNow;
                return new PingReply() { Time = peerDescriptor.Ping.Ticks };
            });
        }

        internal async Task<InvokeReply> Invoke(InvokeRequest request, ServerCallContext context)
        {
            if (this.serviceByName.ContainsKey(request.ServiceName) == false)
                throw new InvalidOperationException();
            var service = this.serviceByName[request.ServiceName];
            if (this.methodDescriptorByName.ContainsKey(request.Name) == false)
                throw new InvalidOperationException();

            var methodDescriptor = methodDescriptorByName[request.Name];
            try
            {
                var (valueType, value) = await methodDescriptor.InvokeAsync(service, request.Datas);
                var reply = new InvokeReply()
                {
                    Data = SerializerUtility.GetString(value, valueType)
                };
                return reply;
            }
            catch (Exception e)
            {
                var serializer = this.GetExceptionSerializer(e);
                var data = serializer.Serialize(e);
                var reply = new InvokeReply()
                {
                    Code = serializer.ExceptionCode,
                    Data = data
                };
                return reply;
            }
        }

        internal async Task Poll(IAsyncStreamReader<PollRequest> requestStream, IServerStreamWriter<PollReply> responseStream, ServerCallContext context)
        {
            var cancellationToken = this.cancellation.Token;
            this.peerHashes.Add(context.Peer);
            var peerDescriptor = await this.dispatcher.InvokeAsync(() => this.peers[context.Peer]);
            while (await requestStream.MoveNext())
            {
                var request = requestStream.Current;
                var services = peerDescriptor.Services;
                if (this.cancellation.IsCancellationRequested == true)
                {
                    await responseStream.WriteAsync(new PollReply() { Code = -1 });
                    break;
                }
                var reply = new PollReply();
                await this.dispatcher.InvokeAsync(() =>
                {
                    foreach (var item in services)
                    {
                        var items = this.Poll(peerDescriptor, item);
                        reply.Items.AddRange(items);
                    }
                });
                await responseStream.WriteAsync(reply);
            }
            this.peerHashes.Remove(context.Peer);
        }

        public object CreateCallback(string peer, IService service)
        {
            var instanceType = service.CallbackType;
            var typeName = $"{instanceType.Name}Impl";
            var typeNamespace = instanceType.Namespace;
            var implType = instanceBuilder.CreateType(typeName, typeNamespace, typeof(InstanceBase), instanceType);
            var instance = TypeDescriptor.CreateInstance(null, implType, null, null) as InstanceBase;
            instance.Service = service;
            instance.Invoker = this;
            instance.Peer = peer;
            return instance;
        }

        public void Dispose()
        {
            this.dispatcher.Dispose();
            this.dispatcher = null;
        }

        public event EventHandler<DisconnectionReasonEventArgs> Disconnected;

        public event EventHandler<PeerEventArgs> PeerAdded;

        public event EventHandler<PeerEventArgs> PeerRemoved;

        protected virtual void OnDisconnected(DisconnectionReasonEventArgs e)
        {
            this.Disconnected?.Invoke(this, e);
        }

        private void ValidateToken(string token)
        {
            this.dispatcher.VerifyAccess();
            if (token == null)
                throw new ArgumentNullException(nameof(token));
        }

        private Task AddCallback(InstanceBase instance, string name, object[] args)
        {
            var datas = SerializerUtility.GetStrings(args);
            return this.dispatcher.InvokeAsync(() =>
            {
                var peerDescriptor = this.peers[instance.Peer];
                var service = instance.Service;
                var callbacks = peerDescriptor.Callbacks[service];
                var pollItem = new PollReplyItem()
                {
                    Id = callbacks.Count,
                    Name = name,
                    ServiceName = instance.ServiceName
                };
                pollItem.Datas.AddRange(datas);
                callbacks.Add(pollItem);
            });
        }

        private PollReplyItem[] Poll(PeerDescriptor peerDescriptor, IService service)
        {
            this.dispatcher.VerifyAccess();
            var callbacks = peerDescriptor.Callbacks[service];
            return callbacks.Flush();
        }

        private IExceptionSerializer GetExceptionSerializer(Exception e)
        {
            if (this.exceptionSerializerByType.ContainsKey(e.GetType()) == true)
            {
                return this.exceptionSerializerByType[e.GetType()];
            }
            return this.exceptionSerializerByType[typeof(Exception)];
        }

        private PeerDescriptor CreatePeerDescriptor(string peer, IEnumerable<string> serviceNames)
        {
            var peerDescriptor = new PeerDescriptor(peer)
            {
                Services = serviceNames.Select(item => this.serviceByName[item]).ToArray(),
            };
            foreach (var item in peerDescriptor.Services)
            {
                var callback = this.CreateCallback(peerDescriptor.Peer, item);
                peerDescriptor.CallbackInstances.Add(item, callback);
                var instance = item.CreateInstance(callback);
                peerDescriptor.ServiceInstances.Add(item, instance);
            }
            return peerDescriptor;
        }

        private static void RegisterMethod(Dictionary<string, MethodDescriptor> methodDescriptorByName, IService service)
        {
            var methods = service.ServiceType.GetMethods();
            foreach (var item in methods)
            {
                if (item.GetCustomAttribute(typeof(OperationContractAttribute)) is OperationContractAttribute attr)
                {
                    var methodName = attr.Name ?? item.Name;
                    var methodDescriptor = new MethodDescriptor(item);
                    methodDescriptorByName.Add(methodDescriptor.Name, methodDescriptor);
                }
            }
        }

        #region IContextInvoker

        void IContextInvoker.Invoke(InstanceBase instance, string name, object[] args)
        {
            this.AddCallback(instance, name, args);
        }

        T IContextInvoker.Invoke<T>(InstanceBase instance, string name, object[] args)
        {
            throw new NotImplementedException();
        }

        Task IContextInvoker.InvokeAsync(InstanceBase instance, string name, object[] args)
        {
            throw new NotImplementedException();
        }

        Task<T> IContextInvoker.InvokeAsync<T>(InstanceBase instance, string name, object[] args)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IAdaptorHost

        Task IAdaptorHost.OpenAsync(string host, int port)
        {
            this.adaptor = new AdaptorServerImpl(this);
            this.server = new Server()
            {
                Services = { Adaptor.BindService(this.adaptor) },
                Ports = { new ServerPort(host, port, ServerCredentials.Insecure) },
            };

            this.cancellation = new CancellationTokenSource();
            return Task.Run(this.server.Start);
        }

        async Task IAdaptorHost.CloseAsync()
        {
            this.adaptor = null;
            this.cancellation.Cancel();

            while (this.peerHashes.Any())
            {
                await Task.Delay(1);
            }
            await this.server.ShutdownAsync();
            this.server = null;
        }

        #endregion
    }
}