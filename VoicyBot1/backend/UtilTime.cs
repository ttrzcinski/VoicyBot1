using System;

namespace VoicyBot1.backend
{
    public sealed class UtilTime
    {
        /// <summary>
        /// Object for lazy initialization. 
        /// </summary>
        private static readonly Lazy<UtilTime> lazy = new Lazy<UtilTime>(() => new UtilTime());

        /// <summary>
        /// Returns the only instance of this class.
        /// </summary>
        public static UtilTime Instance { get { return lazy.Value; } }

        /// <summary>
        /// Creates new instance of util time - only once as it is singleton.
        /// </summary>
        private UtilTime() {}

        /// <summary>
        /// Returns current timestamp as one continous string yyyyMMddHHmm.
        /// </summary>
        /// <returns>now as yyyyMMddHHmm</returns>
        public static string Now => DateTime.Now.ToString("yyyyMMddHHmm");

        /// <summary>
        /// Returns current date as one continous string yyyyMMdd.
        /// </summary>
        /// <returns>today as yyyyMMdd</returns>
        public static string Today => DateTime.Now.ToString("yyyyMMdd");
    }
}
