// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Grpc/adaptor.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Ntreev.Crema.Communication.Grpc {
  internal static partial class Adaptor
  {
    static readonly string __ServiceName = "ntreev.crema.communication.grpc.Adaptor";

    static readonly grpc::Marshaller<global::Ntreev.Crema.Communication.Grpc.PingRequest> __Marshaller_ntreev_crema_communication_grpc_PingRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ntreev.Crema.Communication.Grpc.PingRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Ntreev.Crema.Communication.Grpc.PingReply> __Marshaller_ntreev_crema_communication_grpc_PingReply = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ntreev.Crema.Communication.Grpc.PingReply.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Ntreev.Crema.Communication.Grpc.InvokeRequest> __Marshaller_ntreev_crema_communication_grpc_InvokeRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ntreev.Crema.Communication.Grpc.InvokeRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Ntreev.Crema.Communication.Grpc.InvokeReply> __Marshaller_ntreev_crema_communication_grpc_InvokeReply = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ntreev.Crema.Communication.Grpc.InvokeReply.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Ntreev.Crema.Communication.Grpc.PollRequest> __Marshaller_ntreev_crema_communication_grpc_PollRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ntreev.Crema.Communication.Grpc.PollRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Ntreev.Crema.Communication.Grpc.PollReply> __Marshaller_ntreev_crema_communication_grpc_PollReply = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ntreev.Crema.Communication.Grpc.PollReply.Parser.ParseFrom);

    static readonly grpc::Method<global::Ntreev.Crema.Communication.Grpc.PingRequest, global::Ntreev.Crema.Communication.Grpc.PingReply> __Method_Ping = new grpc::Method<global::Ntreev.Crema.Communication.Grpc.PingRequest, global::Ntreev.Crema.Communication.Grpc.PingReply>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Ping",
        __Marshaller_ntreev_crema_communication_grpc_PingRequest,
        __Marshaller_ntreev_crema_communication_grpc_PingReply);

    static readonly grpc::Method<global::Ntreev.Crema.Communication.Grpc.InvokeRequest, global::Ntreev.Crema.Communication.Grpc.InvokeReply> __Method_Invoke = new grpc::Method<global::Ntreev.Crema.Communication.Grpc.InvokeRequest, global::Ntreev.Crema.Communication.Grpc.InvokeReply>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Invoke",
        __Marshaller_ntreev_crema_communication_grpc_InvokeRequest,
        __Marshaller_ntreev_crema_communication_grpc_InvokeReply);

    static readonly grpc::Method<global::Ntreev.Crema.Communication.Grpc.PollRequest, global::Ntreev.Crema.Communication.Grpc.PollReply> __Method_Poll = new grpc::Method<global::Ntreev.Crema.Communication.Grpc.PollRequest, global::Ntreev.Crema.Communication.Grpc.PollReply>(
        grpc::MethodType.DuplexStreaming,
        __ServiceName,
        "Poll",
        __Marshaller_ntreev_crema_communication_grpc_PollRequest,
        __Marshaller_ntreev_crema_communication_grpc_PollReply);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Ntreev.Crema.Communication.Grpc.AdaptorReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of Adaptor</summary>
    [grpc::BindServiceMethod(typeof(Adaptor), "BindService")]
    public abstract partial class AdaptorBase
    {
      public virtual global::System.Threading.Tasks.Task<global::Ntreev.Crema.Communication.Grpc.PingReply> Ping(global::Ntreev.Crema.Communication.Grpc.PingRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::Ntreev.Crema.Communication.Grpc.InvokeReply> Invoke(global::Ntreev.Crema.Communication.Grpc.InvokeRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task Poll(grpc::IAsyncStreamReader<global::Ntreev.Crema.Communication.Grpc.PollRequest> requestStream, grpc::IServerStreamWriter<global::Ntreev.Crema.Communication.Grpc.PollReply> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for Adaptor</summary>
    public partial class AdaptorClient : grpc::ClientBase<AdaptorClient>
    {
      /// <summary>Creates a new client for Adaptor</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public AdaptorClient(grpc::Channel channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for Adaptor that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public AdaptorClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected AdaptorClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected AdaptorClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::Ntreev.Crema.Communication.Grpc.PingReply Ping(global::Ntreev.Crema.Communication.Grpc.PingRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Ping(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Ntreev.Crema.Communication.Grpc.PingReply Ping(global::Ntreev.Crema.Communication.Grpc.PingRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Ping, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Ntreev.Crema.Communication.Grpc.PingReply> PingAsync(global::Ntreev.Crema.Communication.Grpc.PingRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return PingAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Ntreev.Crema.Communication.Grpc.PingReply> PingAsync(global::Ntreev.Crema.Communication.Grpc.PingRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Ping, null, options, request);
      }
      public virtual global::Ntreev.Crema.Communication.Grpc.InvokeReply Invoke(global::Ntreev.Crema.Communication.Grpc.InvokeRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Invoke(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Ntreev.Crema.Communication.Grpc.InvokeReply Invoke(global::Ntreev.Crema.Communication.Grpc.InvokeRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Invoke, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Ntreev.Crema.Communication.Grpc.InvokeReply> InvokeAsync(global::Ntreev.Crema.Communication.Grpc.InvokeRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return InvokeAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Ntreev.Crema.Communication.Grpc.InvokeReply> InvokeAsync(global::Ntreev.Crema.Communication.Grpc.InvokeRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Invoke, null, options, request);
      }
      public virtual grpc::AsyncDuplexStreamingCall<global::Ntreev.Crema.Communication.Grpc.PollRequest, global::Ntreev.Crema.Communication.Grpc.PollReply> Poll(grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Poll(new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncDuplexStreamingCall<global::Ntreev.Crema.Communication.Grpc.PollRequest, global::Ntreev.Crema.Communication.Grpc.PollReply> Poll(grpc::CallOptions options)
      {
        return CallInvoker.AsyncDuplexStreamingCall(__Method_Poll, null, options);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override AdaptorClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new AdaptorClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(AdaptorBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_Ping, serviceImpl.Ping)
          .AddMethod(__Method_Invoke, serviceImpl.Invoke)
          .AddMethod(__Method_Poll, serviceImpl.Poll).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, AdaptorBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_Ping, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Ntreev.Crema.Communication.Grpc.PingRequest, global::Ntreev.Crema.Communication.Grpc.PingReply>(serviceImpl.Ping));
      serviceBinder.AddMethod(__Method_Invoke, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Ntreev.Crema.Communication.Grpc.InvokeRequest, global::Ntreev.Crema.Communication.Grpc.InvokeReply>(serviceImpl.Invoke));
      serviceBinder.AddMethod(__Method_Poll, serviceImpl == null ? null : new grpc::DuplexStreamingServerMethod<global::Ntreev.Crema.Communication.Grpc.PollRequest, global::Ntreev.Crema.Communication.Grpc.PollReply>(serviceImpl.Poll));
    }

  }
}
#endregion
