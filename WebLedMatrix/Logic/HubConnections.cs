using System;
using System.Collections.Generic;
using System.Linq;
using WebLedMatrix.Models;

namespace WebLedMatrix.Logic
{
    public class HubConnections
    {
        //Todo: To di
        public static HubConnections Repository = new HubConnections();
        public List<HubUser> HubUserList = new List<HubUser>();
        private static IList<Session> Sessions { get; } = new List<Session>();
        private readonly object @lock = new object();
 
        public HubConnections()//IList<Session> sessions)
        {
            //this.Sessions = sessions;
        }

        
        public void DeleteConnection(string connectionId)
        {
            lock (@lock)
            {
                var user = HubUserList.Single(x => x.Ids.Any(y=>y.Equals(connectionId)));
                user.Ids.Remove(connectionId);
                var toEnd = Sessions.Where(x => x.ID == connectionId);
                foreach (var sess in toEnd)
                {
                    sess.EndSession();
                }
                if (! user.Ids.Any())
                {
                    HubUserList.Remove(user);
                }
            }
        }

        public bool IsMuted(string userName)
        {
            lock (@lock)
            {
                return HubUserList.Single(x => x.UserName.Equals(userName)).IsMuted;
            }
        }

        public void SetMuteState(string userName,bool isMuted)
        {
            lock (@lock)
            {
                HubUserList.Single(x => x.UserName.Equals(userName)).IsMuted = isMuted;
            }
        }

        public void AddConnection(string connectionId, string userName)
        {
            lock (@lock)
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
                Sessions.Add(new Session(DateTime.Now, userName, connectionId));
            }
        }
    }
}