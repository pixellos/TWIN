using Xunit;
using WebLedMatrix.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using WebLedMatrix.Logic.Authentication.Infrastructure;
using WebLedMatrix.Logic.Authentication.Models.Roles;

namespace WebLedMatrix.Controllers.Tests
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