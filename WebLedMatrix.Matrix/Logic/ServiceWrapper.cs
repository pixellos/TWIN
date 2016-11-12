using System;
using System.ServiceModel;
using WebLedMatrix.Matrix.MatrixService;

namespace WebLedMatrix.Matrix.Logic
{
    public class ServiceWrapper: IDisposable 
    {
        private MatrixServiceClient _client;

        private string _name
        {
            get
            {
                return Properties.Settings.Default._name;
            }
            set { Properties.Settings.Default._name = value; }
        }

        public ServiceWrapper()
        {
            _client = new MatrixServiceClient(
                new InstanceContext(new MatrixCallback()));
            _client.Open();
            _client.RegisterMatrix(_name);
            /* _client = IoCContainter.Resolve<MatrixServiceClient>();*/
        }

        public void SetName(string name)
        {
            try
            {
                _client.UnRegisterMatrix(_name);
            }
            catch (Exception )
            {
                //Connection has been lost
            }
            
            _name = name;
            _client.RegisterMatrix(_name);
        }

        public void Dispose()
        {
            _client.Close();
        }
    }
}