using Xunit;
using WebLedMatrix.Hubs;
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using NSubstitute;
using WebLedMatrix;
using WebLedMatrix.Controllers.Authentication.Models;
using WebLedMatrix.Logic;
using WebLedMatrix.Logic.Authentication.Abstract;
using WebLedMatrix.Server.Logic.Text_Processing;

namespace Test.WebLedMatrix.Server.Hubs
{
    [ExcludeFromCodeCoverage]
    public class UiManagerFixture : IDisposable
    {
        public UiManagerHub ManagerHub;
        public Clients MatrixManagerInsideManagerHub;

        public string UserName = nameof(UserName);

        public UiManagerFixture()
        {
            MatrixManagerInsideManagerHub = new Clients();
            ManagerHub = new UiManagerHub(new LoginStatusChecker(), MatrixManagerInsideManagerHub,HubConnections.Repository);

            HubConnections.Repository.AddConnection("",UserName);
            HubConnections.Repository.SetMuteState(UserName,false);
        }

        public void Dispose()
        {
        }
    }

    [ExcludeFromCodeCoverage]
    public class UiManagerTests : BaseTest, IClassFixture<UiManagerFixture>
    {
        private readonly ILoginStatusChecker _loginStatusChecker = new LoginStatusChecker();

        private UiManagerFixture _fixture;
        public UiManagerTests(UiManagerFixture fixture)
        {
            _fixture = fixture;
        }

        static IRequest GetIdentityRequest(bool isAuthenticated)
        {
            var request = Substitute.For<IRequest>();
            
            request.User.Identity.IsAuthenticated.Returns(isAuthenticated);

            return request;
        }

        public void CoreAccountTest(State expectedState, IRequest identityRequest)
        {
            var matrixManager = Substitute.For<Clients>();
            matrixManager.When(x=>x.UpdateMatrices()).DoNotCallBase();

            UiManagerHub managerHub = Substitute.For<UiManagerHub>(_loginStatusChecker, matrixManager,HubConnections.Repository);
            managerHub.Context = new HubCallerContext(identityRequest,"1");
            managerHub.Clients = Substitute.For<IHubCallerConnectionContext<IUiManagerHub>>();

            string result = "";
            managerHub.Clients.When(x => { var r = x.Caller; }).DoNotCallBase();
            managerHub.Clients.Caller.WhenForAnyArgs(x=>x.loginStatus("")).Do(x=> { result = x[0].ToString(); });

            managerHub.LoginStatus();
            Assert.Equal(expectedState.ToString(),result);
        }

        [Fact]
        public void IfMuted()
        {
            bool HasCallbackBeenHited = false;
            _fixture.ManagerHub.IfNotMuted(()=>HasCallbackBeenHited = true,_fixture.UserName);
            
            Assert.True(HasCallbackBeenHited);
        }


        [Fact()]
        public void NotLoggedCaseTest()
        {
            CoreAccountTest(State.NotLogged, GetIdentityRequest(false));
        }

        [Fact()]
        public void LoggedCaseTest()
        {
            CoreAccountTest(State.Logged, GetIdentityRequest(true));
        }
    }
}