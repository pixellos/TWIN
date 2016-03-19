using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLedMatrix.Types;

namespace StorageTypes
{
    public static class DisplayDataTypeExtensionsMethods
    {
        public static DataToDisplay GetDataToDisplay(this DisplayDataType dataType,string data)
        {
            return new DataToDisplay()
            {
                Data = data,
                DisplayDataType = dataType
            };
        }
    }


#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
}
