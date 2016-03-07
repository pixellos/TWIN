using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using NLog;
using StorageTypes;
using StorageTypes.HubWrappers;
using WebLedMatrix.Logic.ServerBrowser.Abstract;

namespace WebLedMatrix.Hubs
{
    public interface IUiManagerHub
    {
        void updateMatrixes(string name);
        void loginStatus(string userText);
        void showSections(bool matrixesSection,bool sendingSection,bool administrationSection);
    }

    public class Something
    {
        public IHubCallerConnectionContext<dynamic> Caller;
        public Action<IHubCallerConnectionContext<dynamic>> x;
    }

    public class UiManagerHub : Hub<IUiManagerHub>
    {
        private readonly ILoginStatusChecker _loginStatusChecker;
        private readonly IMatrixManager _matrixManager;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        
        private static string LogInfoUserCheckedState = "User {0} has checked his authentication state {1}";

        public UiManagerHub(ILoginStatusChecker statusChecker, IMatrixManager matrixManager)
        {
            _loginStatusChecker = statusChecker;
            _matrixManager = matrixManager;
        }

        public UiManagerHub(ILoginStatusChecker statusChecker, IMatrixManager matrixManager, Something something)
        {
            _loginStatusChecker = statusChecker;
            _matrixManager = matrixManager;
        }

        public void DisplayText(Matrix matrix, string text)
        {
            _matrixManager.SendData(matrix, new DataToDisplay() {Data =  text,DisplayDataType = DisplayDataType.Text});
        }

        public void OtherMethods()
        {
            var name = "x";
            Clients.All.updateMatrixes(name);
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

            _logger.Info(LogInfoUserCheckedState, Context.User.Identity.Name, Context.User.Identity.IsAuthenticated);
        }
    }
}