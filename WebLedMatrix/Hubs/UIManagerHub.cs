using System;
using System.Diagnostics;
using Microsoft.AspNet.SignalR;
using NLog;
using WebLedMatrix.Logic;
using WebLedMatrix.Logic.Authentication.Abstract;
using WebLedMatrix.Server.Logic.Text_Processing;

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
        private string _currentActiveUser = "";
        private static DateTime LastDate = DateTime.Now;

        public UiManagerHub(ILoginStatusChecker statusChecker, MatrixManager matrixManager, HubConnections repository)
        {
            _loginStatusChecker = statusChecker;
            _matrixManager = matrixManager;
            _repository = repository;
            _webpageValidation = new WebpageValidation();
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
            IfNotMuted(() =>
                {
                    if (this.Context.User.Identity.Name.Equals(this._currentActiveUser))
                    {
                        _matrixManager.AppendData(this.Context.User.Identity.Name, name, _webpageValidation.ParseAddress(data));
                    }
                }
                );
        }

        private void TestAndSend(string text)
        {
            this.SendTo(_currentActiveUser, text);
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
            _matrixManager.AppendData(this.Context.User.Identity.Name, targetName, data);
        }

        public void RequestActivate()
        {
            IfNotMuted(() =>
                {
                    if ((DateTime.Now - LastDate) > new TimeSpan(0, 0, 0, 20))
                    {
                        this._currentActiveUser = this.Context.User.Identity.Name;
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