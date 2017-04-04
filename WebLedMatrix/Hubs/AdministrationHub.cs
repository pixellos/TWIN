using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using WebLedMatrix.Logic;
using WebLedMatrix.Models;

namespace WebLedMatrix.Hubs
{
    public class AdministrationHub : Hub
    {
        private static Dictionary<AdministrationHub, AdministrationModel> _models =
            new Dictionary<AdministrationHub, AdministrationModel>();

        private Clients _matrixManager;

        public AdministrationHub(Clients matrixManager)
        {
            _matrixManager = matrixManager;
        }

        public void GetUsers()
        {
            this.Clients.Caller.activeUsers(HubConnections.Repository.HubUserList);
        }

        public void MuteUser(string name)
        {                                                                                       
            HubConnections.Repository.SetMuteState(name,true);
            this.GetUsers();
        }

        public void UnMuteUser(string name)
        {
            HubConnections.Repository.SetMuteState(name, false);
            this.GetUsers();
        }

        public override Task OnConnected()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                HubConnections.Repository.AddConnection(Context.ConnectionId, Context.User.Identity.Name);
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
                HubConnections.Repository.DeleteConnection(Context.ConnectionId);
            }
            GetUsers();
            return base.OnDisconnected(stopCalled);
        }   
    }
}