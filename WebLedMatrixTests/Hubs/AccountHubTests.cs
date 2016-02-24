using WebLedMatrix.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using Moq;
using Xunit;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace WebLedMatrix.Hubs.Tests
{
    public class AccountHubTests
    {
        [Fact]
        public void AmILoggedTest()
        {
            string messages = "";
            var hub = new AccountHub();
            var mock = new Mock<IHubCallerConnectionContext<dynamic>>();

            hub.Clients = mock.Object;
            dynamic all = new ExpandoObject();

            all.loginStatus = new Action<string>((message) => { messages = message; });
            mock.Setup(m => m.All).Returns((ExpandoObject)all);
            hub.AmILogged();
            Assert.True(messages.Equals(AccountHub.State.NotLogged.ToString()) );
        }
    }
}