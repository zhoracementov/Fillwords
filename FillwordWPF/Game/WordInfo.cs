using System.Linq;

namespace FillwordWPF.Game
{
    internal struct WordInfo
    {
        public string Definition { get; set; }
        public string AnswerIsProbablyNotNoun { get; set; }
        public string AnswerNeedToIncludePlural { get; set; }

        //public override string ToString()
        //{
        //    return string
        //        .Join(", ", GetType().GetProperties()
        //        .Select(x => x.GetValue(this))
        //        .Where(str => str != null));
        //}
    }
}
