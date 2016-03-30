using System.Collections.Generic;
using System.Linq;

namespace WebLedMatrix.Hubs
{
    public class UserRepository
    {
        public static UserRepository Repository = new UserRepository();
        public List<HubUserModel> HubUsers = new List<HubUserModel>();
        private readonly object _lock = new object();

        public void DeleteConnection(string connectionId)
        {
            lock (_lock)
            {
                var user = HubUsers.Single(x => x.Ids.Any(y=>y.Equals(connectionId)));
                user.Ids.Remove(connectionId);
                if (! user.Ids.Any())
                {
                    HubUsers.Remove(user);
                }
            }
        }

        public void SetMuteState(string userName,bool isMuted)
        {
            lock (_lock)
            {
                HubUsers.Single(x => x.UserName.Equals(userName)).IsMuted = isMuted;
            }
        }

        public void AddConnection(string connectionId, string userName)
        {
            lock (_lock)
            {
                HubUserModel existingHubUser = null;
                if (HubUsers.Any(x=>x.UserName.Equals(userName)))
                {
                    existingHubUser = HubUsers.Single(x => x.UserName.Equals(userName));
                }

                if (existingHubUser != null)
                {
                    existingHubUser.Ids.Add(connectionId);
                }
                else
                {
                    var model = new HubUserModel(userName,connectionId);
                    HubUsers.Add(model);
                }
            }
        }
    }
}