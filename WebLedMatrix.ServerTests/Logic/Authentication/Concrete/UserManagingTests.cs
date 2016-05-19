using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Policy;
using WebLedMatrix.Hubs;
using WebLedMatrix.Logic;
using WebLedMatrix.Models;
using Xunit;
using static Test.WebLedMatrix.Server.Logic.Authentication.Concrete.Constants;

namespace Test.WebLedMatrix.Server.Logic.Authentication.Concrete
{
    public static class Constants
    {
        public static string UserName = nameof(UserName);
        public static string AnotherId = nameof(AnotherId);
        public static string Id = nameof(Id);
    }
    [ExcludeFromCodeCoverage]
    public class UserManagingFixture : IDisposable
    {
        public HubConnections Repository;

        public UserManagingFixture()
        {
            Repository = new HubConnections();
            Repository.AddConnection(Id, UserName);
        }

        public void Dispose()
        {
            Repository = null;
        }
    }

    [ExcludeFromCodeCoverage]
    public class UserManagingTests : IClassFixture<UserManagingFixture>
    {
        private UserManagingFixture _fixture;

        public UserManagingTests(UserManagingFixture fixture)
        {
            _fixture = fixture;
        }

        private bool OptionalPredicateDefault(HubUser model)
        {
            return true;
        }

        HubConnections _repository = HubConnections.Repository;

        bool UserRepositoryPredicate_IsTherePredefinedUser(HubConnections repository, Predicate<HubUser> optionalPredicate = null)
         => _fixture.Repository.HubUserList.Any(x => x.UserName.Equals(UserName) && x.Ids.Contains(Id) && 
         (optionalPredicate?.Invoke(x) ?? true));
  

        [Fact()]
        public void AddConnection_IsExist() //User was added in constructor
        {
            Assert.True(UserRepositoryPredicate_IsTherePredefinedUser(_fixture.Repository));
        }

        [Fact()]
        public void AddConnection_IsAddingNewConnectionIdToExisiting() //User was added in constructor
        {
            _fixture.Repository.AddConnection(AnotherId,UserName);

            Assert.True(UserRepositoryPredicate_IsTherePredefinedUser(_fixture.Repository,x=>x.Ids.Contains(AnotherId)));
        }

        [Fact]
        public void DeleteConnection()
        {
            _fixture.Repository.DeleteConnection(Id);
            
            Assert.False(UserRepositoryPredicate_IsTherePredefinedUser(_fixture.Repository));
        }
        [Fact()]
        public void Mute()
        {
            _fixture.Repository.SetMuteState(UserName, isMuted: true);

            Assert.True(_fixture.Repository.IsMuted(UserName));
        }

        [Fact]
        public void UnMute()
        {
            _fixture.Repository.SetMuteState(UserName, isMuted: true);
            _fixture.Repository.SetMuteState(UserName, isMuted: false);

            Assert.False(_fixture.Repository.IsMuted(UserName));
        }
    }
}