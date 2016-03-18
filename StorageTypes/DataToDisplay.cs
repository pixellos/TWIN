using WebLedMatrix.Types;

namespace StorageTypes
{
    public class DataToDisplay
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public DisplayDataType DisplayDataType { get; set; }
        public string Data { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is DataToDisplay)
            {
                return ((DataToDisplay) obj).Data.Equals(this.Data) &&
                       ((DataToDisplay) obj).DisplayDataType.Equals(this.DisplayDataType);
            }
            return base.Equals(obj);
        }
    }
}