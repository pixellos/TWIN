using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLedMatrix.Models
{
    public class Session
    {
        public DateTime SessionStart { get; }
        public DateTime SessionFinish { get; private set; }
        public string UserName { get; }
        private List<string> _Commands { get; }
        public string[] Commands => _Commands.ToArray();
        public bool IsEnded { get; private set;  }

        public Session(DateTime sessionStart, string userName)
        {
            this.SessionStart = sessionStart;
            this.UserName = userName;
            this._Commands = new List<string>();
            this.IsEnded = false;
        }

        public void StoreCommand(string command)
        {
            this._Commands.Add(command);
        }

        public void EndSession()
        {
            if(this.IsEnded)
            {
                throw new InvalidOperationException(nameof(IsEnded));
            }
            this.IsEnded = true;
            this.SessionFinish = DateTime.Now;
        }
    }
}