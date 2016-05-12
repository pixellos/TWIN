using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WebLedMatrix.Controllers;
using WebLedMatrix.Logic.Authentication.Concrete;
using WebLedMatrix.Logic.Authentication.Infrastructure;
using WebLedMatrix.Logic.Authentication.Models.Roles;

namespace WebLedMatrix.Models.Authentication.Roles
{
    public class RoleAdministrating
    {
        private readonly AppRoleManager _injectedAppRoleManager;
        private readonly UserIdentityManager _injectedUserIdentityManager;

        private AppRoleManager RoleManager => _injectedAppRoleManager ?? HttpContext.Current.GetOwinContext().GetUserManager<AppRoleManager>();

        private UserIdentityManager IdentityManager => _injectedUserIdentityManager ?? HttpContext.Current.GetOwinContext().GetUserManager<UserIdentityManager>();

        public RoleAdministrating(AppRoleManager roleManager, UserIdentityManager identityManager)
        {
            _injectedAppRoleManager = roleManager;
            _injectedUserIdentityManager = identityManager;
        }

        public RoleAdministrating() {}
        public async Task<RoleEditModel> CreateRoleEditModel(string id)
        {
            return CreateRoleEditModel(await RoleManager.FindByIdAsync(id));
        }

        public RoleEditModel CreateRoleEditModel(AppRole role)
        {
            var roleModel = new RoleEditModel();
            var userList = IdentityManager.Users.ToList();
            var membersId = role.Users.Select(x=>x.UserId);


            roleModel.Role = role;
            roleModel.Members = userList.Where( x => membersId.Contains(x.Id));
            roleModel.NonMembers = userList.Except(roleModel.Members);

            return roleModel;
        }
        public async Task AddMembers(string roleName, IEnumerable<string> usersId)
        {
            NullExpressionsExtensions.ThrowIfNull(roleName,nameof(roleName));
            NullExpressionsExtensions.ThrowIfNull(usersId, nameof(usersId));

            if (usersId == null || !usersId.Any())
                return;

            AppRole role = await RoleManager.FindByNameAsync(roleName);

            if (role.Users.ToList().Select(x => x.UserId).Intersect(usersId).Any())
                throw new UsersAreRecurringException("Users are recurring, no one were added.");

            foreach (var user in usersId)
            {
                IdentityResult result = await IdentityManager.AddToRoleAsync(user, role.Name);
                result.CheckResult();
            }
        }


        public async Task DeleteMembers(string roleName, IEnumerable<string> usersId)
        {

            NullExpressionsExtensions.ThrowIfNull(roleName, () => new RoleNotFoundException("Role is null"));
            NullExpressionsExtensions.ThrowIfNull(usersId, nameof(usersId));

            AppRole role = await RoleManager.FindByNameAsync(roleName);
            if (role == null)
                throw new RoleNotFoundException("There isn't role named " + roleName);
           
            foreach (var user in usersId)
            {
                IdentityResult result = await IdentityManager.RemoveFromRoleAsync(user, role.Name);
                result.CheckResult();
            }
        }

        public async Task AddRole(string roleName)
        {
            IdentityResult result = await RoleManager.CreateAsync(role: new AppRole(roleName));

            if (!result.Succeeded)
                throw new RoleResultException(){    ErrorStrings = result.Errors    };
        }

        public async Task DeleteRole(string id)
        {

            AppRole role = await RoleManager.FindByIdAsync(id);

            if (role == null)
                throw new RoleNotFoundException($"There is no role of id: {id}");

            IdentityResult result = await RoleManager.DeleteAsync(role);

            if (!result.Succeeded)
                throw new RoleResultException() {ErrorStrings = result.Errors};
        }
    }
}