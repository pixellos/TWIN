using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.WebSockets;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Logging;
using NLog;
using WebLedMatrix.Controllers;
using WebLedMatrix.Logic.Authentication.Infrastructure;
using WebLedMatrix.Logic.Authentication.Models.Roles;
using static System.Web.HttpContext;

namespace WebLedMatrix.Hubs
{
    public class AdministrationHub : Hub
    {
        private static Dictionary<AdministrationHub, AdministrationModel> _models =
            new Dictionary<AdministrationHub, AdministrationModel>();

        private MatrixManager _matrixManager;

        public AdministrationHub(MatrixManager matrixManager)
        {
            _matrixManager = matrixManager;
        }

        public void GetUsers()
        {
            Clients.Caller.activeUsers(UserRepository.Repository.HubUsers);
        }

        public async void MuteUser(string name)
        {
            UserRepository.Repository.SetMuteState(name,true);
            Clients.All.activeUsers(UserRepository.Repository.HubUsers);
        }

        public async void UnMuteUser(string name)
        {
            UserRepository.Repository.SetMuteState(name, false);
            Clients.All.activeUsers(UserRepository.Repository.HubUsers);
        }

        public override Task OnConnected()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                UserRepository.Repository.AddConnection(Context.ConnectionId, Context.User.Identity.Name);
            }
            Clients.All.activeUsers(UserRepository.Repository.HubUsers);

            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                UserRepository.Repository.AddConnection(Context.ConnectionId,Context.User.Identity.Name);
            }
            Clients.All.activeUsers(UserRepository.Repository.HubUsers);

            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                UserRepository.Repository.DeleteConnection(Context.ConnectionId);
            }
            Clients.All.activeUsers(UserRepository.Repository.HubUsers);
            return base.OnDisconnected(stopCalled);
        }
    }
}
