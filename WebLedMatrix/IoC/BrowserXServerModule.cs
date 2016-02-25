using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using WebLedMatrix.Logic.ServerBrowser.Abstract;
using WebLedMatrix.Logic.ServerBrowser.Concrete;

namespace WebLedMatrix.IoC
{
    public class BrowserXServerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new LoginStatusChecker()).As<ILoginStatusChecker>().SingleInstance();
        }
    }
}
