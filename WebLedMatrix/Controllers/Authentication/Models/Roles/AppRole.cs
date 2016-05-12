using Microsoft.AspNet.Identity.EntityFramework;

namespace WebLedMatrix.Logic.Authentication.Models.Roles
{
    public class AppRole : IdentityRole
    {
        public AppRole() : base(){}

        public AppRole(string name) :base(name){}
    }
}
