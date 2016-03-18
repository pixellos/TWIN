using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Autofac;
using Autofac.Core.Registration;
using Autofac.Integration.Wcf;
using WebLedMatrix.Hubs;
using WebLedMatrix.Types.MatrixServiceCallback;

namespace WebLedMatrix
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MatrixService" in code, svc and config file together.
    // NOTE: In order to launch WCF MatrixService Client for testing this service, please select MatrixService.svc or MatrixService.svc.cs at the Solution Explorer and start debugging.

    public class MatrixService : IMatrixService
    {
        private MatrixManager _matrixManager;
        public MatrixService()
        {
            _matrixManager = AutofacHostFactory.Container.Resolve<MatrixManager>();
            OperationContext.Current.Channel.Faulted += Channel_Faulted;
        }

        private Matrix _thisMatrix = null;

        public void RegisterMatrix(string name)
        {
            _thisMatrix = _matrixManager.AddMatrix(name,OperationContext.Current.GetCallbackChannel<IMatrixServiceCallback>() );
        }

        public void UnRegisterMatrix(string name)
        {
            _matrixManager.RemoveMatrix(name);
        }

        private void Channel_Faulted(object sender, EventArgs e)
        {
            _matrixManager.RemoveMatrix(_thisMatrix.Name);
        }
    }
}
