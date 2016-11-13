
using System.Collections.Generic;

namespace WebLedMatrix.Models
{
    public class Matrix
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
                    _PendingData = null;
                    return data;
                }
            }
        }

        public void AppendData(string name, string data)
        {
            _PendingData.Add($"<{name}>{data}</{name}>");
        }

        public override bool Equals(object obj)
        {
            if (obj is Matrix)
            {
                var matrix = obj as Matrix;
                return matrix.Equals(this);
            }
            else
            {
                return obj.GetHashCode() == this.GetHashCode();
            }
        }

        public bool Equals(Matrix other)
        {
            return string.Equals(Name, other.Name);
        }

        public override int GetHashCode()
        {
            return Name?.GetHashCode() ?? 0;
        }
    }
}