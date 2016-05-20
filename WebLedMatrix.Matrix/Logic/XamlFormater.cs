namespace WebLedMatrix.Matrix.Logic
{
    public class XamlFormater : ITextFormater
    {
        private string NewLineXamlLiteral = "&#x0a;";

        public string Format(string text)
        {
            return text.Replace(@"\n", NewLineXamlLiteral);
        }
    }
}