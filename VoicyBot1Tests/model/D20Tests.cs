using VoicyBot1.model;
using Xunit;

namespace VoicyBot1Tests.model
{
    public class D20Tests
    {
        private readonly D20 _d20;

        public D20Tests()
        {
            _d20 = new D20();
        }

        [Fact]
        public void RollInRangeTest()
        {
            // Arrange
            
            // Act
            var score = _d20.Roll();

            // Assert
            Assert.InRange(score, 1, 20);
        }

        [Fact]
        public void RollInRangex100Test()
        {
            // Arrange

            // Act

            // Assert
            for (int i = 0; i < 100; i++) {
                var score = _d20.Roll();
                Assert.InRange(score, 1, 20);
            }
        }

        [Fact]
        public void InitiallyScoreIsEmptyTest()
        {
            Assert.Equal(_d20.LastScore(), -1);
        }

        [Fact]
        public void ReadLastRollTest()
        {
            // Arrange

            // Act

            // Assert
            var score = _d20.Roll();
            Assert.Equal(_d20.LastScore(), score);
        }

        [Fact]
        public void RespondTest()
        {
            // Arrange

            // Act
            var result = _d20.Respond("d20 roll");
            var isNumeric = int.TryParse(result, out int n);

            // Assert
            Assert.True(isNumeric);
        }
    }
}
