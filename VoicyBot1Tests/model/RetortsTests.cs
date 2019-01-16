using System;
using System.Linq;
using VoicyBot1.model;
using Xunit;

namespace VoicyBot1Tests.model
{
    public class RetortsTests
    {
        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("   ", false)]
        [InlineData("  \n  ", false)]
        [InlineData("add-retort|some_question|some_answer", true)]
        [InlineData("add-retort|question1|answer1", false)]
        // TODO USAGE OF MOCKED LIST
        public void AddTest(string line, bool expected)
        {
            // Arrange
            var retorts = new Retorts();

            // Act
            var result = retorts.Add(line);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
