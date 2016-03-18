using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using WebLedMatrix.Logic.Authentication.Infrastructure;
using WebLedMatrix.Logic.Authentication.Models.Roles;

namespace WebLedMatrix.Controllers
{
    public class RoleAdministrating
    {
        private AppRoleManager _roleManager;
        private UserIdentityManager _identityManager;

        public RoleAdministrating(AppRoleManager roleManager, UserIdentityManager identityManager)
        {
            _roleManager = roleManager;
            _identityManager = identityManager;
        }

        public  IQueryable<AppRole> GetRoles()
        {
            return _roleManager.Roles;
        } 

        public async Task<string> GetRoleName(string id)
        {
            AppRole role = await _roleManager.FindByIdAsync(id);
            return role.Name;
        } 

        public async Task DeleteMembers(string roleName, IEnumerable<string> usersId)
        {
            if (!usersId.Any())
            {
                return;
            }
            AppRole role = await FindRole(roleName);
            var roleUsersId = role.Users.Select(x => x.UserId).ToList();
            if (! usersId.Any(roleUsersId.Contains)) 
            {
                throw new InputDataException("You're trying to delete unexisting, noone were deleted.");
            }

            foreach (var user in usersId)
            {
                IdentityResult result = await _identityManager.RemoveFromRoleAsync(user, role.Name);
                if (!result.Succeeded)
                {
                    throw new RoleResultException("Result wasnt succeeded")
                    { ErrorStrings = result.Errors };
                }
            }
        }

        private async Task<AppRole> FindRole(string roleName)
        {

            AppRole role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                throw new RoleNotFoundException("There isn't role named " + roleName);
            }

            return role;
        }

        public async Task AddMembers(string roleName, IEnumerable<string> usersId)
        {
            if (!usersId.Any())
            {
                return;
            }
            AppRole role = await FindRole(roleName);

            if (role.Users.Select(x=>x.UserId).Intersect(usersId).Any())
            {
                throw new InputDataException("Users are recurring, no one were added.");
            }

            foreach (var user in usersId)
            {
                IdentityResult result =  await _identityManager.AddToRoleAsync(user, role.Name);
                if (!result.Succeeded)
                {
                    throw new RoleResultException()
                    {ErrorStrings = result.Errors};
                }
            }
        } 

        public async Task AddRole(string roleName)
        {
            IdentityResult result = await _roleManager.CreateAsync(new AppRole(roleName));
            if (!result.Succeeded)
            {
                throw new RoleResultException(){    ErrorStrings = result.Errors    };
            }
        }
    }
}