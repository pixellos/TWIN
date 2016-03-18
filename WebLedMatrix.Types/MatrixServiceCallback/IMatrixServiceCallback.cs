using System.ServiceModel;

namespace WebLedMatrix.Types.MatrixServiceCallback
{
    public interface IMatrixServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void UpdateWebPage(string text);
        [OperationContract(IsOneWay = true)]
        void UpdateText(string text);
    }
}