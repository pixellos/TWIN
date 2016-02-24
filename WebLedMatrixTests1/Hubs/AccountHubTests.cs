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
using Moq;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace WebLedMatrix.Hubs.Tests
{
    public class AccountHubTests
    {
        static Mock<IRequest> getRequestMock(bool isAuthenticated, bool isAdministrator)
        {
            var request = new Mock<IRequest>();

            request.Setup(x => x.User.IsInRole("Administrators")).Returns(isAdministrator);
            request.SetupGet(z => z.User.Identity.IsAuthenticated).Returns(isAuthenticated);
            request.SetupGet(x => x.User.Identity.Name).Returns("TestMode");
            return request;
        }

        static AccountHub GetAccountHub(IRequest request, IHubCallerConnectionContext<dynamic> client )
        {
            var hub = new AccountHub();
            hub.Context = new HubCallerContext(request, "1");
            hub.Clients = client;
            return hub;
        }

        public void CoreAccountTest(AccountHub.State expectedState, IRequest clientRequest)
        {
            var mockClient = new Mock<IHubCallerConnectionContext<dynamic>>();
            var hub = GetAccountHub(clientRequest, mockClient.Object);

            string status = "";
            dynamic caller = new ExpandoObject();
            mockClient.Setup(x => x.Caller).Returns((ExpandoObject)caller);
            caller.loginStatus = new Action<string>(message => status = message);
           
            hub.LoginStatus();

            Assert.Equal(expectedState.ToString(), status);
        }

        [Fact]
        public void NotLoggedCaseTest()
        {
            CoreAccountTest(AccountHub.State.NotLogged, getRequestMock(false, false).Object);
        }

        [Fact]
        public void LoggedCaseTest()
        {
            CoreAccountTest(AccountHub.State.Logged, getRequestMock(true, false).Object);
        }

        [Fact]
        public void AdminCaseTest()
        {
            CoreAccountTest(AccountHub.State.Admin, getRequestMock(true, true).Object);
        }

    }
}