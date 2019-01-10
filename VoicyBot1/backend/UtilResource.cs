using System;
using System.IO;

namespace VoicyBot1.backend
{
    /// <summary>
    /// Toolbox of methods to call on resources.
    /// </summary>
    public class UtilResource
    {
        /// <summary>
        /// Prepares retorts filepath.
        /// </summary>
        /// <returns>retorts filepath</returns>
        public string PathToResource(string name)
        {
            if (String.IsNullOrWhiteSpace(name)) return null;
            return Path.Combine(Directory.GetCurrentDirectory(), string.Format("resources\\{0}.json", name));
        }

        /// <summary>
        /// Checks, if resource with wanted name exists.
        /// </summary>
        /// <param name="name">watned name</param>
        /// <returns>true means, it exists, false otherwise</returns>
        public bool Exists(string name)
        {
            var path = PathToResource(name);
            return path != null ? File.Exists(path) : false;
        }
    }
}
