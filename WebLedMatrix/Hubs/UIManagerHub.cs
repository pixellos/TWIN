using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using NLog;
using StorageTypes;
using WebLedMatrix.Logic.Authentication.Abstract;

namespace WebLedMatrix.Hubs
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()

    public class UiManagerHub : Hub<IUiManagerHub>
    {
        private readonly ILoginStatusChecker _loginStatusChecker;
        private readonly MatrixManager _matrixManager;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        
        private static string LogInfoUserCheckedState = "User {0} has checked his authentication state {1}";

        public UiManagerHub(ILoginStatusChecker statusChecker, MatrixManager matrixManager)
        {
            _loginStatusChecker = statusChecker;
            _matrixManager = matrixManager;
        }

        public void SendUri(string data,string name)
        {
            _matrixManager.SendCommandToMatirx(name,DisplayDataType.WebPage, data);
        }

        public void SendText(string data, string name)
        {
            _matrixManager.SendCommandToMatirx(name,DisplayDataType.Text, data);
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
                _matrixManager.UpdateMatrices();
            }
            _logger.Info(LogInfoUserCheckedState, Context.User.Identity.Name, Context.User.Identity.IsAuthenticated);
        }
    }
}