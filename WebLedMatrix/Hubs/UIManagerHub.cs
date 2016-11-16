using System;
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

        public void UpClick(string sendername)
        {
            //This
            if (sendername != "")
                this._matrixManager.SendToUser(_currentActiveUser, "Up was clicked", sendername);
            /*Is now used instead
                this._matrixManager.SendToAll(_currentActiveUser, "Up was clicked");
            */
        }

        public void DownClick(string sendername)
        {
            if (sendername != "")
                this._matrixManager.SendToUser(_currentActiveUser, "Down was clicked", sendername);
        }
        public void LeftClick(string sendername)
        {
            if (sendername != "")
                this._matrixManager.SendToUser(_currentActiveUser, "Left was clicked", sendername);
        }

        public void RightClick(string sendername)
        {
            if (sendername != "")
                this._matrixManager.SendToUser(_currentActiveUser, "right was clicked", sendername);
        }

        public void OkClick(string sendername)
        {
            if (sendername != "")
                this._matrixManager.SendToUser(_currentActiveUser, "Ok was clicked", sendername);
        }


        public void SendText(string data, string name)
        {
            RequestActivate();
            IfNotMuted(() =>
            {
                if (this.Context.User.Identity.Name.Equals(this._currentActiveUser))
                {
                    _matrixManager.AppendData(this.Context.User.Identity.Name, name, data);
                }
            });
        }

        public void RequestActivate()
        {
            IfNotMuted(() =>
                {
                    if ((DateTime.Now - LastDate) > new TimeSpan(0,0,0,20))
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