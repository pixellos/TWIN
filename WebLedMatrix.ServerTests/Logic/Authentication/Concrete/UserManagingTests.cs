using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using WebLedMatrix.Hubs;
using WebLedMatrix.Logic;
using WebLedMatrix.Models;
using Xunit;

namespace Test.WebLedMatrix.Server.Logic.Authentication.Concrete
{
    [ExcludeFromCodeCoverage]
    public class UserManagingTests
    {
        string userName = nameof(userName);
        string anotherId = nameof(anotherId); 
        string id = nameof(id);

        private bool OptionalPredicateDefault(HubUser model)
        {
            return true;
        }

        HubConnections _repository = new HubConnections();

        bool UserRepositoryPredicate_IsTherePredefinedUser(HubConnections repository, Predicate<HubUser> optionalPredicate = null)
         => repository.HubUserList.Any(x => x.UserName.Equals(userName) && x.Ids.Contains(id) && 
         (optionalPredicate?.Invoke(x) ?? true)
         );

        public UserManagingTests()
        {
            _repository.AddConnection(id, userName);
        }

        [Fact()]
        public void AddConnection_IsExist() //User was added in constructor
        {
            Assert.True(UserRepositoryPredicate_IsTherePredefinedUser(_repository));
        }

        [Fact()]
        public void AddConnection_IsAddingNewConnectionIdToExisiting() //User was added in constructor
        {
            _repository.AddConnection(anotherId,userName);

            Assert.True(UserRepositoryPredicate_IsTherePredefinedUser(_repository,x=>x.Ids.Contains(anotherId)));
        }

        [Fact]
        public void DeleteConnection()
        {
            _repository.DeleteConnection(id);
            
            Assert.False(UserRepositoryPredicate_IsTherePredefinedUser(_repository));
        }
        [Fact()]
        public void Mute()
        {
            _repository.SetMuteState(userName, isMuted: true);

            Assert.True(_repository.IsMuted(userName));
        }

        [Fact]
        public void UnMute()
        {
            _repository.SetMuteState(userName, isMuted: true);
            _repository.SetMuteState(userName, isMuted: false);

            Assert.False(_repository.IsMuted(userName));
        }
    }
}