using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Moq;
using WebLedMatrix.Controllers;
using WebLedMatrix.Logic.Authentication.Infrastructure;
using WebLedMatrix.Logic.Authentication.Models;
using WebLedMatrix.Logic.Authentication.Models.Roles;
using WebLedMatrix.Models.Authentication.Roles;
using Xunit;

namespace WebLedMatrix.Controllers.Tests
{

    [ExcludeFromCodeCoverage]
    public class RoleAdministratingTests
    {
        [Fact()]
        public void ThrowIfNullTest_Null()
        {
            string nullstring = null;

            Assert.Throws<ArgumentNullException>(()=>
                NullExpressionsExtensions.ThrowIfNull(nullstring,nameof(nullstring)));
        }

        [Fact()]
        public void ThrowIfNullTest_NotNull()
        {
            string nullstring = "Not null";

            NullExpressionsExtensions.ThrowIfNull(nullstring, nameof(nullstring));
            Assert.True(nullstring != null);
        }
    }
}

namespace Test.WebLedMatrix.Server.Controllers
{
    [ExcludeFromCodeCoverage]
    public class RoleAdministratingTests
    {
        private string userRole = nameof(userRole);

        private static void UserIdentityManagerAction(Mock<UserIdentityManager> mock)
        {
            mock.Setup(x => x.AddToRoleAsync(null, null)).ReturnsAsync(IdentityResult.Success);
        }


        RoleAdministrating SetupAppRoleManager(Action<Mock<AppRoleManager>> setupAppRoleAction, Action<Mock<UserIdentityManager>> setupIdentityManagerAction)
        {
            var appRoleMock = new Mock<AppRoleManager>(new RoleStore<AppRole>());
            setupAppRoleAction.Invoke(appRoleMock);

            var identityManagerMock = new Mock<UserIdentityManager>(new UserStore<User>());
            setupIdentityManagerAction.Invoke(identityManagerMock);

            return new RoleAdministrating(appRoleMock.Object, identityManagerMock.Object);
        }

        private Action<Mock<AppRoleManager>> SetupAppRoleAction
        {
            get
            {
                return _ => _.Setup(x => x.FindByNameAsync(userRole))
                    .ReturnsAsync(new AppRole(userRole)
                    {
                        Id = userRole,
                        Users =
                        {
                            new IdentityUserRole() {RoleId = "0", UserId = "0"},
                        }
                    });
            }
        }

        [Fact()]
        public async void AddMembersTest_ThrownUsersReccuringException()
        {
            var manager = SetupAppRoleManager(SetupAppRoleAction, UserIdentityManagerAction);

            await Assert.ThrowsAsync<UsersAreRecurringException>(() => manager.AddMembers(userRole, new[] { "0", "1" }));
        }


        [Fact()]
        public async void DeleteMembersTest()
        {
            var manager = SetupAppRoleManager(SetupAppRoleAction, _ =>
            {
                UserIdentityManagerAction(_);
                _.Setup(x => x.RemoveFromRoleAsync("0", userRole)).ReturnsAsync(IdentityResult.Success);
            });
            
            await manager.DeleteMembers(userRole, new[] { "0" });
        }

        [Fact()]
        public async void AddRoleTest()
        {
            AppRole newRole = new AppRole(nameof(newRole));

            var manager = SetupAppRoleManager(
                _ => { SetupAppRoleAction(_);
                    _.Setup(x => x.CreateAsync(It.IsAny<AppRole>())).ReturnsAsync(IdentityResult.Success);
                },
                UserIdentityManagerAction);

            await manager.AddRole(newRole.Id);
        }

        [Fact( )]
        public async void DeleteRoleTest()
        {
            AppRole newRole = new AppRole(nameof(newRole));
            var manager = SetupAppRoleManager(
                _ => {
                    SetupAppRoleAction(_);
                         _.Setup(x => x.DeleteAsync(newRole)).ReturnsAsync(IdentityResult.Success);

                    _.Setup(x => x.FindByIdAsync(newRole.Id)).ReturnsAsync(newRole);
                    _.Setup(x => x.CreateAsync(newRole)).ReturnsAsync(IdentityResult.Success);
                },
                UserIdentityManagerAction);

            await manager.DeleteRole(newRole.Id);
        }
    }
}