using VoicyBot1.backend.exts;
using Xunit;

namespace VoicyBot1Tests.backend.exts
{
    public class StringExtTests
    {
        [Theory]
        [InlineData(null, -1, null)]
        [InlineData(null, 0, null)]
        [InlineData("", -1, null)]
        [InlineData("some value", 15, new string[]{"some value"})]
        [InlineData("some value", 3, new string[]{"som"})]
        public void SplitAfterTest(string value, int lineLength, string[] result)
        {
            // Arrange

            // Act
            var actual = value.SplitAfter(lineLength);

            // Assert
            Assert.Equal(result, actual);
        }
    }
}
