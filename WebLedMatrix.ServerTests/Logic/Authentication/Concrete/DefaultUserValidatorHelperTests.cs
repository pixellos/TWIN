using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNet.Identity;
using WebLedMatrix.Logic.Authentication.Concrete;
using Xunit;

namespace Test.WebLedMatrix.Server.Logic.Authentication.Concrete
{
    [ExcludeFromCodeCoverage]
    public class DefaultUserValidatorHelperTests
    {
        [Fact()]
        public void CheckResult_ThrowException_WhenIdentityResultIsFalse()
        {
            Assert.Throws<ResultException>(
                ()=>IdentityResult.Failed("Error").CheckResult());
        }



        [Fact()]
        public void CheckResult_Pass_WhenIdentityResultIsTrue()
        {
            IdentityResult.Success.CheckResult();

            Assert.True(true);
        }
    }
}