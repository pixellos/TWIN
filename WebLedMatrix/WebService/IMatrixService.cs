using System.ServiceModel;

namespace WebLedMatrix.WebService
{
    [ServiceContract(SessionMode = SessionMode.Required,CallbackContract = typeof (IMatrixServiceCallback))]
    public interface IMatrixService
    {
        [OperationContract(IsOneWay = true)]
        void RegisterMatrix(string name);
        [OperationContract(IsOneWay = true)]
        void UnRegisterMatrix(string name);
    }
}
