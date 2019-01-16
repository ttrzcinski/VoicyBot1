using System;
using VoicyBot1.backend.interfaces;

namespace VoicyBot1.model
{
    /// <summary>
    /// Represents a single 20-sided dice.
    /// </summary>>
    public class D20 : ISkill
    {
        /// <summary>
        /// Number of sides on the dice.
        /// </summary>
        private const int numberOfSides = 20;

        /// <summary>
        /// Represens a random numger generator.
        /// </summary>
        private Random random;

        /// <summary>
        /// Keeps the last rolled score.
        /// </summary>
        private int theLastScore = -1;

        /// <summary>
        /// Obtains and keeps for the future a Random Number generator.
        /// </summary>
        private void obtainARandomNumberGenerator()
        {
            if (random == null)
                random = new Random(20);
        }

        /// <summary>
        /// Rolls a dice.
        /// </summary>
        /// <returns>
        /// Score from the top side of dice.
        /// </returns>
        public int Roll()
        {
            obtainARandomNumberGenerator();
            theLastScore = random.Next(1, numberOfSides);
            return theLastScore;
        }

        /// <summary>
        /// Returns last score, -1 means, that no roll happened yet.
        /// </summary>
        /// <returns>Last core</returns>
        public int LastScore() => theLastScore;

        /// <summary>
        /// Responds to questions about last roll and commands to roll.
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public string Respond(string question)
        {
            if (string.IsNullOrWhiteSpace(question)) return null;
            question = question.Trim().ToLower();
            if (question.EndsWith("?", StringComparison.Ordinal))
                question.Substring(0, question.Length - 1).TrimEnd();

            string result = null;

            if (question.Equals("roll d20") || question.Equals("roll a dice") || question.Equals("dice roll") 
                                            || question.Equals("roll dice") || question.Equals("d20 roll"))
            {
                result = "" + Roll();
            }
            else if (question.Equals("what's my last roll") || question.Equals("what is my last roll") 
                                                            || question.Equals("my last roll") || question.Equals("last roll"))
            {
                result = (LastScore() == -1) ? "There was no roll." : "" + LastScore();
            }

            return result;
        }
    }
}
