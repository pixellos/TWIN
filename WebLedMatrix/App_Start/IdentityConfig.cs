using System.Reflection;
using System.Web.Helpers;
using System.Web.Http;
using Autofac;
using Autofac.Integration.SignalR;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using WebLedMatrix.IoC;
using WebLedMatrix.Logic.Authentication.Infrastructure;
using WebLedMatrix.Logic.Authentication.Models.Roles;

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
            var builder = new ContainerBuilder();
            var config = new HubConfiguration();
            builder.RegisterHubs(Assembly.GetExecutingAssembly());
            builder.RegisterModule(new BrowserXServerModule());
            var container = builder.Build();
            config.Resolver = new AutofacDependencyResolver(container);
            return config;
        }

        private static void RegisterIdentity(IAppBuilder app, HubConfiguration config)
        {
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