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
        public enum State
        {
            Admin = 2,
            Logged = 1,
            NotLogged = 0
        }

        public void LoginStatus()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                if (Context.User.IsInRole("Administrators"))//Context.User.IsInRole("Administrators"))
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