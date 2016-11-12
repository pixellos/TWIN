using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace WebLedMatrix.Matrix.Logic
{
    public class ServiceWrapper
    {
        string ServerName => "http://webledmatrix.azurewebsites.net/";

        string ApiDefaultAddress => $"{this.ServerName}clientApi/";
        string RegisterNameAddress => $"RefreshState/{Name}";
        string CommandsAddress => $"Commands/{Name}";

        RestClient RestClient { get; set; }
        public ServiceWrapper()
        {
            this.RestClient = new RestClient(ApiDefaultAddress);
        }

        public String Name
        {
            get
            {
                return Properties.Settings.Default._name;
            }
            private set { Properties.Settings.Default._name = value; }
        }

        //Todo: Bind it later to some model
        List<object> CurrentCommands
        {
            get
            {
                var request = new RestRequest(this.RegisterNameAddress, Method.POST);
                request.RequestFormat = DataFormat.Xml;
                var result = this.RestClient.Execute<List<object>>(request);
                return result.Data;
            }
        }

        public void SetName(string name)
        {
            this.Name = name;
            Task.Run(() =>
            {
                while (true)
                {
                    Task.Delay(1000);
                    var request = new RestRequest(this.CommandsAddress, Method.POST);
                    request.RequestFormat = DataFormat.Xml;
                    var result = this.RestClient.Execute(request);
                }
            }).Start();
        }

        public void Dispose()
        {
            _client.Close();
        }
    }
}