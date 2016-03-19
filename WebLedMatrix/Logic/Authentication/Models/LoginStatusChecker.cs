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
                if (user.IsInRole("Admininstrator"))
                {
                    return State.Admin.ToString();
                }
                return (State.Logged.ToString());
            }
            else
            {
                return (State.NotLogged.ToString());
            }
        }
    }
}