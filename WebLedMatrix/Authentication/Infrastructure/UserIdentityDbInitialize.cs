using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
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

        private string userName = "Admin";
        private string password = "ChangeThisPassword1";
        private string email = "admin@admin.admin";
        private string firstName = "TWIN";
        private string lastName= "TWIN";


        public void PerformInitialSetup(UserIdentityDbContext context)
        {
            UserIdentityManager userManager = new UserIdentityManager(new UserStore<User>(context));
            AppRoleManager roleManager= new AppRoleManager(new RoleStore<AppRole>(context));

            userManager.Create(
                new User() {UserName = userName, FirstName = firstName, LastName = lastName, Email = email}, password);

            var user = userManager.FindByName(userName);

            foreach (string roles in Enum.GetNames(typeof(TypicalRoles)))
            {
                roleManager.Create(new AppRole(roles));
                userManager.AddToRole(user.Id, roles);
            }
        }
    }
}