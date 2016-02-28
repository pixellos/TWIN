using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Microsoft.AspNet.SignalR.Client.Infrastructure;
using Microsoft.AspNet.SignalR.Hubs;
using StorageTypes;
using StorageTypes.AppInterface.ClientHub;

namespace WebLedMatrix.Hubs
{
    public class ConnectionHubBase : Hub<NodeConnectionInterface>
    {
        public async void RequestForData()
        {
            //Register call - to UI information "Hi, i'm there and i can receive commands"
        }

        //From ui
        public async void DisplayData(object type, object data)
        {
            DisplayDataType displayDataType;
            DisplayDataType.TryParse((string)type, out displayDataType);
            Clients.Caller.sendData(new DataToDisplay
            {
                DisplayDataType = displayDataType,
                Data = data.ToString()
            });
        }
    }

    public static class Helper
    {
        public static bool IsEquals(this ParameterInfo[] firstParameterInfos, ParameterInfo[] secParameterInfos)
        {
            bool isEveryMatching = true;
            foreach (var first in firstParameterInfos)
            {
                bool isAnyMatching = false;
                foreach (var second in secParameterInfos)
                {
                    if (isAnyMatching == false)
                    {
                        isAnyMatching = first.Name.Equals(second.Name) && first.ParameterType.Equals(second.ParameterType);
                    }
                }
                if (isAnyMatching == false)
                {
                    isEveryMatching = false;
                }
            }
            return isEveryMatching;
        }
        public static bool IsEquals(this ParameterInfo firstParameterInfos, ParameterInfo secParameterInfos)
        {
             return firstParameterInfos.ParameterType.Equals(secParameterInfos.ParameterType);
        }
    }
   
    public class HubWrapper<T> where T : Hub // ClientSide
    {
      

        const string DefaultUrl = "localhost:8080";
        private string Url { get; set; }

        private IHub _hub;//
        private IHubProxy _hubProxy;
        private HubConnection _hubConnection;

        public HubWrapper(IHub hub, HubConnection hubConnection = null, IHubProxy hubProxy = null) : this(hub, null)
        {
            _hubConnection = hubConnection ?? new HubConnection(DefaultUrl);
            _hubProxy = hubProxy ?? new HubProxy(_hubConnection, GetHubName());
        }
        public HubWrapper(IHub hub, string url = DefaultUrl)
        {
            _hub = hub;
            Url = url;
        }

        public void Start()
        {
            _hubConnection.Start();
        }

        public void Register<TInterface, TClassToRegister>(TClassToRegister toRegister) where TClassToRegister : TInterface
        {
           var x = typeof (TInterface).GetRuntimeMethods();

            foreach (var interfaceVar in x)
            {
                    foreach (var classVar in (typeof(TClassToRegister)).GetRuntimeMethods())
                    {
                        var parameterInfos = interfaceVar.GetParameters();
                        var parameterInfo = classVar.GetParameters();
                        var isEqual = parameterInfos.IsEquals(parameterInfo);
                        if (interfaceVar.Name == classVar.Name && interfaceVar.ReturnParameter.IsEquals(classVar.ReturnParameter) && isEqual)
                        {
                            object[] objectMock = new object[parameterInfo.Length];
                            classVar.Invoke(toRegister, objectMock);
                        break;
                            
                        }
                    }
          //      _hubProxy.On(interfaceVar.Name, () =>
           //     {
                    

                //  });
            }
        }
     
        
        public async Task<TReturnValue> InvokeAtServer<TReturnValue>(Delegate hardTypedFromHubFunction,object[] args)
        {
             return await _hubProxy.Invoke<TReturnValue>(GetMethodName(hardTypedFromHubFunction), args);
        }
        public async Task<object> InvokeAtServer(Delegate hardTypedFromHubFunction, object[] args)
        {
            return await InvokeAtServer<object>(hardTypedFromHubFunction,args);
        }

        public string GetHubName()
        {
            return _hub.GetType().Name;
        }
        public string GetMethodName(Delegate x)
        {
            if (typeof (T).Equals(x.Method.DeclaringType))
            {
                return x.Method.Name;
            }
            throw new MethodDoesntBelongToDesiredTypeException(x.Method.Name,
                typeof (T).FullName, x.Method.DeclaringType.FullName);
        }
    }


    public class MethodDoesntBelongToDesiredTypeException : Exception
    {
        const string MethodDoesntBelongToDesiredTypeExceptionMessage =
            "This method {0} seems like it isn't member of desired class {1} - is member of {2}";

        private string _methodName;
        private string _parentType;
        private string _desiredType;
        public MethodDoesntBelongToDesiredTypeException(string methodName, string desiredType, string parentType)
        {
            _methodName = methodName;
            _desiredType = desiredType;
            _parentType = parentType;
        }

        public override string Message
        {
            get { return string.Format(MethodDoesntBelongToDesiredTypeExceptionMessage + base.Message,_methodName,_desiredType,_parentType); }
        }
    }
}
