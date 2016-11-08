using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
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

        public int HttpGet { get; private set; }

        public string parseCommand(string command)
        {
            if (command.LastIndexOf("/") > 0)
                return command.Substring((command.LastIndexOf('/') + 1), command.Length - 1 - command.LastIndexOf('/'));
            else
                return "";
        }

        public ServiceWrapper()
        {
            //Todo: Check at another thread is new data available once at second

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                    string URI = "http://webledmatrix.azurewebsites.net/clientApi/RefreshState/MyClient";
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    wc.Headers[System.Net.HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    while (true)
                    {
                        Thread.Sleep(1000);
                        string HtmlResult = wc.UploadString(URI, "");
                        Debug.WriteLine("Registered status: ");
                        Debug.WriteLine(HtmlResult);
                    }
                }
            }).Start();
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