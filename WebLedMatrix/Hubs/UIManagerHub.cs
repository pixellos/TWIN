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
    public class UIManagerHub : Hub
    {
        private ILoginStatusChecker _loginStatusChecker;
        private Logger logger = LogManager.GetCurrentClassLogger();
        private static string LogInfoUserCheckedState = "User {0} has checked his authentication state {1}";

        //Need class for containing 


        public UIManagerHub(ILoginStatusChecker statusChecker)
        {
            _loginStatusChecker = statusChecker;
        }

        public void LoginStatus()
        {
            Clients.Caller.loginStatus(
                _loginStatusChecker.GetLoginStateString(Context.User));
            if (_loginStatusChecker.GetLoginStateString(Context.User).Equals("NotLogged"))
            {
                Clients.Caller.showSections(matrixesSection: false, sendingSection: false, administrationSection: false);
            }
            else
            {
                Clients.Caller.showSections(matrixesSection: true, sendingSection: true, administrationSection: true);
            }

            logger.Info(LogInfoUserCheckedState, Context.User.Identity.Name, Context.User.Identity.IsAuthenticated);
        }
    }
}