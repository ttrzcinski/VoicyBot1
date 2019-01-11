using VoicyBot1.backend;

namespace VoicyBot1.model
{
    public class QuestionsAboutTime
    {
        /// <summary>
        /// Responds with an answer, if it is about time.
        /// </summary>
        /// <param name="question">given question</param>
        /// <returns>response, if there is one, null otherwise</returns>
        public string Respond(string question)
        {
            if (string.IsNullOrWhiteSpace(question)) return null;
            question = question.Trim().ToLower();
            if (question.EndsWith("?", System.StringComparison.Ordinal)) question = question.Substring(0, question.Length - 1).TrimEnd();

            string result = null;
            if (question.Equals("now") ||  question.Equals("time now") || 
                question.Equals("what's the time now") || question.Equals("what is the time now"))
            {
                result = UtilTime.Now;
            }
            else if (question.Equals("today") || question.Equals("today is") || question.Equals("today is date") || 
                     question.Equals("what's todays date") || question.Equals("what is the date today"))
            {
                result = UtilTime.Today;
            }

            return result;
        }
    }
}
