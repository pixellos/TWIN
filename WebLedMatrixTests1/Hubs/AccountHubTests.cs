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
using WebLedMatrix.Logic.ServerBrowser.Abstract;
using WebLedMatrix.Logic.ServerBrowser.Concrete;
using WebLedMatrixTests1;

namespace WebLedMatrix.Hubs.Tests
{
    public class AccountHubTests : BaseTest
    {
        private readonly ILoginStatusChecker _loginStatusChecker = new LoginStatusChecker();


        static Mock<IRequest> getRequestMock(bool isAuthenticated, bool isAdministrator)
        {
            var request = new Mock<IRequest>();

            request.Setup(x => x.User.IsInRole("Administrators")).Returns(isAdministrator);
            request.SetupGet(z => z.User.Identity.IsAuthenticated).Returns(isAuthenticated);
            return request;
        }

         UIManagerHub GetAccountHub(IRequest request, IHubCallerConnectionContext<dynamic> client)
        {
            var hub = new UIManagerHub(_loginStatusChecker);
            hub.Context = new HubCallerContext(request, "1");
            hub.Clients = client;
            return hub;
        }

        public void CoreAccountTest(State expectedState, IRequest clientRequest)
        {
            var mockClient = new Mock<IHubCallerConnectionContext<dynamic>>();
            var hub = GetAccountHub(clientRequest, mockClient.Object);

            string status = "";
            dynamic caller = new ExpandoObject();
            caller.loginStatus = new Action<string>((message) => status = message);

            mockClient.Setup(x => x.Caller).Returns((ExpandoObject)caller);
            hub.LoginStatus();

            Assert.Equal(expectedState.ToString(), status);
        }

        [Fact()]
        public void NotLoggedCaseTest()
        {
            CoreAccountTest(State.NotLogged, getRequestMock(false, false).Object);
        }

        [Fact()]
        public void LoggedCaseTest()
        {
            CoreAccountTest(State.Logged, getRequestMock(true, false).Object);
        }

        [Fact()]
        public void AdminCaseTest()
        {
            CoreAccountTest(State.Admin, getRequestMock(true, true).Object);
        }

    }
}