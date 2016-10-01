using Microsoft.AspNet.Identity.EntityFramework;
using WebLedMatrix.Logic.Authentication.Models;

namespace WebLedMatrix.Logic.Authentication.Infrastructure
{
    public class UserIdentityDbContext : IdentityDbContext<User>
    {
        public UserIdentityDbContext(): base("Server=tcp:data11admin.database.windows.net,1433;Initial Catalog=TestDatabase;Persist Security Info=False;User ID=admin1@data11admin;Password=Data123456!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;")
        {
            
        }
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
