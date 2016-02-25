using System.Security.Principal;
using WebLedMatrix.Hubs;
using WebLedMatrix.Logic.ServerBrowser.Abstract;

namespace WebLedMatrix.Logic.ServerBrowser.Concrete
{
    public class LoginStatusChecker : ILoginStatusChecker
    {
        public string GetLoginStateString(IPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                if (user.IsInRole("Administrators")) //Context.User.IsInRole("Administrators"))
                {
                    return (State.Admin.ToString());
                }
                else
                {
                    return (State.Logged.ToString());
                }
            }
            else
            {
                return (State.NotLogged.ToString());
            }
        }
    }
}