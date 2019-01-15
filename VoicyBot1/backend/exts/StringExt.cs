namespace VoicyBot1.backend.exts
{
    public static class StringExt
    {
        /// <summary>
        /// Cuts given line after pointed length and chops it whole in a-like pieces.
        /// </summary>
        /// <param name="value">given line to cut</param>
        /// <param name="lineLength">wanted length</param>
        /// <returns>array with those strings, if worked, null means wrong data</returns>
        public static string[] SplitAfter(this string value, int lineLength)
        {
            // Check, if value has content
            if (string.IsNullOrWhiteSpace(value) || lineLength < 1) return null;
            // Check, if given value is not longer, than lineLength
            if (value.Length <= lineLength)
            {
                return new[] { value };
            }
            // Process it as longer, than 1
            var numOfItems = value.Length % lineLength;
            var items = new string[numOfItems];
            for (var i = 0; i < numOfItems; i++)
            {
                items[i] = value.Substring(i * lineLength, (i + 1) * lineLength);
            }
            return items;
        }
    }
}
