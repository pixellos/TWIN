using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WebLedMatrix
{
    public interface IMatrixServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void UpdateText(string text);
    }

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMatrixHandleService" in both code and config file together.
    [ServiceContract()]

public interface IMatrixService
    {
        [OperationContract(IsOneWay = true)]
        void RegisterMatrix(string name);
    }
}
