using System;
using WebLedMatrix.Types.MatrixServiceCallback;

namespace WebLedMatrix.Hubs
{
    public class Matrix
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public string Name { get; set; }
        public bool Connected { get; set; }
        public bool Enabled { get; set; }
        public IMatrixServiceCallback Callback { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Matrix)
            {
                return ((Matrix) obj).Name == this.Name;
            }
            return base.Equals(obj);
        }
    }
}