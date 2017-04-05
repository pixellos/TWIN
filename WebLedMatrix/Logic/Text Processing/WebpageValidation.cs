using System.Text.RegularExpressions;

namespace WebLedMatrix.Server.Logic.Text_Processing
{
    public class WebpageValidation
    {
        internal static readonly string PornRegex = @"(porn)";
        public const string GoogleSiteAddress = "www.google.com";

        private string EmbeddedYoutubeAddress(string videoId)
        {
            return $"www.youtube.com/v/{videoId}?autoplay=1";
        }

        public string ParseAddress(string address)
        {
            return YoutubeParse(address) ?? PornParse(address) ?? address;
        }

        private string PornParse(string address)
        {
            Regex regex = new Regex(PornRegex);
            if (regex.Match(address).Success)
            {
                return GoogleSiteAddress;
            }
            return address;
        }

        private string YoutubeParse(string address)
        {
            Regex regex = new Regex(@".*youtube(.*)watch[?]v=(\S*)");
            var youtubeRegex = regex.Match(address);
            if (youtubeRegex.Success)
            {
                return EmbeddedYoutubeAddress(youtubeRegex.Groups[2].Value);
            }
            return GoogleSiteAddress;
        }
    }
}