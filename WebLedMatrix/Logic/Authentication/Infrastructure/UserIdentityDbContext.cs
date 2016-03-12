using Microsoft.AspNet.Identity.EntityFramework;
using WebLedMatrix.Logic.Authentication.Models;

namespace WebLedMatrix.Logic.Authentication.Infrastructure
{
    public class UserIdentityDbContext : IdentityDbContext<User>
    {
        public UserIdentityDbContext() : base("Server=tcp:pixeldatabase.database.windows.net,1433;Database=PIXELSQLDATABASE;User ID=data123uj1io23jh312oi@pixeldatabase;Password=Data1234;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;") {}

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
