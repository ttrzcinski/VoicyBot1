using VoicyBot1.backend;
using Xunit;

namespace VoicyBot1Tests.backend
{
    public class UtilTimeTests
    {
        [Fact]
        public void Now_Test()
        {
            // Arrange
            var utilTime = new UtilTime();
            var expectedLength = 12;

            // Act
            var result = utilTime.Now;

            // Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result));
            Assert.Equal(expectedLength, result.Length);
        }

        [Fact]
        public void Today_Test()
        {
            // Arrange
            var utilTime = new UtilTime();
            var expectedLength = 8;

            // Act
            var result = utilTime.Today;

            // Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result));
            Assert.Equal(expectedLength, result.Length);
        }
    }
}
