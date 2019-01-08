using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using VoicyBot1.backend;

namespace VoicyBot1.model
{
    public class Retorts
    {
        private UtilJSON utilJson;
        /// <summary>
        /// Kept decitionary of retorts
        /// </summary>
        private Dictionary<string, string> _retorts;
        /// <summary>
        /// Standard logger used for debug.
        /// </summary>
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

            utilJson = new UtilJSON();
            Load();
        }

        /// <summary>
        /// Checks, if loaded retorts are null and initalizes them.
        /// </summary>
        private void AssureNotNull_retorts()
        {
            _logger.LogInformation("AssureNotNull_retorts was initialized.");
            if (_retorts == null)
            {
                _retorts = new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// Prepares retorts filepath.
        /// </summary>
        /// <returns>retorts filepath</returns>
        private string PrepareFilePath() => Path.Combine(Directory.GetCurrentDirectory(), "resources\\retorts.json");

        /// <summary>
        /// Loads retorts from file.
        /// </summary>
        private void Load()
        {
            // Check, if retorts were already loaded
            if (_retorts == null)
            {
                AssureNotNull_retorts();
            }
            else
            {
                _retorts.Clear();
            }

            // Check, if retorts file is present
            var path = PrepareFilePath();
            _logger.LogInformation("Load will be using file " + path);
            if (!File.Exists(path))
            {
                _logger.LogError("Load - Couldn't load retorts file - file is missing.");
                return;
            }

            _logger.LogInformation("Load - Initialized empty list of retorts.");
            // Load retors from a file
            string retortFileContent = File.ReadAllText(path);
            _logger.LogInformation("Load - Reads - " + retortFileContent);
            Dictionary<string, string> result = utilJson.dictionaryFromJSON(retortFileContent);
            if (result != null)
            {
                _retorts = result;
                _logger.LogInformation("Load was done with retorts: " + _retorts.Count);
            }
            else
            {
                _logger.LogInformation("Load was cancelled due malformed JSON.");
            }
        }

        /// <summary>
        /// Adds new retorts from given command line.
        /// </summary>
        /// <param name="fromLine">given command line</param>
        /// <returns>true, if added, false means error with logged reason.</returns>
        public bool Add(string fromLine)
        {
            if (String.IsNullOrWhiteSpace(fromLine))
            {
                _logger.LogError("Add - Given line was empty.");
                return false;
            }

            string[] lineParts = fromLine.Split("|");
            return lineParts.Length > 2 ? Add(lineParts[1].Trim(), lineParts[2].Trim()) : false;
        }

        /// <summary>
        /// Adds new retort to dictionary.
        /// </summary>
        /// <param name="question">given question</param>
        /// <param name="answer">given answer</param>
        /// <returns>true means added, false means error with logged reason.</returns>
        public bool Add(string question, string answer)
        {
            if (String.IsNullOrWhiteSpace(question))
            {
                _logger.LogError("Add - Given question is empty.");
                return false;
            }
            else if (String.IsNullOrWhiteSpace(answer))
            {
                _logger.LogError("Add - Given answer is empty.");
                return false;
            }

            AssureNotNull_retorts();

            if (_retorts.ContainsKey(question))
            {
                _logger.LogError("Add - Given key already exists.");
                return false;
            }

            _retorts.Add(question, answer);

            var path = PrepareFilePath();

            var convertedJson = utilJson.dictionaryToJSON(_retorts);
            if (convertedJson != null)
            {
                File.WriteAllText(path, convertedJson);
                return true;
            }
            else
            {
                _logger.LogError("Add - converted JSON of dictionary was null");
                return false;
            }
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
