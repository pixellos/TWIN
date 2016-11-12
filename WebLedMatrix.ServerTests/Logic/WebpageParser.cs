using System.Diagnostics.CodeAnalysis;
using WebLedMatrix.Server.Logic.Text_Processing;
using Xunit;

namespace Test.WebLedMatrix.Server.Logic
{
    [ExcludeFromCodeCoverage]
    public class WebpageResolverTests
    {
        WebpageValidation _validation = new WebpageValidation();

        [Fact]
        public void NoParseAddressTest()
        {
            var address = "www.somesite.com";
            
            Assert.Equal(address,_validation.ParseAddress(address));
        }

        [Fact]
        public void ParseYoutubeAddressTest()
        {
            var result = _validation.ParseAddress("https://www.youtube.com/watch?v=Gld-19Ttn2M");

            Assert.Equal(result, "www.youtube.com/v/Gld-19Ttn2M?autoplay=1");
        }

        [Fact]
        public void ParsePornAddressTest()
        {
            var result = _validation.ParseAddress("www.pornsite.com");

            Assert.Equal(result,WebpageValidation.GoogleSiteAddress);
        }
    }
}