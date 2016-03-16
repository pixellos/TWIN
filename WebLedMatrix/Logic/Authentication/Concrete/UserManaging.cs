using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using WebLedMatrix.Logic.Authentication.Infrastructure;
using WebLedMatrix.Logic.Authentication.Models;

namespace WebLedMatrix.Logic.Authentication.Concrete
{
    //YAGNI???
    //Instance per call
    public class UserManaging
    {
        private UserIdentityManager _userIdentities;
        private IAuthenticationManager _authenticationManager;

        public UserManaging(UserIdentityManager userIdentities, IAuthenticationManager authenticationManager)
        {
            _userIdentities = userIdentities;
            _authenticationManager = authenticationManager;
        }

        public async Task<IdentityResult> TryDeleteAsync(string id)
        {
            User user = await _userIdentities.FindByIdAsync(id);
            
                return await _userIdentities.DeleteAsync(user);
            
        }        
        public bool TryLogin(LoginModel loginModel)
        {
            var user = _userIdentities.Find(loginModel.Name, loginModel.Password);
            if (user != null)
            {
                _authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, _userIdentities.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie));
            }
            return user != null;
        }
    }
}
