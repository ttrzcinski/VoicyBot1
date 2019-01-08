using System;
using System.Collections.Generic;
using System.Linq;
using VoicyBot1.backend;
using Xunit;
using System.Threading.Tasks;
using Xunit;

namespace VoicyBot1Tests.backend
{
    public class UtilJSONTests
    {
        [Theory]
        [InlineData(null, 0, false)]
        [InlineData("", 0, false)]
        [InlineData("   ", 0, false)]
        [InlineData("  \n  ", 0, false)]
        [InlineData("{\"q1\":\"a1\"}", 1, true)]
        [InlineData("{\"q1\":\"a1\",\"q2\":\"a2\"}", 2, true)]
        [InlineData("{\"q1\":\"a1\",\"q2\":\"a2\",}", 2, true)]
        [InlineData("[\"q1\":\"a1\",\"q2\":\"a2\"]", 0, false)]
        [InlineData("{}", 0, false)]
        [InlineData("[]", 0, false)]
        [InlineData("{\"q1\"}", 0, false)]
        [InlineData("[\"q1\"]", 0, false)]
        [InlineData("{\"q1\"", 0, false)]
        [InlineData("hello test ", 0, false)]
        public void DictionaryFromJSON_resultTest(string json, int size, bool expectsValue)
        {
            // Arrange
            var utilJson = new UtilJSON();
            var expected = new Dictionary<string, string>();
            for (int i = 1; i <= size; i++)
            {
                expected.Add("q" + i, "a" + i);
            }

            // Act
            var result = utilJson.DictionaryFromJSON(json);

            // Assert
            if (expectsValue == false)
            {
                Assert.Null(result);
            }
            else
            {
                Assert.NotNull(result);
                Assert.Equal(size, result.Count);
                foreach (KeyValuePair<string, string> elem in expected)
                {
                    Assert.True(result.ContainsKey(elem.Key));
                    Assert.Equal(result[elem.Key], elem.Value);
                }
            }
        }

        [Fact]
        public void DictionaryFromJSON_typeTest()
        {
            // Arrange
            var utilJson = new UtilJSON();
            var json = "{\"q1\":\"a1\"}";

            // Act
            var result = utilJson.DictionaryFromJSON(json);

            // Assert
            Assert.NotNull(result);
            var dict = new Dictionary<string, string>();
            Type type = dict.GetType();
            Assert.IsType(type, result);
        }

        [Theory]
        [InlineData("null", false)]
        [InlineData("empty", false)]
        [InlineData("one_elem_good", true)]
        [InlineData("two_elem_good", true)]
        public void DictionaryToJSON_resultTest(string value, bool expectsResult)
        {
            // Arrange
            var utilJson = new UtilJSON();
            Dictionary<string, string> processed = null;
            switch (value)
            {
                case "null":
                    processed = null;
                    break;
                case "empty":
                    processed = new Dictionary<string, string>();
                    break;
                case "one_elem_good":
                    processed = new Dictionary<string, string>
                    {
                        { "q1", "a1" }
                    };
                    break;
                case "two_elem_good":
                    processed = new Dictionary<string, string>
                    {
                        { "q1", "a1" },
                        { "q2", "a2" }
                    };
                    break;
                default:
                    Assert.Empty("Dictionary must be initialized in right way.");
                    break;
            }

            // Act
            var result = utilJson.DictionaryToJSON(processed);

            // Assert
            if (expectsResult == true)
            {
                Assert.NotNull(result);
            }
            else
            {
                Assert.Null(result);
            }
        }

        [Fact]
        public void DictionaryToJSON_typeTest()
        {
            // Arrange
            var utilJson = new UtilJSON();
            var json = "{\"q1\":\"a1\"}";

            // Act
            var result = utilJson.DictionaryFromJSON(json);

            // Assert
            Assert.NotNull(result);
            var dict = new Dictionary<string, string>();
            Type type = dict.GetType();
            Assert.IsType(type, result);
        }
    }
}
