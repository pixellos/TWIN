using System.Collections.Generic;

namespace WebLedMatrix.Models
{
    public class HubUser : IEqualityComparer<HubUser>
    {
        public HubUser(string userName,params string[] argStrings)
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

        public bool Equals(HubUser x, HubUser y)
        {
            return x.UserName.Equals(y.UserName);
        }

        public int GetHashCode(HubUser obj)
        {
            return obj.UserName.GetHashCode();
        }
    }
}