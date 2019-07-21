using System;
using System.Threading.Tasks;

namespace Ntreev.Crema.Services
{
    public interface IService : IDisposable
    {
        Task<InvokeResult> InvokeAsync(object context, InvokeInfo info);

        Task<PollItem[]> PollAsync(object context, int id);

        void Open(ServiceToken token);

        void Close(ServiceToken token);

        Type ServiceType { get; }

        Type CallbackType { get; }

        event EventHandler Opened;

        event EventHandler Closed;
    }
}