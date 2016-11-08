using System;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using WebLedMatrix.Matrix.MatrixService;

namespace WebLedMatrix.Matrix.Logic
{
    public class ServiceWrapper
    {
        private MatrixServiceClient _client;

        private String _name
        {
            get
            {
                return Properties.Settings.Default._name;
            }
            set { Properties.Settings.Default._name = value; }
        }

        WebClient HttpClient { get; set; }
        string _CurentCommands;
        string CurrentCommands
        {
            get
            {
               return _CurentCommands;
            }

            set
            {
                _CurentCommands = value;
            }

        }

        void Method()
        {
            this.CurrentCommands = "tajoktjtea";
        }

        public ServiceWrapper()
        {
            //Todo: Check at another thread is new data available once at second
            string URI = "http://webledmatrix.azurewebsites.net/clientApi/RefreshState/MyClient";
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                wc.Headers[System.Net.HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string HtmlResult = wc.UploadString(URI, "");
                Console.WriteLine(HtmlResult);
                Task.Run( () =>
                {
                    Task.Delay(100);
                    var x = this._name;
                });
            }
            /* _client = IoCContainter.Resolve<MatrixServiceClient>();*/
        }

        public void SetName(string name)
        {
            try
            {
                _client.UnRegisterMatrix(_name);
                _name = name;
                _client.RegisterMatrix(_name);
            }
            catch (Exception)
            {
                //Connection has been lost
            }
        }
    }
}