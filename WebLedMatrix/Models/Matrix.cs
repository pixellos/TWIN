using WebLedMatrix.WebService;

namespace WebLedMatrix.Models
{
    public class Matrix
    {
        public string Name { get; set; }
        public bool Connected { get; set; }
        public bool Enabled { get; set; }
        public IMatrixServiceCallback Callback { get; set; }

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