using System;

namespace VoicyBot1.backend
{
    public class UtilTime
    {
        /// <summary>
        /// Returns current timestamp as one continous string yyyyMMddHHmm.
        /// </summary>
        /// <returns>now as yyyyMMddHHmm</returns>
        public string Now() => DateTime.Now.ToString("yyyyMMddHHmm");
    }
}
