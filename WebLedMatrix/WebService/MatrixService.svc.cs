using System;
using System.ServiceModel;
using Autofac;
using Autofac.Integration.Wcf;
using WebLedMatrix.Hubs;
using WebLedMatrix.Models;

namespace WebLedMatrix.WebService
{
    public class MatrixService : IMatrixService
    {
        private MatrixManager _matrixManager;
        private Matrix _thisMatrix = null;

        public MatrixService()
        {
            _matrixManager = AutofacHostFactory.Container.Resolve<MatrixManager>();
            OperationContext.Current.Channel.Faulted += Channel_Faulted;
        }

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
