using System;
using System.Linq;
using System.Diagnostics;
using Microsoft.AspNet.SignalR;
using NLog;
using WebLedMatrix.Logic;
using WebLedMatrix.Logic.Authentication.Abstract;
using WebLedMatrix.Server.Logic.Text_Processing;
using System.Collections.Generic;
using WebLedMatrix.Models;

namespace WebLedMatrix.Hubs
{
    public class UiManagerHub : Hub<IUiManagerHub>
    {
        private static string LogInfoUserCheckedState = "User {0} has checked his authentication state {1}";
        private static string currentActiveUser = "";
        private static DateTime LastDate = DateTime.Now;
        private readonly ILoginStatusChecker StatusChecker;
        private readonly WebpageValidation Validator;
        private readonly Clients MatrixManager;
        private readonly HubConnections Repository;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
       
        private List<DelayedMessage> Messages { get; set; }

        private List<Session> Sessions { get; }

        public UiManagerHub(ILoginStatusChecker statusChecker, Clients matrixManager, HubConnections repository)
        {
            this.Sessions = new List<Session>();
            this.Validator = new WebpageValidation();
            this.Messages = new List<DelayedMessage>();
            this.StatusChecker = statusChecker;
            this.MatrixManager = matrixManager;
            this.Repository = repository;
        }

        public void IfNotMuted(Action x, string userName = null)
        {
            if (!Repository.IsMuted(userName ?? Context.User.Identity.Name))
            {
                x?.Invoke();
            }
        }

        public void SendUri(string data, string targetName)
        {
            this.RequestActivate(targetName);
            this.SendTo(Validator.ParseAddress(data), this.Context.User.Identity.Name);
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
           // if (currentActiveUser == user)
            //{
               // this.SendQueuedString(targetName, user);
                MatrixManager.AppendData(user, targetName, data);
            //}
            //else
            //{
               // this.Messages.Add(new DelayedMessage(user, targetName, data));
            //}
        }

        private void SendQueuedString(string targetName, string user)
        {
            var queuedStrings = this.Messages.Where(x => x.User == user && x.TargetId == targetName);
            this.Messages.RemoveAll(t => queuedStrings.Contains(t));
            foreach (var msg in queuedStrings)
            {
                MatrixManager.AppendData(user, targetName, msg.Data);
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
            Clients.Caller.loginStatus(StatusChecker.GetLoginStateString(Context.User));
            if (Context.User.Identity.IsAuthenticated)
            {
                Clients.Caller.showSections(matrixesSection: true, sendingSection: true, administrationSection: true);
                MatrixManager.UpdateMatrices();
            }
            _logger.Info(LogInfoUserCheckedState, Context.User.Identity.Name, Context.User.Identity.IsAuthenticated);
        }
    }
}