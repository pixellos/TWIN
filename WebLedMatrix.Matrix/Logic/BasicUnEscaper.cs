using System.Text.RegularExpressions;

namespace WebLedMatrix.Matrix.Logic
{
    public class BasicUnEscaper : ITextFormater
    {
        public string Format(string text)
        {
            return Regex.Unescape(text);
        }
    }
}