using Xunit;
using WebLedMatrix.Matrix.Logic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.WebLedMatrix.Matrix.Logic
{
    public class XamlFormaterTests
    {
        [Fact()]
        public void FormatTest_ReplaceAllOccurences()
        {
            string TextWithThreeLineBreaks = @"FirstLine\nSecondLine\nThirdLine";
            string ExpectedResult = "FirstLine&#x0a;SecondLine&#x0a;ThirdLine";

            ITextFormater formater = new XamlFormater();

            Assert.Equal(
                ExpectedResult,
                formater.Format(TextWithThreeLineBreaks));
        }
    }
}