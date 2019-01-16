using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using VoicyBot1.backend;
using VoicyBot1.backend.interfaces;

namespace VoicyBot1.model
{
    public class Retorts : ISkill
    {
        /// <summary>
        /// toolbox with resource methods.
        /// </summary>
        private UtilResource utilResource;
        /// <summary>
        /// Toolbox with JSON methods.
        /// </summary>
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
        /// Holds all the errors, which occured in the run.
        /// </summary>
        private List<string> _errors;

        /// <summary>
        /// Initializes a new instance of Retorts.
        /// </summary>
        public Retorts()
        {
            AssureNotNull_retorts();
            AssureNotNull_errors();

            utilResource = UtilResource.Instance;
            utilJson = UtilJSON.Instance;
            Load();
        }

        /// <summary>
        /// Checks, if loaded retorts are null and initalizes them.
        /// </summary>
        private void AssureNotNull_retorts()
        {
            if (_retorts == null)
                _retorts = new Dictionary<string, string>();
        }

        /// <summary>
        /// Checks, if loaded retorts are null and initalizes them.
        /// </summary>
        private void AssureNotNull_errors()
        {
            if (_errors == null)
                _errors = new List<string>();
        }

        /// <summary>
        /// Clears dictionary.
        /// </summary>
        private void Clear()
        {
            if (_retorts == null)
            {
                _retorts = new Dictionary<string, string>();
            }
            else
            {
                _retorts.Clear();
            }
        }

        /// <summary>
        /// Is used to log down errors.
        /// </summary>
        /// <param name="line">given line to log</param>
        private void error(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                _errors.Add(line);
        }

        /// <summary>
        /// Loads retorts from file.
        /// </summary>
        private void Load()
        {
            // Check, if retorts were already loaded
            Clear();

            // Check, if retorts file is present
            var path = utilResource.PathToResource("retorts.json");
            if (!File.Exists(path))
            {
                error("Load - Couldn't load retorts file - file is missing.");
                return;
            }

            // Load retors from a file
            string retortFileContent = File.ReadAllText(path);
            Dictionary<string, string> result = utilJson.DictionaryFromJSON(retortFileContent);
            if (result != null)
            {
                _retorts = result;
            }
            else
            {
                error("Load was cancelled due malformed JSON.");
            }
        }

        /// <summary>
        /// Adds new retorts from given command line.
        /// </summary>
        /// <param name="fromLine">given command line</param>
        /// <returns>true, if added, false means error with logged reason.</returns>
        public bool Add(string fromLine)
        {
            if (string.IsNullOrWhiteSpace(fromLine))
            {
                error("Add - Given line was empty.");
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
            if (string.IsNullOrWhiteSpace(question))
            {
                error("Add - Given question is empty.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(answer))
            {
                error("Add - Given answer is empty.");
                return false;
            }

            AssureNotNull_retorts();

            if (_retorts.ContainsKey(question))
            {
                error("Add - Given key already exists.");
                return false;
            }

            _retorts.Add(question, answer);

            var path = utilResource.PathToResource("retorts.json");

            var convertedJson = utilJson.DictionaryToJSON(_retorts);
            if (convertedJson != null)
            {
                File.WriteAllText(path, convertedJson);
                return true;
            }
            error("Add - converted JSON of dictionary was null");
            return false;
        }

        // TODO ADD REMOVE

        // TODO ADD save to file

        // TODO ADD LIST ALL ERRORS

        /// <summary>
        /// Responds with an answer, if there is one in retorts.
        /// </summary>
        /// <param name="question">given question</param>
        /// <returns>response, if there is one, null otherwise</returns>
        public string Respond(string question)
        {
            // Checked entered questino
            if (string.IsNullOrWhiteSpace(question)) return null;
            
            // Prepare response
            string response = null;
            // Check, if it is not one of commands
            if (question.StartsWith("add-retort|", System.StringComparison.Ordinal))
            {
                return Add(question)
                    ? "Added new retort."
                    : "Couldn't process " + question;
            }
            // Check, if retorts are loaded
            if (_retorts.Count == 0) return "Respond - Retorts are emopty.";
            // Process with retorts
            if (_retorts.ContainsKey(question))
            {
                error(string.Format("Respond - working with retort: {0} and answer {1}.", question, _retorts[question]));
                return _retorts[question];
            }

            return response;
        }
    }
}
