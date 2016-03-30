using System.Collections.Generic;

namespace WebLedMatrix.Hubs
{
    public class HubUserModel : IEqualityComparer<HubUserModel>
    {
        public HubUserModel(string userName,params string[] argStrings)
        {
            foreach (var argString in argStrings)
            {
                this.Ids.Add(argString);
            }
            UserName = userName;
        }

        public List<string> Ids { get; } = new List<string>();
        public string UserName { get; }
        public bool IsMuted { get; set; } = false;

        public bool Equals(HubUserModel x, HubUserModel y)
        {
            return x.UserName.Equals(y.UserName);
        }

        public int GetHashCode(HubUserModel obj)
        {
            return obj.UserName.GetHashCode();
        }
    }
}