using System.Diagnostics.CodeAnalysis;
using Autofac;
using WebLedMatrix.IoC;

namespace Test.WebLedMatrix.Server
{
    [ExcludeFromCodeCoverage]
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
