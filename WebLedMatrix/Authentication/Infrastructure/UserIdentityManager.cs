using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using WebLedMatrix.Authentication.Models;

namespace WebLedMatrix.Authentication.Infrastructure
{
    public class UserIdentityManager : UserManager<User>
    {
        public UserIdentityManager(IUserStore<User> store ) : base(store) 
        {

        }

        public static UserIdentityManager Create(IdentityFactoryOptions<UserIdentityManager> options, IOwinContext context)
        {
            UserIdentityDbContext dbContext = context.Get<UserIdentityDbContext>();
            UserIdentityManager identityManager = new UserIdentityManager(new UserStore<User>(dbContext));

            identityManager.PasswordValidator = new PasswordValidator()
            {
                RequireDigit   = false,
                RequireLowercase = true,
                RequireUppercase = true,
                RequiredLength = 6,
                RequireNonLetterOrDigit = false
            };

            identityManager.UserValidator = new UserValidator<User>(identityManager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };

            return identityManager;
        }
    }
}