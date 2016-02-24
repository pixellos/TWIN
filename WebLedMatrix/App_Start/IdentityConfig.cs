using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using WebLedMatrix.Authentication.Infrastructure;
using WebLedMatrix.Authentication.Models.Roles;

namespace WebLedMatrix
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
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

            app.MapSignalR();
            

        }
    }
}