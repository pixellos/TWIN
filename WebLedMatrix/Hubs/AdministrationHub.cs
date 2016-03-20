using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using WebLedMatrix.Controllers;
using WebLedMatrix.Logic.Authentication.Infrastructure;
using WebLedMatrix.Logic.Authentication.Models.Roles;
using static System.Web.HttpContext;

namespace WebLedMatrix.Hubs
{
    public class AdministrationModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

    }

    public static class Repo
    {
        public static Dictionary<string,string> Dictionary = new Dictionary<string, string>(); 
    }

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
            if (Context.User.Identity.IsAuthenticated)
            {
                Repo.Dictionary.Add(Context.ConnectionId, Context.User.Identity.Name);
                Clients.All.activeUsers(new HashSet<string>(Repo.Dictionary.Select(x => x.Value)));
            }
        }

        public async void MuteUser(string name)
        {
            RoleAdministrating role = new RoleAdministrating(HttpContext.Current.GetOwinContext().GetUserManager<AppRoleManager>(), Current.GetOwinContext().GetUserManager<UserIdentityManager>());
            await role.AddMembers("Muted", new List<string>() { Current.GetOwinContext().GetUserManager<UserIdentityManager>().FindByName(name).Id });
        }

        public async void UnMuteUser(string name)
        {
            RoleAdministrating role = new RoleAdministrating(HttpContext.Current.GetOwinContext().GetUserManager<AppRoleManager>(), Current.GetOwinContext().GetUserManager<UserIdentityManager>());
            try
            {
                await role.DeleteMembers("Muted", new List<string>() { Current.GetOwinContext().GetUserManager<UserIdentityManager>().FindByName(name).Id });
            }
            catch (Exception)
            {}

        }

        public override Task OnReconnected()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                Repo.Dictionary.Add(Context.ConnectionId, Context.User.Identity.Name);
            }
            
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            try
            {
                if (Context.User.Identity.IsAuthenticated)
                {

                    Repo.Dictionary.Remove(Context.ConnectionId);
                }
            }
            catch (Exception) { }
           
            Clients.All.activeUsers(new HashSet<string>(Repo.Dictionary.Select(x => x.Value)));
            return base.OnDisconnected(stopCalled);
        }
    }
}
