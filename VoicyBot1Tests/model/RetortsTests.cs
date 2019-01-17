using VoicyBot1.model;
using Xunit;

namespace VoicyBot1Tests.model
{
    public class RetortsTests
    {
        // TODO USAGE OF MOCKED LIST

        [Theory]
        [InlineData(null, false, false)]
        [InlineData("", false, false)]
        [InlineData("   ", false, false)]
        [InlineData("  \n  ", false, false)]
        [InlineData("add-retort|", false, false)]
        [InlineData("add-retort|   ", false, false)]
        [InlineData("add-retort|question1|", false, false)]
        [InlineData("add-retort|question1|   ", false, false)]
        [InlineData("add-retort|question1|answer1", true, false)]
        [InlineData("add-retort|question1|answer1", false, true)]
        [InlineData("addddd-ertort|question1|answer1", false, false)]
        public void AddTest(string line, bool expected, bool existsBefore)
        {
            // Arrange
            var retorts = new Retorts("test");
            retorts.Clear();
            var question = (!string.IsNullOrWhiteSpace(line)) ? line.Split("|")[1] : "";
            var answer = "defaultAnswer1";
            if (existsBefore) Assert.True(retorts.Add(question, answer));

            // Act
            var result = retorts.Add(line);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null, false, false)]
        [InlineData("", false, false)]
        [InlineData("   ", false, false)]
        [InlineData("  \n  ", false, false)]
        [InlineData("remove-retort|", false, false)]
        [InlineData("remove-retort|   ", false, false)]
        [InlineData("remove-retort|question1", false, false)]
        [InlineData("remove-retort|question1", true, true)]
        [InlineData("removeddd-ertort|question1", false, false)]
        public void RemoveTest(string line, bool expected, bool existsBefore)
        {
            // Arrange
            var retorts = new Retorts("test");
            retorts.Clear();
            var question = (!string.IsNullOrWhiteSpace(line)) ? line.Split("|")[1] : "";
            var answer = "defaultAnswer1";
            if (existsBefore) Assert.True(retorts.Add(question, answer));

            // Act
            var result = retorts.Remove(line);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ClearTest()
        {
            // Arrange
            var retorts = new Retorts("test");
            var question = "question1_test";
            var answer = "defaultAnswer1_test";
            retorts.Add(question.Trim(), answer);
            Assert.True(retorts.Count() > 0);
            var expectedSize = 0;

            // Act
            retorts.Clear();
            var actualSize = retorts.Count();

            // Assert
            Assert.Equal(expectedSize, actualSize);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void CountTest(bool mustHaveEntries)
        {
            // Arrange
            var retorts = new Retorts("test");
            retorts.Clear();
            var expectedSize = 0;
            if (mustHaveEntries)
            {
                var question = "question1_test";
                var answer = "defaultAnswer1_test";
                Assert.True(retorts.Add(question.Trim(), answer));
                expectedSize = 1;
            }

            // Act
            var actualSize = retorts.Count();

            // Assert
            Assert.Equal(expectedSize, actualSize);
        }

        [Theory]
        [InlineData(null, false, false)]
        [InlineData("", false, false)]
        [InlineData("   ", false, false)]
        [InlineData("  \n  ", false, false)]
        [InlineData("  question1  ", false, false)]
        [InlineData("question1", false, false)]
        [InlineData("question1", true, true)]
        public void ContainsTest(string question, bool expected, bool existsBefore)
        {
            // Arrange
            var retorts = new Retorts("test");
            retorts.Clear();
            var answer = "defaultAnswer1";
            if (existsBefore) retorts.Add(question.Trim(), answer);

            // Act
            var result = retorts.Contains(question);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
