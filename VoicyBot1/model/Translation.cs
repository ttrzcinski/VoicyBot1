using System;
using System.Net;
using VoicyBot1.backend.interfaces;

namespace VoicyBot1.model
{
    public class Translation : ISkill
    {
        public string Respond(string question)
        {
            if (string.IsNullOrWhiteSpace(question)) return null;
            question = question.Trim().ToLower();
            // TODO ADD CHECK OF LENGTH of question
            if (!question.StartsWith("translate|", StringComparison.Ordinal)) return null;
            question = question.Substring("translate|".Length);

            string result = null;

            string languagePair = "en|de";

            try
            {
                string url = String.Format("http://www.google.com/translate_t?hl=en&ie=UTF8&text={0}&langpair={1}", question, languagePair);
                var webClient = new WebClient();
                webClient.Encoding = System.Text.Encoding.UTF8;
                var response = webClient.DownloadString(url);
                if (response.Contains("<span title=\"") && response.Contains("</span>"))
                {
                    response = response.Substring(response.IndexOf("<span title=\"", StringComparison.Ordinal) +
                                              "<span title=\"".Length);
                    response = response.Substring(response.IndexOf(">", StringComparison.Ordinal) + 1);
                    response = response.Substring(0, response.IndexOf("</span>", StringComparison.Ordinal));
                    result = response.Trim();
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("Couldn't translate" + exc);
                result = null;
            }

            return result;
        }

        public string Translate(string command, string coreLang = "auto")
        {
            if (String.IsNullOrWhiteSpace(command)) return null;
            // Check, if it is a command
            string keyword = "translate|";
            if (!command.Trim().ToLower().StartsWith(keyword, StringComparison.Ordinal)) return null;
            // Cut the 1st word and it must have content
            string line = command.Substring(keyword.Length).Trim();
            if (String.IsNullOrWhiteSpace(line)) return null;
            // Call the translate
            //"ttp://www.google.com/translate_t?hl=en&ie=UTF8&text={0}&langpair={1}"
            //"https://translate.google.com/?hl=en&ie=UTF8&text=%7B0%7D#view=home&op=translate&sl=auto&tl=en&text=wunderbar"
            var transalteUrl =
                "https://translate.google.com/?hl=en&ie=UTF8&text=%7B0%7D#view=home&op=translate&sl={0}&tl={1}&text={2}";
            string url = String.Format(transalteUrl, coreLang, "en", line);
            WebClient webClient = new WebClient();
            webClient.Encoding = System.Text.Encoding.UTF8;
            string result = webClient.DownloadString(url);
            string startTag = "<span title class>";
            if (result.Contains(startTag))
            {
                result = result.Substring(result.IndexOf(startTag));
                result = result.Substring(0, result.IndexOf("</span>"));
            }

            return String.IsNullOrWhiteSpace(result) ? result : null;
        }
    }
}
