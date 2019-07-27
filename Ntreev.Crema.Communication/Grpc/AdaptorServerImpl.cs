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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Newtonsoft.Json;
using Ntreev.Library.Threading;

namespace Ntreev.Crema.Communication.Grpc
{
    class AdaptorServerImpl : Adaptor.AdaptorBase, IContextInvoker
    {
        private const string tokenKey = "token";
        private const string servicesKey = "services";
        private static readonly JsonSerializerSettings settings = new JsonSerializerSettings();
        private readonly Dictionary<string, IService> serviceByName = new Dictionary<string, IService>();
        private readonly Dictionary<string, MethodInfo> methodByName = new Dictionary<string, MethodInfo>();
        private readonly Dictionary<string, CallbackCollection> callbacksByName = new Dictionary<string, CallbackCollection>();
        private readonly PeerProperties properties = new PeerProperties();
        private Dispatcher dispatcher;

        public AdaptorServerImpl(IEnumerable<IService> services)
        {
            this.serviceByName = services.ToDictionary(item => item.Name);
            this.callbacksByName = services.ToDictionary(item => item.Name, item => new CallbackCollection(item));
            this.dispatcher = new Dispatcher(this);
            foreach (var item in services)
            {
                RegisterMethod(this.methodByName, item);
            }
        }

        public override async Task<OpenReply> Open(OpenRequest request, ServerCallContext context)
        {
            var token = Guid.NewGuid();
            var tokenString = token.ToString();
            await this.dispatcher.InvokeAsync(() =>
            {
                this.properties.Add(context.Peer, tokenKey, tokenString);
                this.properties.Add(context.Peer, servicesKey, request.ServiceName.ToArray());
                foreach (var item in this.callbacksByName)
                {
                    this.properties.Add(context.Peer, $"{item.Key}.ID", item.Value.Count);
                }
            });
            return new OpenReply() { Token = tokenString };
        }

        public override async Task<CloseReply> Close(CloseRequest request, ServerCallContext context)
        {
            await this.dispatcher.InvokeAsync(() =>
            {
                this.properties.Remove(context.Peer, tokenKey);
                this.properties.Remove(context.Peer, servicesKey);
            });
            return new CloseReply();
        }

        public override Task<PingReply> Ping(PingRequest request, ServerCallContext context)
        {
            return this.dispatcher.InvokeAsync(() =>
            {
                return new PingReply() { Time = DateTime.UtcNow.Ticks };
            });
        }

        public override async Task<InvokeReply> Invoke(InvokeRequest request, ServerCallContext context)
        {
            if (this.serviceByName.ContainsKey(request.ServiceName) == false)
                throw new InvalidOperationException();
            var service = this.serviceByName[request.ServiceName];
            var methodName = $"{request.ServiceName}.{request.Name}";
            if (this.methodByName.ContainsKey(methodName) == false)
                throw new InvalidOperationException();

            var args = AdaptorUtility.GetArguments(request.Types_, request.Datas);
            var method = methodByName[methodName];
            var value = await Task.Run(() => method.Invoke(service, args));
            var valueType = method.ReturnType;
            if (value is Task task)
            {
                await task;
                var taskType = task.GetType();
                if (taskType.GetGenericArguments().Any() == true)
                {
                    var propertyInfo = taskType.GetProperty(nameof(Task<object>.Result));
                    value = propertyInfo.GetValue(task);
                    valueType = propertyInfo.PropertyType;
                }
                else
                {
                    value = null;
                    valueType = typeof(void);
                }
            }
            var reply = new InvokeReply()
            {
                ServiceName = request.ServiceName,
                Type = valueType.AssemblyQualifiedName,
            };
            if (valueType != typeof(void))
            {
                reply.Data = JsonConvert.SerializeObject(value, valueType, settings);
            }

            return reply;
        }

        public override async Task Poll(IAsyncStreamReader<PollRequest> requestStream, IServerStreamWriter<PollReply> responseStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext(context.CancellationToken))
            {
                var request = requestStream.Current;
                var serviceNames = this.properties[context.Peer, servicesKey] as string[];
                var reply = new PollReply();
                await this.dispatcher.InvokeAsync(() =>
                {
                    foreach (var item in serviceNames)
                    {
                        var service = this.serviceByName[item];
                        var id = (int)this.properties[context.Peer, $"{item}.ID"];
                        var items = this.Poll(service, ref id);
                        reply.Items.AddRange(items);
                        this.properties.Add(context.Peer, $"{item}.ID", id);
                    }
                });
                await responseStream.WriteAsync(reply);
            }
        }

        private PollReplyItem[] Poll(IService service, ref int id)
        {
            this.dispatcher.VerifyAccess();
            var callbacks = this.callbacksByName[service.Name];
            var items = new PollReplyItem[callbacks.Count - id];
            for (var i = id; i < callbacks.Count; i++)
            {
                items[i - id] = callbacks[i];
            }
            id = callbacks.Count;
            return items;
        }

        // private Task<PollReplyItem[]> PollAsync(IService service, int id)
        // {
        //     return this.dispatcher.InvokeAsync(() =>
        //     {
        //         var callbacks = this.callbacksByName[service.Name];
        //         var items = new PollReplyItem[callbacks.Count - id];
        //         for (var i = id; i < callbacks.Count; i++)
        //         {
        //             items[i - id] = callbacks[i];
        //         }
        //         return items;
        //     });
        // }

        public async Task DisposeAsync()
        {
            await this.dispatcher.InvokeAsync(() =>
            {
                var pollItem = new PollReplyItem()
                {
                    ServiceName = AdaptorUtility.ClosedName,
                    Name = AdaptorUtility.ClosedName
                };
                foreach (var item in this.callbacksByName.Values)
                {
                    item.Add(pollItem);
                }
            });
            this.dispatcher.Dispose();
            this.dispatcher = null;
        }

        private void ValidateToken(string token)
        {
            this.dispatcher.VerifyAccess();
            if (token == null)
                throw new ArgumentNullException(nameof(token));
        }

        private Task AddCallback(string serviceName, string name, object[] args)
        {
            var (types, datas) = AdaptorUtility.GetStrings(args);
            return this.dispatcher.InvokeAsync(() =>
            {
                var callbacks = this.callbacksByName[serviceName];
                var pollItem = new PollReplyItem()
                {
                    Id = callbacks.Count,
                    Name = name,
                };
                pollItem.Types_.AddRange(types);
                pollItem.Datas.AddRange(datas);
                callbacks.Add(pollItem);
            });
        }

        private static void RegisterMethod(Dictionary<string, MethodInfo> methodByName, IService service)
        {
            var methods = service.ServiceType.GetMethods();
            foreach (var item in methods)
            {
                if (item.GetCustomAttribute(typeof(ServiceContractAttribute)) is ServiceContractAttribute attr)
                {
                    var methodName = attr.Name ?? item.Name;
                    methodByName.Add($"{service.Name}.{methodName}", item);
                }
            }
        }

        #region IContextInvoker

        void IContextInvoker.Invoke(IService service, string name, object[] args)
        {
            this.AddCallback(service.Name, name, args);
        }

        T IContextInvoker.Invoke<T>(IService service, string name, object[] args)
        {
            throw new NotImplementedException();
        }

        Task IContextInvoker.InvokeAsync(IService service, string name, object[] args)
        {
            throw new NotImplementedException();
        }

        Task<T> IContextInvoker.InvokeAsync<T>(IService service, string name, object[] args)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}