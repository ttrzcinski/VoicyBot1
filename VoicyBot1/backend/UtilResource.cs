using System;
using System.IO;

namespace VoicyBot1.backend
{
    /// <summary>
    /// Toolbox of methods to call on resources.
    /// </summary>
    public sealed class UtilResource
    {
        /// <summary>
        /// Holds current directory path.
        /// </summary>
        private string _currDirectory;

        /// <summary>
        /// Object for lazy initialization. 
        /// </summary>
        private static readonly Lazy<UtilResource> lazy = new Lazy<UtilResource>(() => new UtilResource());

        /// <summary>
        /// Returns the only instance of this class.
        /// </summary>
        public static UtilResource Instance { get { return lazy.Value; } }

        /// <summary>
        /// Creates new instance of util resource - only once as it is singleton.
        /// </summary>
        private UtilResource()
        {
            // Provide current directory - bot, not unit tests
            _currDirectory = Directory.GetCurrentDirectory();
            if (_currDirectory.Contains("Tests")) _currDirectory = _currDirectory.Replace("Tests", "");
            if (_currDirectory.Contains("bin")) _currDirectory = _currDirectory.Substring(0, _currDirectory.IndexOf("bin", StringComparison.Ordinal));
        }

        /// <summary>
        /// Prepares retorts file path.
        /// </summary>
        /// <returns>retort's full file path</returns>
        public string PathToResource(string name) => 
            string.IsNullOrWhiteSpace(name) ? null : Path.Combine(_currDirectory, string.Format("resources\\{0}", name));

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

        /// <summary>
        /// Creates new resource with given name.
        /// </summary>
        /// <param name="name">given name</param>
        /// <returns>true means created new resource, false otherwise</returns>
        public bool Create(string name)
        {
            if (Exists(name)) return false;
            name = PathToResource(name);
            string content = name.EndsWith("json", StringComparison.Ordinal) ? "{}" : "";
            bool result = false;
            try
            {
                File.WriteAllText(name, "");
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            
            return result;
        }

        /// <summary>
        /// Renames pointed file.
        /// </summary>
        /// <param name="oldName">file name before change</param>
        /// <param name="newName">file anme after cahgne</param>
        /// <returns>true means it worked, false otherwise</returns>
        public bool Rename(string oldName, string newName)
        {
            if (!Exists(oldName) || Exists(newName)) return false;
            bool result = false;
            oldName = PathToResource(oldName);
            newName = PathToResource(newName);
            try
            {
                File.Move(oldName, newName);
                File.Delete(oldName);
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Copies pointed resoruce from pointed source to target name.
        /// </summary>
        /// <param name="sourceName">source name</param>
        /// <param name="targetName">target name</param>
        /// <returns></returns>
        public bool Copy(string sourceName, string targetName)
        {
            if (!Exists(sourceName) || Exists(targetName)) return false;
            bool result = false;
            sourceName = PathToResource(sourceName);
            targetName = PathToResource(targetName);
            try
            {
                File.Copy(sourceName, targetName);
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Backups resource via coping it with timestamped name.
        /// </summary>
        /// <param name="name">resource's name</param>
        /// <returns>truem means copies, false otherwise</returns>
        public bool Backup(string name)
        {
            if (!Exists(name)) return false;
            string bckpName = string.Format("bckp_{0}_{1}", UtilTime.Now, PathToResource(name));
            return Exists(bckpName) ? false : Copy(name, bckpName);
        }
    }
}
