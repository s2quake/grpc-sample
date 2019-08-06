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

using System.ComponentModel;
using System.ComponentModel.Composition;
using Ntreev.Crema.Communication;
using Ntreev.Crema.Services;

namespace Server.Services
{
    [Export(typeof(IServiceHost))]
    class UserServiceHost : ServerServiceHostBase<IUserService, IUserServiceCallback>, INotifyPropertyChanged
    {
        private IUserService userService;

        public override object CreateInstance(object obj)
        {
            this.userService = new UserService(obj as IUserServiceCallback);
            this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(UserService)));
            return this.userService;
        }

        public override void DestroyInstance(object obj)
        {
            if (obj is UserService userService)
            {
                userService.Dispose();
            }
        }

        [Export(typeof(IUserService))]
        public IUserService UserService => this.userService;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            this.PropertyChanged?.Invoke(this, e);
        }
    }
}