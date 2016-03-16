using StorageTypes;
using Xunit;

namespace StorageTypesTests
{
    public class DisplayDataTypeExtensionsMethodsTests
    {
        [Fact]
        public void GetDataToDisplayTest()
        {
            string testString = "Test";

            var data = new DataToDisplay() { DisplayDataType = DisplayDataType.Text, Data = testString };
            var fromFunction = DisplayDataType.Text.GetDataToDisplay(testString);

            Assert.True(data.Equals(fromFunction));
        }
    }
}