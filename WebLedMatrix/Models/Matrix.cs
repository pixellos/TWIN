
namespace WebLedMatrix.Models
{
    public class Matrix
    {
        public string Name { get; set; }
        public bool Connected { get; set; }
        public bool Enabled { get; set; }
        public string Ip { get; set; }
        private string _PendingData;
        /// <summary>
        /// If this is geted pendingData is cleared
        /// </summary>
        public string PendingData
        {
            get
            {
                var data = _PendingData;
                _PendingData = null;
                return data;
            }
        }

        public void AppendData(string data)
        {
            _PendingData += data;
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