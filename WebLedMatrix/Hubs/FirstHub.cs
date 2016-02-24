using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace WebLedMatrix
{
    public class FirstHub : Hub
    {
        public void SendMessage(string message)
        {
            var user = Context.User;
            if (user.IsInRole("Administrators"))
            {
                Clients.All.newMessage(string.Format("{0}: {1}", DateTime.Now, message));
            }
            else
            {
                Clients.Caller.newMessage("Dont have permissions");
            }
        }

        public void Kill()
        {
            var x = (new Random()).Next(0,10);
            if (x < 6)
            {
                Clients.All.getKill("You're Killed");
            }
            else
            {
                Clients.All.getKill("You're alive");
            }
        }
    }
}