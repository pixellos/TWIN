using Microsoft.AspNet.Identity.EntityFramework;
using WebLedMatrix.Logic.Authentication.Models;

namespace WebLedMatrix.Logic.Authentication.Infrastructure
{
    public class UserIdentityDbContext : IdentityDbContext<User>
    {
        public UserIdentityDbContext() : base("Slave/IdentityDb"){}

        static UserIdentityDbContext()
        {
            System.Data.Entity.Database.SetInitializer<UserIdentityDbContext>(new UserIdentityDbInitialize());
        }
        
        public static UserIdentityDbContext Create()
        {
            return new UserIdentityDbContext();
        }
    }
}
