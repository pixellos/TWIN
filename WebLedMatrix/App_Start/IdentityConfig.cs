using System.Reflection;
using System.Web;
using System.Web.Helpers;
using Autofac;
using Autofac.Integration.SignalR;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using WebLedMatrix.Hubs;
using WebLedMatrix.IoC;
using WebLedMatrix.Logic.Authentication.Concrete;
using WebLedMatrix.Logic.Authentication.Infrastructure;
using WebLedMatrix.Logic.Authentication.Models.Roles;
using static Autofac.Integration.Wcf.AutofacHostFactory;

namespace WebLedMatrix
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            var config = SignalRIoCConfiguration();
            RegisterIdentity(app,config);
        }

        private static HubConfiguration SignalRIoCConfiguration()
        {
            ContainerBuilder builder = new ContainerBuilder();
            var config = new HubConfiguration();
            builder.RegisterHubs(Assembly.GetExecutingAssembly());
            builder.RegisterType(typeof (UiManagerHub)).AsSelf().InstancePerDependency();
            builder.RegisterModule(new BrowserXServerModule());
            
            var container = builder.Build();
            config.Resolver = new AutofacDependencyResolver(container);
            Container = container;
            
            GlobalHost.DependencyResolver = config.Resolver;
            return config;
        }

        private static void WCFIoC()
        {
            
        }

        private static void RegisterIdentity(IAppBuilder app, HubConfiguration config)
        {
            WCFIoC();
            app.CreatePerOwinContext<UserIdentityDbContext>(UserIdentityDbContext.Create);
            app.CreatePerOwinContext<UserIdentityManager>(UserIdentityManager.Create);
            app.CreatePerOwinContext<AppRoleManager>(AppRoleManager.Create);
                

            app.UseCookieAuthentication(
                new CookieAuthenticationOptions()
                {
                    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                    LoginPath = new PathString("/Account/Login"),
                });
            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;
        
            app.MapSignalR(config);
        }
    }
}