using System.Data.Entity;

namespace WebLedMatrix.Authentication.Infrastructure
{
    public class UserIdentityDbInitialize : DropCreateDatabaseIfModelChanges<UserIdentityDbContext>
    {
        protected override void Seed(UserIdentityDbContext context)
        {
            PerformInitialSetup(context);
            base.Seed(context);
        }

        public void PerformInitialSetup(UserIdentityDbContext context)
        {
            
        }
    }
}