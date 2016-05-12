using System.Collections.Generic;
using System.Linq;
using WebLedMatrix.Hubs;
using WebLedMatrix.Models;

namespace WebLedMatrix.Logic
{
    public class HubConnections
    {
        public static HubConnections Repository = new HubConnections();
        public List<HubUser> HubUserList = new List<HubUser>();
        private readonly object _lock = new object();

        public void DeleteConnection(string connectionId)
        {
            lock (_lock)
            {
                var user = HubUserList.Single(x => x.Ids.Any(y=>y.Equals(connectionId)));
                user.Ids.Remove(connectionId);
                if (! user.Ids.Any())
                {
                    HubUserList.Remove(user);
                }
            }
        }

        public bool IsMuted(string userName)
        {
            lock (_lock)
            {
                return HubUserList.Single(x => x.UserName.Equals(userName)).IsMuted;
            }
        }

        public void SetMuteState(string userName,bool isMuted)
        {
            lock (_lock)
            {
                HubUserList.Single(x => x.UserName.Equals(userName)).IsMuted = isMuted;
            }
        }

        public void AddConnection(string connectionId, string userName)
        {
            lock (_lock)
            {
                HubUser existingHubUser = null;
                if (HubUserList.Any(x=>x.UserName.Equals(userName)))
                {
                    existingHubUser = HubUserList.Single(x => x.UserName.Equals(userName));
                }

                if (existingHubUser != null)
                {
                    existingHubUser.Ids.Add(connectionId);
                }
                else
                {
                    HubUserList.Add(new HubUser(userName, connectionId));
                }
            }
        }
    }
}