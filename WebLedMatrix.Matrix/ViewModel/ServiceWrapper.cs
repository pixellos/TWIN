using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Windows;
using WebLedMatrix.Matrix.MatrixService;

namespace WebLedMatrix.Matrix.ViewModel
{
    public class MatrixCallback :IMatrixServiceCallback
    {
        public void UpdateText(string text)
        {
            MessageBox.Show(text);
        }

        public IAsyncResult BeginUpdateText(string text, AsyncCallback callback, object asyncState)
        {
            return null;
        }

        public void EndUpdateText(IAsyncResult result)
        {

        }
    }

    public class ServiceWrapper
    {
        private MatrixServiceClient _client;
        const string kurwaAdres = "http://127.0.0.1/Matrix/MatrixService.svc";


        public ServiceWrapper()
        {
            _client = new MatrixService.MatrixServiceClient(new InstanceContext(new MatrixCallback()));
            _client.RegisterMatrix(_name);
            _client.DisplayInitializationUI();
            /*
            var listener = new ConsoleTraceListener();
            _client = IoCContainter.Resolve<MatrixServiceClient>();
           
     */
        }

        private String _name
        {
            get
            {
                return Properties.Settings.Default._name;
            }
            set {  Properties.Settings.Default._name = value; }
        } 

        public void SetName(string name)
        {
            _client.UnRegisterMatrix(_name);
            _name = name;
            _client.RegisterMatrix(_name);
        }


        ~ServiceWrapper()
        {
            _client.Close();
        }
    }
}