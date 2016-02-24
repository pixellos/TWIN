using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebLedMatrix.Authentication.Models;
using WebLedMatrix.Authentication.Models.Roles;

namespace WebLedMatrix.Authentication.Infrastructure
{
    public class UserIdentityDbInitialize : CreateDatabaseIfNotExists<UserIdentityDbContext>
    {
        protected override void Seed(UserIdentityDbContext context)
        {
            PerformInitialSetup(context);
            base.Seed(context);
        }

        public void PerformInitialSetup(UserIdentityDbContext context)
        {
            UserIdentityManager userManager = new UserIdentityManager(new UserStore<User>(context));
            AppRoleManager roleManager= new AppRoleManager(new RoleStore<AppRole>(context));




        }
    }
}