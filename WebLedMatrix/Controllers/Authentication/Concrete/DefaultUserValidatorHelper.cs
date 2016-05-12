using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using WebLedMatrix.Logic.Authentication.Infrastructure;
using WebLedMatrix.Logic.Authentication.Models;

namespace WebLedMatrix.Logic.Authentication.Concrete
{
    public static class DefaultUserValidatorHelper
    {
        public static void CheckResult(this IdentityResult result)
        {
            if (! result.Succeeded) throw new ResultException() { ResultErrors = result.Errors };
        }
        public static  async Task TryValidUser(this UserIdentityManager userManager, User user)
        {
            (await userManager.UserValidator.ValidateAsync(user)).CheckResult();
        }
    }
}