using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebLedMatrix.Logic.Authentication.Infrastructure;
using WebLedMatrix.Logic.Authentication.Models;


public class ResultException : Exception
{
    public IEnumerable<string> ResultErrors { get; set; }
}

namespace WebLedMatrix.Logic.Authentication.Concrete
{
    public static class DefaultUserValidatorHelper
    {
        public static void CheckResult(this IdentityResult result)
        {
            if (! result.Succeeded) throw new ResultException() { ResultErrors = result.Errors };
        }
    }

    /// <summary>
    /// Move to other file after completing
    /// </summary>
    public class DefaultUserValidator
    {
        public DefaultUserValidator(UserIdentityManager userManager, IAuthenticationManager authManager)
        {
            UserManager = userManager;
            AuthManager = authManager;
        }

        private UserIdentityManager UserManager { get; }
        private IAuthenticationManager AuthManager { get; }
    
        public async Task TryValidUser(User user)
        {
            (await UserManager.UserValidator.ValidateAsync(user)).CheckResult();
        }

        public async Task<string> TryGetHashOfPassword(string password)
        {
            (await UserManager.PasswordValidator.ValidateAsync(password))
                .CheckResult();

            return UserManager.PasswordHasher.HashPassword(password);
        }
    }

    public class UserManaging
    {
        private UserIdentityManager UserManager;
        private IAuthenticationManager _authenticationManager;
        private DefaultUserValidator UserValidator;
        public UserManaging(DefaultUserValidator validator,UserIdentityManager userManager, IAuthenticationManager authenticationManager)
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
            user.PasswordHash = await UserValidator.TryGetHashOfPassword(password);
            await TryUpdateUser(user);
        }

        public async Task TryUpdateUser(User user)
        {
            await UserValidator.TryValidUser(user);
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
