using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using WebLedMatrix.IoC;

namespace WebLedMatrixTests1
{
    public class BaseTest
    {
        protected IContainer _Container= null;

        public BaseTest()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new BrowserXServerModule());
            _Container = builder.Build();
        }
    }
}
