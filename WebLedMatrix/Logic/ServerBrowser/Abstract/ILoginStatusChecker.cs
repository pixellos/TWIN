using System.Security.Principal;

namespace WebLedMatrix.Logic.ServerBrowser.Abstract
{
    public enum State
    {
        Admin = 2,
        Logged = 1,
        NotLogged = 0
    }


    public interface ILoginStatusChecker
    {
        string GetLoginStateString(IPrincipal user);
    }

}