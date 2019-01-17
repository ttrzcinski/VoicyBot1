using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using VoicyBot1.backend;
using VoicyBot1.backend.interfaces;

namespace VoicyBot1.model
{
    public class Retorts : ISkill
    {
        private readonly string FileName_Prod = "retorts.json";
        private readonly string FileName_Dev = "retorts_dev.json";
        private readonly string FileName_Test = "retorts_test.json";

        /// <summary>
        /// Currently used file name for retorts.
        /// </summary>
        private string _fileName;

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
        /// Holds all the errors, which occured in the run.
        /// </summary>
        private List<string> _errors;

        /// <summary>
        /// Initializes a new instance of Retorts.
        /// </summary>
        /// <param name="env">shortcut of wanted environment</param>
        public Retorts(string env = "prod")
        {
            // Set the right file of retorts
            switch (env)
            {
                case "prod":
                    _fileName = FileName_Prod;
                    break;
                case "dev":
                    _fileName = FileName_Dev;
                    break;
                case "test":
                    _fileName = FileName_Test;
                    break;
                default:
                    _fileName = FileName_Prod;
                    break;
            }

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
            if (_retorts == null) _retorts = new Dictionary<string, string>();
        }

        /// <summary>
        /// Checks, if loaded retorts are null and initalizes them.
        /// </summary>
        private void AssureNotNull_errors()
        {
            if (_errors == null) _errors = new List<string>();
        }

        /// <summary>
        /// Clears dictionary.
        /// </summary>
        public void Clear()
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
            var path = utilResource.PathToResource(_fileName);
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
            if (!fromLine.StartsWith("add-retort|", System.StringComparison.Ordinal))
            {
                error("Add - Given line has wrong beginning.");
                return false;
            }

            if (fromLine.Contains("|") 
                && Equals(fromLine.IndexOf("|", System.StringComparison.Ordinal), fromLine.LastIndexOf("|", System.StringComparison.Ordinal)))
            {
                error("Add - Given line has only one |.");
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

            return true;
        }

        /// <summary>
        /// Removes pointed retort.
        /// </summary>
        /// <param name="question">given command to call remvoe retort</param>
        /// <returns>true means removed, false otherwise</returns>
        public bool Remove(string question)
        {
            if (string.IsNullOrWhiteSpace(question))
            {
                error("Remove - Given line was empty.");
                return false;
            }
            question = question.Trim().ToLower();
            if (!question.StartsWith("remove-retort|", System.StringComparison.Ordinal))
            {
                error("Remove - Given line has wrong beginning.");
                return false;
            }
            question = question.Split("|")[1].Trim();
            if (string.IsNullOrWhiteSpace(question))
            {
                error("Remove - Given command line has no argument.");
                return false;
            }

            return _retorts.ContainsKey(question) ? _retorts.Remove(question) : false;
        }

        /// <summary>
        /// Saves currently kept retorts to file.
        /// </summary>
        /// <returns>true means saved, false otherwise</returns>
        public bool SaveToFile()
        {
            var convertedJson = utilJson.DictionaryToJSON(_retorts);
            if (convertedJson != null)
            {
                var path = utilResource.PathToResource(_fileName);
                File.WriteAllText(path, convertedJson);
                return true;
            }
            error("SaveToFile - converted JSON of dictionary was null.");
            return false;
        }

        /// <summary>
        /// Checks, if given question exists within retorts.
        /// </summary>
        /// <param name="question">given question</param>
        /// <returns>true means exists, false otherwise</returns>
        public bool Contains(string question)
        {
            // Checked entered question
            if (string.IsNullOrWhiteSpace(question)) return false;
            question = question.Trim().ToLower();

            // Check, if key exists
            return _retorts.ContainsKey(question);
        }

        /// <summary>
        /// Responds with an answer, if there is one in retorts.
        /// </summary>
        /// <param name="question">given question</param>
        /// <returns>response, if there is one, null otherwise</returns>
        public string Respond(string question)
        {
            // Checked entered question
            if (string.IsNullOrWhiteSpace(question)) return null;
            question = question.Trim().ToLower();

            // Prepare response
            string response = null;
            // Check, if it is add retort
            if (question.StartsWith("add-retort|", System.StringComparison.Ordinal))
            {
                return Add(question) ? "Added new retort." : "Couldn't process " + question;
            }
            // Check, if retorts are loaded
            if (_retorts != null && _retorts.Count == 0) return "Respond - Retorts are empty.";
            // Check, if it is remove retort
            if (question.StartsWith("remove-retort|", System.StringComparison.Ordinal))
            {
                return Remove(question) ? "Removed the retort." : "Couldn't remove the retort.";
            }
            // Process with retorts
            if (_retorts.ContainsKey(question))
            {
                error(string.Format("Respond - working with retort: {0} and answer {1}.", question, _retorts[question]));
                return _retorts[question];
            }

            return response;
        }

        /// <summary>
        /// Count of retorts kept in the dictionary.
        /// </summary>
        /// <returns>number of rtorts in</returns>
        public int Count() => _retorts != null ? _retorts.Count : 0;

        /// <summary>
        /// Lists all the errors, which occured during the talk.
        /// </summary>
        /// <returns>list of errors, if there are some, null otherwise</returns>
        public List<string> ListErrors() => (_errors != null || _errors.Count > 0) ? _errors : null;
    }
}
