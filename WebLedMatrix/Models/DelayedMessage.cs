using System;

namespace WebLedMatrix.Models
{
    public class DelayedMessage
    {
        public string User { get; }
        public string TargetId { get; }
        public string Data { get; }
        public DelayedMessage(string user, string targetId, string data)
        {
            this.User = user;
            this.TargetId = targetId;
            this.Data = data;
        }
    }
}