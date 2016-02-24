using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;
using NLog;

namespace WebLedMatrix.Hubs
{
    using System.Security.Principal;

    public class AccountHub : Hub
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        private static string LogInfoUserCheckedState = "User {0} has checked his authentication state {1}";

        public enum State
        {
            Admin = 2,
            Logged = 1,
            NotLogged = 0
        }

        public void LoginStatus()
        {
            IPrincipal user = Context.User;
            if (user.Identity.IsAuthenticated)
            {
                if (user.IsInRole("Administrators"))//Context.User.IsInRole("Administrators"))
                {
                    Clients.Caller.loginStatus(State.Admin.ToString());
                }
                else
                {
                    Clients.Caller.loginStatus(State.Logged.ToString());
                }
            }
            else
            {
                Clients.Caller.loginStatus(State.NotLogged.ToString());
            }

            logger.Info(LogInfoUserCheckedState, user.Identity.Name, user.Identity.IsAuthenticated);
        }
    }
}