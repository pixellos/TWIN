using System.ServiceModel;

namespace WebLedMatrix.WebService
{
    public interface IMatrixServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void UpdateWebPage(string text);
        [OperationContract(IsOneWay = true)]
        void UpdateText(string text);
    }
}