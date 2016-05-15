using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using WebLedMatrix.Controllers.Authentication.Models;
using WebLedMatrix.Hubs;
using WebLedMatrix.Logic;
using WebLedMatrix.Logic.Authentication.Abstract;
using WebLedMatrix.Logic.Authentication.Models;

namespace WebLedMatrix.IoC
{
    public class BrowserXServerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new LoginStatusChecker()).As<ILoginStatusChecker>().SingleInstance();
            builder.Register(c => new MatrixManager()).SingleInstance();
            builder.Register(c => HubConnections.Repository);
        }
    }
}
