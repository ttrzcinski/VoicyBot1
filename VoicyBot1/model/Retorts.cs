using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VoicyBot1.model
{
    public class Retorts
    {
        private Dictionary<string, string> _retorts;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of Retorts.
        /// </summary>
        /// <param name="loggerFactory">A <see cref="ILoggerFactory"/> that is hooked to the Azure App Service provider.</param>
        /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-2.1#windows-eventlog-provider"/>
        public Retorts(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new System.ArgumentNullException(nameof(loggerFactory));
            }

            _logger = loggerFactory.CreateLogger<Retorts>();
            _logger.LogTrace("Retorts initialized.");
            AssureNotNull_retorts();
        }

        /// <summary>
        /// Checks, if loaded retorts are empty and loads them from resource JSON file as dictionary base.
        /// </summary>
        private void AssureNotNull_retorts()
        {
            _logger.LogInformation("AssureNotNull_retorts was initialized.");

            // Check, if rtorts were already loaded
            if (_retorts != null && _retorts.Count > 0) return;

            // Check, if retorts file is present
            var path = Path.Combine(Directory.GetCurrentDirectory(), "resources\\retorts.json");
            //string path = Path.Combine(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory), @"resources\retorts.json");
            _logger.LogInformation("AssureNotNull_retorts will be using file " + path);
            if (!File.Exists(path))
            {
                _logger.LogError("AssureNotNull_retorts - Couldn't load retorts file - file is missing.");
                return;
            }

            _retorts = new Dictionary<string, string>();
            _logger.LogInformation("AssureNotNull_retorts - Initialized empty list of retorts.");
            string retortsContentArray = File.ReadAllText(path);
            _logger.LogInformation("AssureNotNull_retorts - Reads - " + retortsContentArray);
            // Load retors from a file
            var jsonArray = JArray.Parse(retortsContentArray);
            _logger.LogInformation("AssureNotNull_retorts - Parsed: " + jsonArray.ToString());
            // Loop through list of elements
            foreach (JObject element in jsonArray)
            {
                _logger.LogInformation("AssureNotNull_retorts - element " + element.ToString());
                _retorts.Add(element["question"].ToString(), element["answer"].ToString());
            }
            _logger.LogInformation("AssureNotNull_retorts was done with retorts: " + _retorts.Count);
        }

        /// <summary>
        /// Responds with an answer, if there is one in retorts.
        /// </summary>
        /// <param name="question">given question</param>
        /// <returns>response, if there is one, null otherwise</returns>
        public string Respond(string question)
        {
            string response = null;
            _logger.LogInformation("Respond - working with retorts.");
            foreach (KeyValuePair<string, string> retort in _retorts)
            {
                _logger.LogInformation("Respond - working with retort: " + retort.ToString());
                if (retort.Key.Equals(question))
                {
                    _logger.LogInformation("Respond - worked comparison for retort: " + retort.Key.ToString());
                    response = retort.Value;
                    break;
                }
            }

            return response;
        }
    }
}
