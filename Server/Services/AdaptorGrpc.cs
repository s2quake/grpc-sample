// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: adaptor.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Ntreev.Crema.Services {
  internal static partial class Adaptor
  {
    static readonly string __ServiceName = "ntreev.crema.services.Adaptor";

    static readonly grpc::Marshaller<global::Ntreev.Crema.Services.InvokeRequest> __Marshaller_ntreev_crema_services_InvokeRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ntreev.Crema.Services.InvokeRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Ntreev.Crema.Services.InvokeReply> __Marshaller_ntreev_crema_services_InvokeReply = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ntreev.Crema.Services.InvokeReply.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Ntreev.Crema.Services.PollRequest> __Marshaller_ntreev_crema_services_PollRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ntreev.Crema.Services.PollRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Ntreev.Crema.Services.PollReply> __Marshaller_ntreev_crema_services_PollReply = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ntreev.Crema.Services.PollReply.Parser.ParseFrom);

    static readonly grpc::Method<global::Ntreev.Crema.Services.InvokeRequest, global::Ntreev.Crema.Services.InvokeReply> __Method_Invoke = new grpc::Method<global::Ntreev.Crema.Services.InvokeRequest, global::Ntreev.Crema.Services.InvokeReply>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Invoke",
        __Marshaller_ntreev_crema_services_InvokeRequest,
        __Marshaller_ntreev_crema_services_InvokeReply);

    static readonly grpc::Method<global::Ntreev.Crema.Services.PollRequest, global::Ntreev.Crema.Services.PollReply> __Method_Poll = new grpc::Method<global::Ntreev.Crema.Services.PollRequest, global::Ntreev.Crema.Services.PollReply>(
        grpc::MethodType.DuplexStreaming,
        __ServiceName,
        "Poll",
        __Marshaller_ntreev_crema_services_PollRequest,
        __Marshaller_ntreev_crema_services_PollReply);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Ntreev.Crema.Services.AdaptorReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of Adaptor</summary>
    [grpc::BindServiceMethod(typeof(Adaptor), "BindService")]
    public abstract partial class AdaptorBase
    {
      public virtual global::System.Threading.Tasks.Task<global::Ntreev.Crema.Services.InvokeReply> Invoke(global::Ntreev.Crema.Services.InvokeRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task Poll(grpc::IAsyncStreamReader<global::Ntreev.Crema.Services.PollRequest> requestStream, grpc::IServerStreamWriter<global::Ntreev.Crema.Services.PollReply> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(AdaptorBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_Invoke, serviceImpl.Invoke)
          .AddMethod(__Method_Poll, serviceImpl.Poll).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, AdaptorBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_Invoke, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Ntreev.Crema.Services.InvokeRequest, global::Ntreev.Crema.Services.InvokeReply>(serviceImpl.Invoke));
      serviceBinder.AddMethod(__Method_Poll, serviceImpl == null ? null : new grpc::DuplexStreamingServerMethod<global::Ntreev.Crema.Services.PollRequest, global::Ntreev.Crema.Services.PollReply>(serviceImpl.Poll));
    }

  }
}
#endregion
