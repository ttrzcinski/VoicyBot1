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
    }
}
