using Xunit;
using WebLedMatrix.Controllers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Moq;
using NSubstitute.Core.Arguments;
using static WebLedMatrix.Controllers.IdentityResultExtensions;
namespace WebLedMatrix.Controllers.Tests
{
    [ExcludeFromCodeCoverage]
    public class IdentityResultWrapperTests
    {
        [Fact()]
        public void FoldMessagesTest()
        {
            string Test = nameof(Test);
            string Something_Goes_Wrong = nameof(Something_Goes_Wrong);

            Controller controller = new AccountController();

            var result = IdentityResult.Success;
            
            controller.FoldMessages(IdentityResult.Failed(Test),Something_Goes_Wrong);

            Assert.Equal(controller.ModelState[""].Errors[0].ErrorMessage,Something_Goes_Wrong);
        } 
    }
}