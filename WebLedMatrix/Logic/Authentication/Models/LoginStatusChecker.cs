using System.Security.Principal;
using WebLedMatrix.Logic.Authentication.Abstract;

namespace WebLedMatrix.Logic.Authentication.Models
{
    public class LoginStatusChecker : ILoginStatusChecker
    {
        public string GetLoginStateString(IPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                if (user.IsInRole(State.Admin.ToString())) //Context.User.IsInRole("Administrators"))
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