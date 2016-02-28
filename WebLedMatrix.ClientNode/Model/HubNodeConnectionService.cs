using System;
using System.Net;
using System.Security.Policy;
using Microsoft.AspNet.SignalR.Client;
using StorageTypes;
using StorageTypes.AppInterface.ClientHub;
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
        private HubWrapper<ConnectionHubBase> _hubWrapper; 
        

        public HubNodeConnectionService(string url = null)
        {
            _hubWrapper = new HubWrapper<ConnectionHubBase>(new ConnectionHubBase(), Url);
            Url = url ?? Url;
            ServicePointManager.DefaultConnectionLimit = 10; // For WPF App
        }

        private void ConfigureHubMethods()
        {
//            _hubProxy.On("sendData", () =>
//            {
//                //Do something
//            });
        }

        public void SetConnection(string Url)
        {
//            _hubConnection = new HubConnection(Url);
//            _hubProxy = _hubConnection.CreateHubProxy(HubName);
//            ConfigureHubMethods();
        }

        public async void RequestData()
        {
//            var returnedData = await _hubProxy.Invoke<StorageTypes.DataToDisplay>("RequestForData");
        }

        public async void Start()
        {
            _hubWrapper.Start();
        }

        public void sendData(DataToDisplay data, object[] args = null)
        {
           Console.WriteLine("sendData");
        }

        public sealed override string ToString()
        {
            return base.ToString();
        }

        public void Hello()
        {
            Console.WriteLine("Hello");
        }
    }
}
