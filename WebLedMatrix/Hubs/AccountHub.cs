using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;
using NLog;
using WebLedMatrix.Logic.ServerBrowser.Abstract;

namespace WebLedMatrix.Hubs
{
    public class AccountHub : Hub
    {
        private ILoginStatusChecker _loginStatusChecker;
        private Logger logger = LogManager.GetCurrentClassLogger();
        private static string LogInfoUserCheckedState = "User {0} has checked his authentication state {1}";

        public AccountHub(ILoginStatusChecker statusChecker)
        {
            _loginStatusChecker = statusChecker;
        }

        public void LoginStatus()
        {
            Clients.Caller.loginStatus(
                _loginStatusChecker.GetLoginStateString(Context.User));
            logger.Info(LogInfoUserCheckedState, Context.User.Identity.Name, Context.User.Identity.IsAuthenticated);
        }
    }
}