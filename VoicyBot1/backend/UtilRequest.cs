using Microsoft.AspNetCore.StaticFiles;
using System;
namespace VoicyBot1.backend
{
    public sealed class UtilRequest
    {
        /// <summary>
        /// Object for lazy initialization. 
        /// </summary>
        private static readonly Lazy<UtilRequest> lazy = new Lazy<UtilRequest>(() => new UtilRequest());

        /// <summary>
        /// Returns the only instance of this class.
        /// </summary>
        public static UtilRequest Instance { get { return lazy.Value; } }

        /// <summary>
        /// Creates new instance of util request - only once as it is singleton.
        /// </summary>
        private UtilRequest() {}

        /// <summary>
        /// Checks, if given string is an url.
        /// </summary>
        /// <param name="given">given string as url</param>
        /// <returns>true, if it is url, false otherwise</returns>
        public static bool IsURL(string given) => string.IsNullOrWhiteSpace(given) ? false : Uri.IsWellFormedUriString(given, UriKind.RelativeOrAbsolute);

        /// <summary>
        /// Checks, if given string is a number.
        /// </summary>
        /// <param name="given">given string as number</param>
        /// <returns>true, if it is a number ,false otherwise</returns>
        public static bool IsNumber(string given) => string.IsNullOrWhiteSpace(given) ? false : int.TryParse(given, out int num);

        /// <summary>
        /// Parses mime type (content type) from given url from it's ending.
        /// </summary>
        /// <param name="url">given url</param>
        /// <returns>name of cotnent type if found, null otheriwse</returns>
        public static string ParseMimeType(string url)
        {
            if (string.IsNullOrWhiteSpace(url) || url.Contains(".")) return null;
            string ending = url.Substring(url.LastIndexOf(".", StringComparison.Ordinal)).Trim().ToLower();
            if (ending.Length == 0) return null;
            new FileExtensionContentTypeProvider().TryGetContentType(ending, out string contentType);
            return contentType ?? "application/octet-stream";
        }
    }
}
