using NSubstitute;
using WebLedMatrix.Controllers;
using WebLedMatrix.Logic.Authentication.Infrastructure;
using WebLedMatrix.Logic.Authentication.Models.Roles;
using Xunit;

namespace WebLedMatrixTests1.Controllers
{
    public class RoleAdministratingTests
    {
        public void RoleAdministratingTest()
        {
            AppRoleManager roleManager = Substitute.For<AppRoleManager>();
            UserIdentityManager identityManager = Substitute.For<UserIdentityManager>();

            RoleAdministrating roleAdministrating = new RoleAdministrating(roleManager,identityManager);
        }

        [Fact()]
        public void AddMembersTest()
        {

        }

        [Fact()]
        public void AddRoleTest()
        {

        }
    }
}