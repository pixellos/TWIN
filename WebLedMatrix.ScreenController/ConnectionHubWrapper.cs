using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Microsoft.AspNet.SignalR.Hubs;

namespace WebLedMatrix.Hubs
{
    public class ConnectionHubWrapper<T> where T : Hub
    {
        const string DefaultUrl = "localhost:8080";
        private string Url { get; set; }

        private IHub _hub;//
        private IHubProxy _hubProxy;
        private HubConnection _hubConnection;

        public ConnectionHubWrapper(IHub hub, string url = DefaultUrl)
        {
            _hub = hub;
            Url = url;
        }

        public void GetConnection()
        {
            _hubConnection = new HubConnection(DefaultUrl);
            _hubProxy = new HubProxy(_hubConnection,GetHubName());
        }

        public string GetHubName()
        {
            return _hub.GetType().Name;
        }

        public async Task<object> InvokeMethod(object x, object dataToSend)
        {
            if (x is MethodBase)
            {
                return await _hubProxy.Invoke<object>((x as MethodBase).Name);
            }
            throw new NotImplementedException();
        }

    }
}
