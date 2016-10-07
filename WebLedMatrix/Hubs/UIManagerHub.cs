using System;
using System.ServiceModel.Security;
using Microsoft.AspNet.SignalR;
using NLog;
using WebLedMatrix.Logic;
using WebLedMatrix.Logic.Authentication.Abstract;
using WebLedMatrix.Logic.Text_Processing;
using static WebLedMatrix.Logic.HubConnections;

namespace WebLedMatrix.Hubs
{
    public class UiManagerHub : Hub<IUiManagerHub>
    {
        private readonly ILoginStatusChecker _loginStatusChecker;
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

        private static string LogInfoUserCheckedState = "User {0} has checked his authentication state {1}";
        private readonly WebpageValidation _webpageValidation;


        public void SendUri(string data, string name)
        {
            RequestActivate();
            IfNotMuted(() =>
                {
                    if (this.Context.User.Identity.Name.Equals(this._currentActiveUser))
                    {
                        _matrixManager.SendWebPage(name, _webpageValidation.ParseAddress(data));
                    }
                }
                );
        }

        public void UpClick()
        {
            this._matrixManager.SendToAll("Up was clicked");
        }


        public void SendText(string data, string name)
        {
            RequestActivate();
            IfNotMuted(() =>
            {
                if (this.Context.User.Identity.Name.Equals(this._currentActiveUser))
                {
                    _matrixManager.SendText(name, data);
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