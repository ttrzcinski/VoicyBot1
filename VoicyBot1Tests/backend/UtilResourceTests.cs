using VoicyBot1.backend;
using Xunit;

namespace VoicyBot1Tests.backend
{
    public class UtilResourceTests
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData("str.json", "resources\\str.json")]
        public void PathToResource_Test(string entered, string outcome)
        {
            // Arrange
            var utilResource = UtilResource.Instance;

            // Act
            var result = utilResource.PathToResource(entered);

            // Assert
            if (outcome == null)
            {
                Assert.Null(result);
            }
            else
            {
                Assert.NotNull(result);
                Assert.Contains(outcome, result);
            }
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("    ", false)]
        [InlineData("  aimpossiblenameofresourcewhichdoesntexists  ", false)]
        [InlineData("retorts.json", true)]
        public void Exists_Test(string entered, bool outcome)
        {
            // Arrange
            var utilResource = UtilResource.Instance;

            // Act
            var result = utilResource.Exists(entered);

            // Assert
            Assert.Equal(outcome, result);
        }
    }
}
