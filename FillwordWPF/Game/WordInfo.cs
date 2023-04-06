using System.Linq;

namespace FillwordWPF.Game
{
    internal class WordInfo
    {
        public string Definition { get; }
        public string AnswerIsProbablyNotNoun { get; }
        public string AnswerNeedToIncludePlural { get; }

        public WordInfo(string definition, string answerIsProbablyNotNoun = null, string answerNeedToIncludePlural = null)
        {
            Definition = definition;
            AnswerNeedToIncludePlural = answerNeedToIncludePlural;
            AnswerIsProbablyNotNoun = answerIsProbablyNotNoun;
        }

        public override string ToString()
        {
            return string
                .Join(", ", GetType().GetProperties()
                .Select(x => x.GetValue(this))
                .Where(str => str != null));
        }
    }
}
