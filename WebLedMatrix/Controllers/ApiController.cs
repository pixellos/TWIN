using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Autofac.Integration.Wcf;
using Autofac;

namespace WebLedMatrix.Controllers
{
    public class ApiController : System.Web.Http.ApiController
    {
        private MatrixManager MatrixManager;
        public ApiController()
        {
            MatrixManager = AutofacHostFactory.Container.Resolve<MatrixManager>();
        }

        [Route("clientApi/RefreshState/{name}")]
        [HttpPost]
        public string RefreshState(string name)
        {
            var matrix = MatrixManager.Matrices.SingleOrDefault(x => x.Name == name);
            if (matrix == null)
            {
                return "Registered";
                //Todo: Add timeout and forcing reregistering
            }
            else
            {
                return "Refreshed";
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
        public List<string> Users(string name)
        {
            return null; //;
        }
    }
}