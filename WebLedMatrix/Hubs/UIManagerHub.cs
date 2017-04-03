using System;
using System.Linq;
using System.Diagnostics;
using Microsoft.AspNet.SignalR;
using NLog;
using WebLedMatrix.Logic;
using WebLedMatrix.Logic.Authentication.Abstract;
using WebLedMatrix.Server.Logic.Text_Processing;
using System.Collections.Generic;

namespace WebLedMatrix.Hubs
{
    public class UiManagerHub : Hub<IUiManagerHub>
    {
        private static string LogInfoUserCheckedState = "User {0} has checked his authentication state {1}";
        private readonly ILoginStatusChecker _loginStatusChecker;
        private readonly WebpageValidation _webpageValidation;
        private readonly MatrixManager _matrixManager;
        private readonly HubConnections _repository;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static string currentActiveUser = "";
        private static DateTime LastDate = DateTime.Now;
        private List<Tuple<string, string, string>> UserToTargetToData { get; set; }

        public UiManagerHub(ILoginStatusChecker statusChecker, MatrixManager matrixManager, HubConnections repository)
        {
            _loginStatusChecker = statusChecker;
            _matrixManager = matrixManager;
            _repository = repository;
            _webpageValidation = new WebpageValidation();
            this.UserToTargetToData = new List<Tuple<string, string, string>>();
        }

        public void IfNotMuted(Action x, string userName = null)
        {
            if (!_repository.IsMuted(userName ?? Context.User.Identity.Name))
            {
                x?.Invoke();
            }
        }

        public void SendUri(string data, string name)
        {
            RequestActivate();
            this.SendTo(_webpageValidation.ParseAddress(data), this.Context.User.Identity.Name);
        }

        private void TestAndSend(string text)
        {
            this.SendTo(currentActiveUser, text);
        }

        public void UpClick(string targetName)
        {
            this.SendTo("Up", targetName);
        }

        public void DownClick(string targetName)
        {
            this.SendTo("Down", targetName);
        }

        public void LeftClick(string targetName)
        {
            this.SendTo("Left", targetName);
        }

        public void RightClick(string targetName)
        {
            this.SendTo("Right", targetName);
        }

        public void OkClick(string targetName)
        {
            this.SendTo("OK", targetName);
        }
        public void ExitClick(string targetName)
        {
            this.SendTo("Exit", targetName);
        }

        public void SendText(string data, string targetName)
        {
            this.SendTo(data, targetName);
        }

        private void SendTo(string data, string targetName)
        {
            var user = this.Context.User.Identity.Name;
            if (currentActiveUser == user)
            {
                this.SendQueuedString(targetName, user);
                _matrixManager.AppendData(user, targetName, data);
            }
            else
            {
                this.UserToTargetToData.Add(new Tuple<string, string, string>(user, targetName, data));
            }
        }

        private void SendQueuedString(string targetName, string user)
        {
            var queuedStrings = this.UserToTargetToData.Where(x => x.Item1 == user && x.Item2 == targetName);
            this.UserToTargetToData.RemoveAll(t => queuedStrings.Contains(t));
            foreach (var str in queuedStrings)
            {
                _matrixManager.AppendData(user, targetName, str.Item3);
            }
        }
 
        //Todo: Add selected matrix for user request
        public void RequestActivate(string targetName)
        {
            IfNotMuted(() =>
            {
                if ((DateTime.Now - LastDate) > new TimeSpan(0, 0, 0, 200))
                {
                    var user = this.Context.User.Identity.Name;
                    this.SendQueuedString(targetName, user);
                    currentActiveUser = this.Context.User.Identity.Name;
                    this.Clients.All.userIsActiveStatus(false);
                    this.Clients.Caller.userIsActiveStatus(true);
                    UiManagerHub.LastDate = DateTime.Now;
                }
            }
            );
        }

        public void LoginStatus()
        {
            Clients.Caller.loginStatus(
                _loginStatusChecker.GetLoginStateString(Context.User));
            if (Context.User.Identity.IsAuthenticated)
            {
                Clients.Caller.showSections(matrixesSection: true, sendingSection: true, administrationSection: true);
                _matrixManager.UpdateMatrices();
            }
            _logger.Info(LogInfoUserCheckedState, Context.User.Identity.Name, Context.User.Identity.IsAuthenticated);
        }
    }
}