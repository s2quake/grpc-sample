using System.Threading.Tasks;
using Ntreev.Crema.Communication;

namespace Ntreev.Crema.Services
{
    public interface IUserService
    {
        [ServiceContract]
        Task<int> LoginAsync(string user);

        [ServiceContract]
        Task<(int, string)> LogoutAsync(string user, int count);
    }
}
