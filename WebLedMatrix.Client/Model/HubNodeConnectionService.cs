using System;
using System.Net;
using System.Security.Policy;
using Microsoft.AspNet.SignalR.Client;
using StorageTypes;
using StorageTypes.AppInterface.ClientHub;
using StorageTypes.HubWrappers;
using WebLedMatrix.Hubs;

namespace WebLedMatrix.ClientNode.Model
{
    public interface IDisplayCallbacks
    {
        Action<string> DisplayTextAction { get; }
        Action<string> DisplayImageAction { get; }
    }

    public interface IHubConnectionService
    {
        void SetConnection(string Url);
        void Start();
    }
    
    public class HubNodeConnectionService : IHubConnectionService, NodeConnectionInterface
    {
        private string Url = "localhost:8080";
        private HubWrapper<ClientNodeHubBase> _hubWrapper;
        private ClientNodeHubBase _typingClientNodeHubBase = new ClientNodeHubBase();

        /// <summary>
        /// For testing purposes - I couldnt make nsubstitute call ctor(null);
        /// </summary>
        public HubNodeConnectionService() : this("url"){        }

        public HubNodeConnectionService(string url = null)
        {
            _hubWrapper = new HubWrapper<ClientNodeHubBase>(new ClientNodeHubBase(), Url);
            Url = url ?? Url;
            ServicePointManager.DefaultConnectionLimit = 10; // For WPF App
        }

        private void ConfigureHubMethods()
        {
            _hubWrapper.RegisterAndInvoke<NodeConnectionInterface,HubNodeConnectionService>(this);   
        }

        public void SetConnection(string Url)
        {
//            _hubConnection = new HubConnection(Url);
//            _hubProxy = _hubConnection.CreateHubProxy(HubName);
//            ConfigureHubMethods();
        }

        public async void RequestData()
        {
//            var returnedData = await _hubProxy.Invoke<StorageTypes.DataToDisplay>("ClientIsReady");
        }

        public async void Start()
        {
            _hubWrapper.Start();
        }

        public virtual void sendData(DataToDisplay data, object[] args = null)
        {
           Console.WriteLine("sendData");
        }

        public sealed override string ToString()
        {
            return base.ToString();
        }

        public virtual async void Hello()
        {
            
            Action @delegate = _typingClientNodeHubBase.ClientIsReady;
            await _hubWrapper.InvokeAtServer(@delegate, null);
        }
    }
}
