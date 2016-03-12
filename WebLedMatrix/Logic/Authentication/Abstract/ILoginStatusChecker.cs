using System.Security.Principal;

namespace WebLedMatrix.Logic.Authentication.Abstract
{
    public interface ILoginStatusChecker
    {
        string GetLoginStateString(IPrincipal user);
    }

}