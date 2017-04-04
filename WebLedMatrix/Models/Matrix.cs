
using System.Collections.Generic;

namespace WebLedMatrix.Models
{
    public class Client
    {
        public string Name { get; set; }
        public bool Connected { get; set; }
        public bool Enabled { get; set; }
        public string Ip { get; set; }
        private List<string> _PendingData;
        /// <summary>
        /// If this is geted pendingData is cleared
        /// </summary>
        public List<string> PendingData
        {
            get
            {
                lock (this)
                {
                    var data = _PendingData;
                    _PendingData = new List<string>();
                    return data;
                }
            }
        }

        public void AppendData(string name, string data)
        {
            _PendingData.Add($"{name}:{data}");
        }

        public override bool Equals(object obj)
        {
            if (obj is Client)
            {
                var matrix = obj as Client;
                return matrix.Equals(this);
            }
            else
            {
                return obj.GetHashCode() == this.GetHashCode();
            }
        }

        public bool Equals(Client other)
        {
            return string.Equals(Name, other.Name);
        }

        public override int GetHashCode()
        {
            return Name?.GetHashCode() ?? 0;
        }
    }
}