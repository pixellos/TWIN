using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebLedMatrix.Controllers
{
    public class Api : ApiController
    {
        [Route("api/RefreshState/{name}")]
        [HttpPost]
        public string RefreshState(string name)
        {
            if (true)
            {
                return "Refreshed";
            }
            else
            {
                return "Error";
            }
        }

        [Route("api/Commands/{name}")]
        [HttpGet]
        public string Commands(string name)
        {
            if (true)//)there are pending commands for name client
            {
                return "Up, Down, :url:=\"SomeTypedUll\"";
            }
            else
            {
                return "0";
            }
        }

        [Route("api/Users/{name}")]
        [HttpGet]
        public List<string> Users(string name)
        {
            return null; //;
        }
    }
}