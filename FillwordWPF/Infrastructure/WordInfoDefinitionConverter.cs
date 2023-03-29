using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FillwordWPF.Infrastructure
{
    internal class WordInfoDefinitionConverter
    {
        private readonly WordsData data;

        public ICollection<string> Errors { get; }

        public WordInfoDefinitionConverter(WordsData data)
        {
            this.data = data;

            Errors = new List<string>();
        }

        public WordsData Convert()
        {
            var regexCurrDefinition = new Regex(@"(?<key>[\w]+) \((?<index>[\d]+)\*\)");
            var regexSplitLinkedDefinition = new Regex(@"\p{Lu}[^\d\.]+");

            for (int i = 0; i < data.Count; i++)
            {
                var item = data.ElementAt(i);

                var matches = regexCurrDefinition
                    .Matches(item.Value.Definition)
                    .OfType<Match>();

                foreach (var match in matches)
                {
                    var groups = match.Groups;
                    var keyLinked = groups["key"].Value;
                    var indexInSplit = int.Parse(groups["index"].Value);

                    //Console.WriteLine("Слово: {0}", item.Key);
                    //Console.WriteLine("Из какого слова брать: {0}. Определение номер: {1}", keyLinked, indexInSplit);

                    if (string.IsNullOrEmpty(keyLinked))
                        continue;

                    if (!data.ContainsKey(keyLinked))
                    {
                        Errors.Add(keyLinked);
                        continue;
                    }

                    if (data[keyLinked] == null)
                    {
                        Errors.Add(keyLinked);
                        continue;
                    }

                    var valueOld = data[keyLinked];
                    var valueMatches = regexSplitLinkedDefinition
                        .Matches(valueOld.Definition)
                        .OfType<Match>()
                        .ToArray();

                    if (indexInSplit >= valueMatches.Length)
                    {
                        Errors.Add(keyLinked);
                        continue;
                    }

                    var valueFound = valueMatches[indexInSplit - 1].Value;
                    var valueNew = valueOld.Definition.Replace($"({indexInSplit}*)", valueFound);

                    data[keyLinked] = new WordInfo(valueNew, valueOld.AnswerIsProbablyNotNoun, valueOld.AnswerNeedToIncludePlural);

                    //Console.WriteLine(valueFound);
                    //Console.WriteLine(valueNew);
                    //Console.WriteLine();
                }
            }

            return data;
        }
    }
}
