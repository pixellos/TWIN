using System;
using System.Collections.Generic;
using System.Text;

namespace WebLedMatrix.Matrix.Model
{
    public interface IDataService
    {
        void GetData(Action<DataItem, Exception> callback);
    }
}
