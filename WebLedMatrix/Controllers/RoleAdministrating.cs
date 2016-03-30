using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WebGrease.Css.Extensions;
using WebLedMatrix.Logic.Authentication.Infrastructure;
using WebLedMatrix.Logic.Authentication.Models;
using WebLedMatrix.Logic.Authentication.Models.Roles;

namespace WebLedMatrix.Controllers
{
    public class RoleResultException : Exception
    {
        public RoleResultException(){}

        public RoleResultException(string message) : base(message){}

        public RoleResultException(string message, Exception innerException) : base(message, innerException){}

        protected RoleResultException(SerializationInfo info, StreamingContext context) : base(info, context){}

        public IEnumerable<string> ErrorStrings { get; set; }
    }
    class UsersAreRecurringException : Exception
    {
        public UsersAreRecurringException()
        {
        }

        public UsersAreRecurringException(string message) : base(message)
        {
        }

        public UsersAreRecurringException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UsersAreRecurringException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
    class RoleNotFoundException : Exception
    {
        public RoleNotFoundException()
        {
        }

        public RoleNotFoundException(string message) : base(message)
        {
        }

        public RoleNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RoleNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class RoleAdministrating
    {
        private AppRoleManager _roleManager => HttpContext.Current.GetOwinContext().GetUserManager<AppRoleManager>();
        private UserIdentityManager _identityManager => HttpContext.Current.GetOwinContext().GetUserManager<UserIdentityManager>();

        public RoleAdministrating(AppRoleManager roleManager, UserIdentityManager identityManager)
        {

        }

        public async Task<RoleEditModel> CreateRoleEditModel(string id)
        {
            return CreateRoleEditModel(await _roleManager.FindByIdAsync(id));
        }

        public RoleEditModel CreateRoleEditModel(AppRole role)
        {
            var roleModel = new RoleEditModel();
            var userList = _identityManager.Users.ToList();
            var membersId = role.Users.Select(x=>x.UserId);


            roleModel.Role = role;
            roleModel.Members = userList.Where( x => membersId.Contains(x.Id));
            roleModel.NonMembers = userList.Except(roleModel.Members);

            return roleModel;
        }


        private static void CheckResult(IdentityResult result)
        {
            if (! result.Succeeded)
            {
                throw new RoleResultException("Result wasnt succeeded") { ErrorStrings = result.Errors };
            }
        }

        public async Task AddMembers(string roleName, IEnumerable<string> usersId)
        {
            if (roleName == null)
                throw new RoleNotFoundException("Role is null");

            if (usersId == null)
                return;

            if (! usersId.Any())
                return;

            AppRole role = await _roleManager.FindByNameAsync(roleName);

            if (role.Users.ToList().Select(x=>x.UserId).Intersect(usersId).Any())
                throw new UsersAreRecurringException("Users are recurring, no one were added.");

            foreach (var user in usersId)
            {
                IdentityResult result =  await _identityManager.AddToRoleAsync(user, role.Name);
                CheckResult(result);
            }
        }

        public async Task DeleteMembers(string name, IEnumerable<string> usersId)
        {
            if (name == null)
                throw new RoleResultException("Role is null");

            if (usersId == null)
                return;

            AppRole role = await _roleManager.FindByNameAsync(name);
            if (role == null)
                throw new RoleNotFoundException("There isn't role named " + name);
           
            foreach (var user in usersId)
            {
                IdentityResult result = await _identityManager.RemoveFromRoleAsync(user, role.Name);
                CheckResult(result);
            }
        }

        public async Task AddRole(string roleName)
        {
            IdentityResult result = await _roleManager.CreateAsync(new AppRole(roleName));

            if (!result.Succeeded)
                throw new RoleResultException(){    ErrorStrings = result.Errors    };
        }

        public async Task DeleteRole(string id)
        {

            AppRole role = await _roleManager.FindByIdAsync(id);

            if (role == null)
                throw new RoleNotFoundException($"There is no role of id: {id}");

            IdentityResult result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
                throw new RoleResultException() {ErrorStrings = result.Errors};
        }
    }
}