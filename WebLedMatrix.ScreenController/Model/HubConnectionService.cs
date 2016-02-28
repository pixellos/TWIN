using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace WebLedMatrix.ScreenController.Model
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

    public class HubConnectionService : IHubConnectionService
    {

        private IHubProxy _hubProxy;
        private HubConnection _hubConnection;
       
        const string HubName = "ConnectionHub";

        public HubConnectionService()
        {
            ServicePointManager.DefaultConnectionLimit = 10; // For WPF App
        }

        private void ConfigureHubMethods()
        {
            _hubProxy.On("sendData", () =>
            {
                //Do something
            });
        }

        public void SetConnection(string Url)
        {
            _hubConnection = new HubConnection(Url);
            _hubProxy = _hubConnection.CreateHubProxy(HubName);
            ConfigureHubMethods();
        }

        public async void RequestData()
        {
            var returnedData = await _hubProxy.Invoke<StorageTypes.DataToDisplay>("RequestForData");
        }

        public async void Start()
        {
            _hubConnection.Start();
        }
    }
}
