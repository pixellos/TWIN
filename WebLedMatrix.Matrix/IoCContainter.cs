using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using WebLedMatrix.Matrix.Logic;
using WebLedMatrix.Matrix.ViewModel;

namespace WebLedMatrix.Matrix
{
    static class IoCContainter
    {
        public static IContainer BaseContainer { get; private set; }

        public static TService Resolve<TService>()
        {
            return BaseContainer.Resolve<TService>();
        }

        public static void Build()
        {
            if (BaseContainer == null)
            {
                var builder = new ContainerBuilder();
                Load(builder);
                BaseContainer = builder.Build();
            }
        }

        private static void Load(ContainerBuilder builder)
        {
            
            builder.RegisterType<ServiceWrapper>();
            //builder.RegisterType<MatrixService.MatrixServiceClient>().UsingConstructor(() => new MatrixServiceClient(new InstanceContext(new MatrixCallback())));
        }
    }
}
