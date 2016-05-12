using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebLedMatrix.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLedMatrix.Logic.Text_Processing;

namespace WebLedMatrix.Hubs.Tests
{
    [TestClass()]
    public class WebpageResolverTests
    {
        WebpageResolver resolver = new WebpageResolver();

        [TestMethod]
        public void NoParseAddressTest()
        {
            var address = "www.somesite.com";
            
            Assert.AreEqual(address,resolver.ParseAddress(address));
        }
        
        [TestMethod()]
        public void ParseYoutubeAddressTest()
        {
            var result = resolver.ParseAddress("https://www.youtube.com/watch?v=Gld-19Ttn2M");

            Assert.AreEqual(result, "www.youtube.com/v/Gld-19Ttn2M?autoplay=1");
        }

        [TestMethod]
        public void ParsePornAddressTest()
        {
            var result = resolver.ParseAddress("www.pornsite.com");

            Assert.AreEqual(result,WebpageResolver.GoogleSiteAddress);
        }
    }
}