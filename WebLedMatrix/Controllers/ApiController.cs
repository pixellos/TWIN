using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Autofac.Integration.Wcf;
using Autofac;
using WebLedMatrix.Logic;

namespace WebLedMatrix.Controllers
{
    public class ApiController : System.Web.Http.ApiController
    {
        public List<string> Users => HubConnections.Repository.HubUserList.Select(x => x.UserName).ToList();

        private MatrixManager MatrixManager;
        public ApiController()
        {
            MatrixManager = AutofacHostFactory.Container.Resolve<MatrixManager>();
        }

        [Route("clientApi/Register/{name}")]
        [HttpGet]
        public string Register(string name)
        {
            var matrix = MatrixManager.Matrices.SingleOrDefault(x => x.Name == name);
            if (matrix == null)
            {
                this.MatrixManager.AddMatrix(name);
                return "Registered";
                //Todo: Add timeout and forcing reregistering
            }
            else
            {
                return "Refreshed";
            }
        }

        [Route("clientApi/UnRegister/{name}")]
        [HttpGet]
        public HttpResponseMessage CloseConnection(string name)
        {
            var matrix = MatrixManager.Matrices.SingleOrDefault(x => x.Name == name);
            if (matrix == null)
            {
                var respond = new HttpResponseMessage(HttpStatusCode.Forbidden) {ReasonPhrase = "This connection has not be established"};
                return respond;
            }
            else
            {
                MatrixManager.RemoveMatrix(name);
                return new HttpResponseMessage(HttpStatusCode.OK) { ReasonPhrase = $"Matrix {name} has been successfully removed" };
            }
        }

        [Route("clientApi/Commands/{name}")]
        [HttpGet]
        public string Commands(string name)
        {
            var matrix = MatrixManager.Matrices.SingleOrDefault(x => x.Name == name);
            if (matrix == null)
            {
                return "ERROR: Sorry, your matrix is not registered. Please register it before getting data.";
            }
            else
            {
                return matrix.PendingData;
            }
        }

        [Route("clientApi/Users/{name}")]
        [HttpGet]
        public List<string> GetUsers(string name)
        {
            return Users;//;
            //Todo: unregister user when log out
        }
    }
}