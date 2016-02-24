using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;

namespace WebLedMatrix.Hubs
{
    public class AccountHub : Hub
    {
        enum State
        {
            Admin = 2,
            Logged = 1,
            NotLogged = 0
        }

        public void AmILogged()
        {
            var user = Context.User;
    
            if (user.Identity.IsAuthenticated)
            {
                if (user.IsInRole("Administrators"))
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
        }
    }
}