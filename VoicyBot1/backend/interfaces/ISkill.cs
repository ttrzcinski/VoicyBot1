namespace VoicyBot1.backend.interfaces
{
    /// <summary>
    /// Represents a skill, which is a utility class, which must be able to resond to a quesry, phrase or command.
    /// </summary>
    interface ISkill
    {
        /// <summary>
        /// Responds with an answer to given question.
        /// </summary>
        /// <param name="question">given question</param>
        /// <returns>response, if there is one, null otherwise</returns>
        string Respond(string question);
    }
}
