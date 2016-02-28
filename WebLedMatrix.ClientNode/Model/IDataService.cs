using System;

namespace WebLedMatrix.ClientNode.Model
{
    public interface IDataService
    {
        void GetData(Action<DataItem, Exception> callback);
    }
}
