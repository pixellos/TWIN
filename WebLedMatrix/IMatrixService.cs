using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using StorageTypes.MatrixServiceCallback;

namespace WebLedMatrix
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMatrixHandleService" in both code and config file together.
    [ServiceContract(SessionMode = SessionMode.Required,CallbackContract = typeof (IMatrixServiceCallback))]
    public interface IMatrixService
    {
        [OperationContract(IsOneWay = true)]
        void RegisterMatrix(string name);
        [OperationContract(IsOneWay = true)]
        void UnRegisterMatrix(string name);
    }
}
