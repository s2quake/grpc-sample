using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using Ntreev.Library.Commands;
using Client;
using Client.Services;
using System.Threading.Tasks;
using Ntreev.Crema.Services;

namespace Client.Commands
{
    [Export(typeof(ICommand))]
    class UserCommand : CommandMethodBase
    {
        [Import]
        private Lazy<IUserService> userService = null;

        public UserCommand()
        {
            
        }

        [CommandMethod]
        public Task LoginAsync(string userID)
        {
            return this.UserService.LoginAsync(userID);
        }

        [CommandMethod]
        public async Task LoginOutAsync(string userID)
        {
            var value = await this.UserService.LogoutAsync(userID, 0);
        }

        private IUserService UserService => this.userService.Value;
    }
}
