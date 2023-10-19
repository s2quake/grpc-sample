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

using JSSoft.Library.ObjectModel;
using JSSoft.Library.Threading;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace JSSoft.Communication;

public abstract class ServiceHostBase : IServiceHost
{
    private ServiceToken? _token;
    private Dispatcher? _dispatcher;

    internal ServiceHostBase(Type serviceType, Type callbackType)
    {
        ServiceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
        CallbackType = callbackType ?? throw new ArgumentNullException(nameof(callbackType));
        Name = serviceType.Name;
        MethodDescriptors = new MethodDescriptorCollection(this);
        OnValidate();
    }

    public Type ServiceType { get; }

    public Type CallbackType { get; }

    public Dispatcher Dispatcher => _dispatcher ?? throw new InvalidOperationException();

    public MethodDescriptorCollection MethodDescriptors { get; }

    public async Task OpenAsync(ServiceToken token)
    {
        if (_dispatcher != null)
            throw new InvalidOperationException();
        if (token == ServiceToken.Empty)
            throw new ArgumentException("Empty tokens cannot be used.", nameof(token));

        _token = token;
        _dispatcher = new Dispatcher(this);
        await Dispatcher.InvokeAsync(() =>
        {
            OnOpened(EventArgs.Empty);
        });
    }

    public async Task CloseAsync(ServiceToken token)
    {
        if (_dispatcher == null || _token != token)
            throw new InvalidOperationException();

        await Dispatcher.InvokeAsync(() =>
        {
            OnClosed(EventArgs.Empty);
        });
        _token = null;
        _dispatcher.Dispose();
        _dispatcher = null;
    }

    public string Name { get; }

    public event EventHandler? Opened;

    public event EventHandler? Closed;

    protected virtual void OnOpened(EventArgs e)
    {
        Opened?.Invoke(this, e);
    }

    protected virtual void OnClosed(EventArgs e)
    {
        Closed?.Invoke(this, e);
    }

    protected virtual void OnValidate()
    {
        if (ServiceType.IsInterface == false)
            throw new InvalidOperationException("service type must be interface.");
        if (ServiceType.IsNested == true)
            throw new InvalidOperationException("service type can not be nested type.");
        if (IsPublicType(ServiceType) == false && IsInternalType(ServiceType) == false)
            throw new InvalidOperationException($"'{ServiceType.Name}' must be public or internal.");

        if (CallbackType != typeof(void))
        {
            if (CallbackType.IsInterface == false)
                throw new InvalidOperationException("callback type must be interface.");
            if (CallbackType.IsNested == true)
                throw new InvalidOperationException("callback type can not be nested type.");
            if (IsPublicType(CallbackType) == false && IsInternalType(CallbackType) == false)
                throw new InvalidOperationException($"'{ServiceType.Name}' type must be public or internal.");
        }
    }

    private static bool IsPublicType(Type type)
    {
        return type.IsVisible == true && type.IsPublic == true && type.IsNotPublic == false;
    }

    private static bool IsInternalType(Type t)
    {
        return t.IsVisible == false && t.IsPublic == false && t.IsNotPublic == true;
    }

    private protected abstract Task<object> CreateInstanceInternalAsync(IPeer peer, object obj);

    private protected abstract Task DestroyInstanceInternalAsync(IPeer peer, object obj);

    internal static bool IsServer(ServiceHostBase serviceHost)
    {
        if (serviceHost.GetType().GetCustomAttribute(typeof(ServiceHostAttribute)) is ServiceHostAttribute attribute)
        {
            return attribute.IsServer;
        }
        return false;
    }

    #region IServiceHost

    Task<object> IServiceHost.CreateInstanceAsync(IPeer peer, object obj)
    {
        return CreateInstanceInternalAsync(peer, obj);
    }

    Task IServiceHost.DestroyInstanceAsync(IPeer peer, object obj)
    {
        return DestroyInstanceInternalAsync(peer, obj);
    }

    IContainer<MethodDescriptor> IServiceHost.MethodDescriptors => MethodDescriptors;

    #endregion
}
