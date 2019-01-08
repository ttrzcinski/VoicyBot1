using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace VoicyBot1.backend
{
    /// <summary>
    /// Holds methods usable with JSON processing.
    /// </summary>
    public class UtilJSON
    {
        /// <summary>
        /// Coverts JSON to dictionary.
        /// </summary>
        /// <param name="json">given JSON</param>
        /// <returns>full dictionary means, finished conversion, null means error</returns>
        public Dictionary<string, string> dictionaryFromJSON(string json)
        {
            if (String.IsNullOrWhiteSpace(json)) return null;
            Dictionary<string, string> elements = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            return elements != null && elements.Count > 0 ? elements : null;
        }

        /// <summary>
        /// Converts dictionary to JSON.
        /// </summary>
        /// <param name="dictionary">given dictionary</param>
        /// <returns>json of dictionary, if it had content, null otherwise</returns>
        public string dictionaryToJSON(Dictionary<string, string> dictionary)
        {
            if (dictionary == null || dictionary.Count == 0) return null;
            return JsonConvert.SerializeObject(dictionary, Formatting.Indented);
        }
    }
}
