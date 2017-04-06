using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Autofac;
using System.Web.Http.Cors;
using WebLedMatrix.Models;

namespace WebLedMatrix.Controllers
{
    [EnableCors("*", "*", "GET")]
    public class ApiController : System.Web.Http.ApiController
    {
        private Clients ConnectedClients;
        private IList<Session> Sessions { get; }
        public ApiController(IList<Session> session, Clients clients)
        {
            this.Sessions = session;
            this.ConnectedClients = clients;
        }

        [Route("clientApi/Reference")]
        [HttpGet]
        public string ApiReference()
        {
            var apiMethods = typeof(ApiController).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                .Where(x => x.CustomAttributes.Any(a => a.AttributeType == typeof(RouteAttribute)));
            var strings = apiMethods.Select(x => x.Name + "(" + String.Join(", ", x.GetParameters().Select(p => p.ParameterType)) + ")");
            return String.Join(Environment.NewLine, strings);
        }

        [Route("clientApi/Register/{name}")]
        [HttpGet]
        public string Register(string name)
        {
            var matrix = ConnectedClients.SingleOrDefault(x => x.Name == name);
            if (matrix == null)
            {
                this.ConnectedClients.Register(name);
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
            var matrix = ConnectedClients.SingleOrDefault(x => x.Name == name);
            if (matrix == null)
            {
                var respond = new HttpResponseMessage(HttpStatusCode.Forbidden) { ReasonPhrase = "This connection has not be established" };
                return respond;
            }
            else
            {
                ConnectedClients.RemoveMatrix(name);
                return new HttpResponseMessage(HttpStatusCode.OK) { ReasonPhrase = $"Matrix {name} has been successfully removed" };
            }
        }

        [Route("clientApi/Commands/{name}")]
        [HttpGet]
        public string[] Commands(string name)
        {
            var matrix = ConnectedClients.SingleOrDefault(x => x.Name == name);
            if (matrix == null)
            {
                return new string[] { "ERROR: Sorry, your matrix is not registered. Please register it before getting data." };
            }
            else
            {
                var data = matrix.PendingData;
                return matrix.PendingData?.ToArray() ?? Array.Empty<String>();
            }
        }

        [Route("clientApi/Users/{name}")]
        [HttpGet]
        public List<string> GetUsers(string name)
        {
            return this.Sessions.Where(x => !x.IsEnded).Select(x => x.UserName).Distinct().ToList();
            //Todo: unregister user when log out
        }
    }
}