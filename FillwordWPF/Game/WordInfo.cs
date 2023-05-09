using System.Linq;

namespace FillwordWPF.Game
{
    internal class WordInfo
    {
        public WordInfo(string definition, string answerIsProbablyNotNoun, string answerNeedToIncludePlural)
        {
            Definition = definition;
            AnswerIsProbablyNotNoun = answerIsProbablyNotNoun;
            AnswerNeedToIncludePlural = answerNeedToIncludePlural;
        }

        public string Definition { get; }
        public string AnswerIsProbablyNotNoun { get; }
        public string AnswerNeedToIncludePlural { get; }

        //public override string ToString()
        //{
        //    return string
        //        .Join(", ", GetType().GetProperties()
        //        .Select(x => x.GetValue(this))
        //        .Where(str => str != null));
        //}
    }
}
