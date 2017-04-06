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
using WebLedMatrix.Logic;
using Autofac.Integration.WebApi;
using System.Web.Http;

namespace WebLedMatrix
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            var hubConfig = new HubConfiguration();
            builder.RegisterHubs(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType(typeof(UiManagerHub)).AsSelf().InstancePerDependency();
            builder.RegisterType(typeof(HubConnections)).AsSelf().SingleInstance();
            builder.RegisterModule(new BrowserXServerModule());
            var container = builder.Build();
            hubConfig.Resolver = new AutofacDependencyResolver(container);
            GlobalHost.DependencyResolver = hubConfig.Resolver;
            IdentityConfig.RegisterIdentity(app, hubConfig);
            app.UseAutofacMiddleware(container);
            var webApiResolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = webApiResolver;
            app.MapSignalR(hubConfig);
        }

        private static void RegisterIdentity(IAppBuilder app, HubConfiguration config)
        {
            app.CreatePerOwinContext(UserIdentityDbContext.Create);
            app.CreatePerOwinContext<UserIdentityManager>(UserIdentityManager.Create);
            app.CreatePerOwinContext<AppRoleManager>(AppRoleManager.Create);
            app.UseCookieAuthentication(
                new CookieAuthenticationOptions()
                {
                    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                    LoginPath = new PathString("/Account/Login"),
                });
            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;
        }
    }
}