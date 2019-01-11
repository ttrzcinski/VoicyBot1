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

            // Act
            var result = utilTime.Now();

            // Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result));
            Assert.Equal(12, result.Length);
        }
    }
}
