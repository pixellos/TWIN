using Xunit;
using WebLedMatrix.Hubs;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Autofac.Integration.SignalR;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Moq;
using NSubstitute;
using NSubstitute.Extensions;
using NSubstitute.ReturnsExtensions;
using WebLedMatrix.Logic.ServerBrowser.Abstract;
using WebLedMatrix.Logic.ServerBrowser.Concrete;
using WebLedMatrixTests1;
using CallInfo = NSubstitute.Core.CallInfo;

namespace WebLedMatrix.Hubs.Tests
{
    public class UiManagerTests : BaseTest
    {
        private readonly ILoginStatusChecker _loginStatusChecker = new LoginStatusChecker();


        static IRequest getIdentityRequest(bool isAuthenticated, bool isAdministrator)
        {
            var request = Substitute.For<IRequest>();

            request.User.When(x=>x.IsInRole("Administrators")).DoNotCallBase();
            request.User.IsInRole("Administrators").Returns(isAdministrator);
           

            request.User.Identity.IsAuthenticated.Returns(isAuthenticated);

            return request;
        }

         UiManagerHub GetAccountHub(IRequest request)
        {
            var hub = new UiManagerHub(_loginStatusChecker,null);
            hub.Context = new HubCallerContext(request, "1");
            return hub;
        }

        public delegate void showSectionsDelegate(bool matrixesSection, bool sendingSection, bool administrationSection);


        public void CoreAccountTest(State expectedState, IRequest identityRequest)
        {
            UiManagerHub managerHub = Substitute.For<UiManagerHub>(_loginStatusChecker, null);
            managerHub.Context = new HubCallerContext(identityRequest,"1");

            string result = "";
            managerHub.Clients = Substitute.For<IHubCallerConnectionContext<IUiManagerHub>>();
            managerHub.Clients.When(x => { var r = x.Caller; }).DoNotCallBase();
            managerHub.Clients.Caller.WhenForAnyArgs(x=>x.loginStatus("")).Do(x=> { result = x[0].ToString(); });

            managerHub.LoginStatus();
            Assert.Equal(expectedState.ToString(),result);
        }

        [Fact()]
        public void NotLoggedCaseTest()
        {
            CoreAccountTest(State.NotLogged, getIdentityRequest(false, false));
        }

        [Fact()]
        public void LoggedCaseTest()
        {
            CoreAccountTest(State.Logged, getIdentityRequest(true, false));
        }

        [Fact()]
        public void AdminCaseTest()
        {
            CoreAccountTest(State.Admin, getIdentityRequest(true, true));
        }

    }
}