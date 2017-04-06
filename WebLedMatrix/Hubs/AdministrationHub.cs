using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using WebLedMatrix.Logic;
using WebLedMatrix.Models;

namespace WebLedMatrix.Hubs
{
    public class AdministrationHub : Hub
    {
        private static Dictionary<AdministrationHub, AdministrationModel> Models = new Dictionary<AdministrationHub, AdministrationModel>();

        private Clients ConnectedClients;
        private HubConnections HubConnection;

        public AdministrationHub(Clients clients, HubConnections connection)
        {
            this.ConnectedClients = clients;
            this.HubConnection = connection;
        }

        public void GetUsers()
        {
            this.Clients.Caller.activeUsers(this.HubConnection.HubUserList);
        }

        public void MuteUser(string name)
        {                                                                                       
            this.HubConnection.SetMuteState(name,true);
            this.GetUsers();
        }

        public void UnMuteUser(string name)
        {
            this.HubConnection.SetMuteState(name, false);
            this.GetUsers();
        }

        public override Task OnConnected()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                this.HubConnection.AddConnection(Context.ConnectionId, Context.User.Identity.Name);
            }
            this.GetUsers();
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            OnConnected();
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                this.HubConnection.DeleteConnection(Context.ConnectionId);
            }
            GetUsers();
            return base.OnDisconnected(stopCalled);
        }   
    }
}