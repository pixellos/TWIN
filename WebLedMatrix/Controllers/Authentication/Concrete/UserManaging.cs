using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using WebLedMatrix.Logic.Authentication.Infrastructure;
using WebLedMatrix.Logic.Authentication.Models;

namespace WebLedMatrix.Logic.Authentication.Concrete
{
    public class UserManaging
    {
        private UserIdentityManager UserManager;
        private IAuthenticationManager _authenticationManager;
        public UserManaging(UserIdentityManager userManager, IAuthenticationManager authenticationManager)
        {
            UserManager = userManager;
            _authenticationManager = authenticationManager;
        }

        public async Task<IdentityResult> TryDeleteAsync(string id)
        {
            User user = await UserManager.FindByIdAsync(id);
            return await UserManager.DeleteAsync(user);
        }

        public async Task TryCreateUser(User user, string password)
        {
            var result = await UserManager.CreateAsync(user, password);
            result.CheckResult();
        }

        /// <returns>Final User object</returns>
        public async Task TryDeleteUser(string id)
        {
            var result = await UserManager.DeleteAsync(await UserManager.FindByIdAsync(id));
            result.CheckResult();
        }


        /// <returns>Final User object</returns>
        public async Task TryUpdateUser(string id, string email, string password)
        {
            User user = await UserManager.FindByIdAsync(id);
            if (user == null)
                throw new ResultException() {ResultErrors = new []{"User Not Found"}};

            user.Email = email;
            user.PasswordHash = UserManager.PasswordHasher.HashPassword(password);
            await TryUpdateUser(user);
        }

        public async Task TryUpdateUser(User user)
        {
            await UserManager.TryValidUser(user);

            var result = await UserManager.UpdateAsync(user);
            result.CheckResult();
        }

        public async Task TryCreateUser(CreationModel userModel)
        {
            await TryCreateUser(userModel.GetUser(), userModel.Password);
        }

        public bool TryLogin(LoginModel loginModel)
        {
            var user = UserManager.Find(loginModel.Name, loginModel.Password);
            if (user != null)
            {
                _authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie));
            }
            return user != null;
        }
    }
}
