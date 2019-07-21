using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Grpc.Core;
using Newtonsoft.Json;
using Ntreev.Library.Threading;

namespace Ntreev.Crema.Services.Users
{
    [Export(typeof(IService))]
    class UserService : ServiceBase<IUserService, IUserServiceCallback>, IUserService
    {
        [ServiceContract]
        public async Task<int> LoginAsync(string user)
        {
            await Task.Delay(100);
            Console.WriteLine("LoginAsync");
            return 0;
            //Task.Run(()=> this.Callback.OnAdd("WER", 0));
        }

        [ServiceContract]
        public async Task<(int, string)> LogoutAsync(string user, int count)
        {
            await Task.Delay(100);
            return (1, "ser");
        }
    }

    // class UserServiceOld : Adaptor.AdaptorBase, IUserServiceCallback
    // {
    //     private JsonSerializerSettings settings = new JsonSerializerSettings();
    //     private readonly Dispatcher dispatcher;
    //     private List<PollReplyItem> callbackList = new List<PollReplyItem>();
    //     private int id;
    //     private readonly PollReplyItem nullReply = new PollReplyItem() { Id = -1 };

    //     public UserService()
    //     {
    //         this.dispatcher = new Dispatcher(this);
    //     }
    //     public override Task<InvokeReply> Invoke(InvokeRequest request, ServerCallContext context)
    //     {
    //         throw new NotImplementedException();
    //     }

    //     public void OnLoggedIn(string userID)
    //     {
    //         this.AddCallback(nameof(OnLoggedIn), userID);
    //     }

    //     public void OnAdd(string userID, int test)
    //     {

    //     }

    //     public override async Task Poll(IAsyncStreamReader<PollRequest> requestStream, IServerStreamWriter<PollReply> responseStream, ServerCallContext context)
    //     {
    //         while (await requestStream.MoveNext())
    //         {
    //             var request = requestStream.Current;
    //             var id = request.Id;
    //             var reply = new PollReply();
    //             await this.dispatcher.InvokeAsync(() =>
    //             {
    //                 var items = new PollReplyItem[this.callbackList.Count - id];
    //                 for (var i = id; i < this.callbackList.Count; i++)
    //                 {
    //                     reply.Items.Add(this.callbackList[i]);
    //                 }
    //                 return items;
    //             });
    //             await responseStream.WriteAsync(reply);
    //         }
    //     }

    //     public void Dispose()
    //     {
    //         this.dispatcher.Dispose();
    //     }

    //     private void AddCallback<T>(string name, T arg)
    //     {
    //         this.dispatcher.InvokeAsync(() =>
    //         {
    //             var reply = new PollReplyItem() { Id = id++ };
    //             reply.Name = name;
    //             reply.Type.Add(typeof(T).AssemblyQualifiedName);
    //             reply.Data.Add(JsonConvert.SerializeObject(arg, typeof(T), this.settings));
    //             this.callbackList.Add(reply);
    //         });
    //     }

    //     private void AddCallback<T1, T2>(string name, T1 arg1, T2 arg2)
    //     {
    //         this.dispatcher.InvokeAsync(() =>
    //         {
    //             var reply = new PollReplyItem() { Id = id++ };
    //             reply.Name = name;
    //             reply.Type.Add(typeof(T1).AssemblyQualifiedName);
    //             reply.Data.Add(JsonConvert.SerializeObject(arg1, typeof(T1), this.settings));
    //             reply.Type.Add(typeof(T2).AssemblyQualifiedName);
    //             reply.Data.Add(JsonConvert.SerializeObject(arg2, typeof(T2), this.settings));
    //             this.callbackList.Add(reply);
    //         });
    //     }
    // }
}