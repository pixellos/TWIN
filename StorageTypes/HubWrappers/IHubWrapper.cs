using System;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace WebLedMatrix.Hubs
{
    public interface IHubWrapper<T>  where T : Hub
    {
        string GetHubName();
        string GetMethodName(Delegate x);
        Task<object> InvokeAtServer(Delegate hardTypedFromHubFunction, object[] args);
        Task<TReturnValue> InvokeAtServer<TReturnValue>(Delegate hardTypedFromHubFunction, object[] args);
        void RegisterAndInvoke<TInterface, TClassToRegister>(TClassToRegister registerClassObject) where TClassToRegister : TInterface;
        Task Start(string url = null);
    }
}