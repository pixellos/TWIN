using Xunit;
using WebLedMatrix.Logic.Authentication.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLedMatrix.Logic.Authentication.Infrastructure;
using NSubstitute;
using WebLedMatrix.Hubs;

namespace Test.WebLedMatrix.Logic.Authentication
{
    public class UserManagingTests
    {
        string userName = nameof(userName);
        string id = nameof(id);
        UserRepository repo = new UserRepository();

        public UserManagingTests()
        {
            repo.AddConnection(id, userName);
        }

        [Fact()]
        public void AddConnection()
        {
            Assert.True(repo.HubUsers.Any(x => x.UserName.Equals(userName) && x.Ids.Contains(id)));
        }

        [Fact()]
        public void Mute()
        {
            repo.SetMuteState(userName, isMuted: true);

            Assert.True(repo.IsMuted(userName));
        }

        [Fact]
        public void UnMute()
        {
            repo.SetMuteState(userName, isMuted: true);
            repo.SetMuteState(userName, isMuted: false);

            Assert.False(repo.IsMuted(userName));
        }
    }
}