using System;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Microsoft.AspNet.SignalR.Hubs;
using WebLedMatrix.Hubs;

namespace StorageTypes.HubWrappers
{
    public class HubWrapper<THub> : IHubWrapper<THub> where THub : Hub// ClientSide
    {
        const string DefaultUrl = "http://localhost:8080";
        private string Url { get; set; }

        private IHub _hub;//
        private IHubProxy _hubProxy;
        private HubConnection _hubConnection;

        public HubWrapper(IHub hub, HubConnection hubConnection = null, IHubProxy hubProxy = null)
        {
            _hub = hub;
            _hubConnection = hubConnection ?? new HubConnection(DefaultUrl);
            _hubProxy = hubProxy ?? _hubConnection.CreateHubProxy(GetHubName());
        }
        public HubWrapper(IHub hub, string url = null) : this(hub,null,null)
        {
            Url = url ?? DefaultUrl;
        }

        public async Task Start(string url = null)
        {
            try
            {
                await _hubConnection.Start();
            }
            catch (Exception e )
            {
                Console.WriteLine(e.StackTrace);
                throw;
            }
        }

        public void RegisterAndInvoke<TInterface, TClassToRegister>(TClassToRegister registerClassObject) where TClassToRegister : TInterface
        {
           var x = typeof (TInterface).GetRuntimeMethods();
            
            foreach (var interfaceVar in x)
            {
                foreach (var classVar in (typeof(TClassToRegister)).GetRuntimeMethods())
                {
                    var interfaceParameters = interfaceVar.GetParameters();
                    var classParameters = classVar.GetParameters();

                    bool areReturnParametersEquals = interfaceParameters.IsEquals(classParameters);
                    bool areNamesEquals = interfaceVar.Name == classVar.Name;

                    if (areNamesEquals && interfaceVar.ReturnParameter.IsEquals(classVar.ReturnParameter) && areReturnParametersEquals)
                    {
                        _hubProxy.On<object[]>(interfaceVar.Name, (parameter) =>
                        {
                            classVar.Invoke(registerClassObject,
                                parameter.DeserializeObject(classParameters.Length));
                        });
                        classVar.Invoke(registerClassObject, new object[classParameters.Length]); //Invoking for test purposes -> cant test it without it
                        break;
                    }
                }
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
            if (typeof (THub) == x.Method.DeclaringType)
            {
                return x.Method.Name;
            }
            throw new MethodDoesntBelongToDesiredTypeException(x.Method.Name,
                typeof (THub).FullName, x.Method.DeclaringType.FullName);
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
