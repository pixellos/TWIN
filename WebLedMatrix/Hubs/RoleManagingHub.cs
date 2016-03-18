using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using WebLedMatrix.Controllers;
using WebLedMatrix.Logic.Authentication.Abstract;
using WebLedMatrix.Logic.Authentication.Infrastructure;
using WebLedMatrix.Logic.Authentication.Models.Roles;

namespace WebLedMatrix.Hubs
{
    public static class extensions
    {
        public static bool IsCurrentUserInRole(this RoleManagingHub managingHub,State state)
        {
            return managingHub.Context.User.IsInRole(state.ToString());
        }
    }

    public class RoleManagingHub : Hub
    {
        private UserIdentityManager UserManager
        {
            get
            {
                return  Context.Request.GetHttpContext().GetOwinContext().GetUserManager<UserIdentityManager>();
            }
        }
        private AppRoleManager RoleManager
        {
            get
            {
                return Context.Request.GetHttpContext().GetOwinContext().GetUserManager<AppRoleManager>();
            }
        }
        private ILoginStatusChecker _loginStatusChecker;
        private RoleAdministrating _roleAdministrating => new RoleAdministrating(RoleManager,UserManager);

        public RoleManagingHub(ILoginStatusChecker loginStatusChecker)
        {
            _loginStatusChecker = loginStatusChecker;
        }

        public  List<AppRole> GetRoles()
        {
            if (this.IsCurrentUserInRole(State.Admin))
            {
                return _roleAdministrating.GetRoles().ToList();
            }
            return new List<AppRole>();
        }
    }
}