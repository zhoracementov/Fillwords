using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DataLoaderConsoleTest
{
    class RusWordInfoDefinitionConverter
    {
        private readonly IDictionary<string, WordInfo> data;

        public RusWordInfoDefinitionConverter(IDictionary<string, WordInfo> data)
        {
            this.data = data;
        }

        public IDictionary<string, WordInfo> Convert()
        {
            var regexCurr = new Regex(@"(?<key>[\w]+) \((?<index>[\d]+)\*\)");
            var regexSplit = new Regex(@"\p{Lu}[^\d\.]+");
            var regexReplace = new Regex(@"\(\d\*\)");

            var indexed = data.Values.ToArray();

            for (int i = 0; i < data.Count; i++)
            {
                var item = data.ElementAt(i);
                var matches = regexCurr.Matches(item.Value.Definition).OfType<Match>();

                foreach (var match in matches)
                {
                    var groups = match.Groups;
                    var key = groups["key"].Value;
                    var index = int.Parse(groups["index"].Value);

                    Console.WriteLine("Слово: {0}", item.Key);
                    Console.WriteLine("Из какого слова брать: {0}. Определение номер: {1}", key, index);

                    if (string.IsNullOrEmpty(key))
                        continue;

                    if (!data.ContainsKey(key))
                        continue;

                    if (data[key] == null)
                        continue;

                    var valueOld = data[key];
                    var valueMatches = regexSplit
                        .Matches(valueOld.Definition)
                        .OfType<Match>()
                        .Select(x => x.Value)
                        .ToArray();

                    if (index >= valueMatches.Length)
                        continue;

                    var valueFound = valueMatches[index - 1];

                    var valueNew = valueOld.Definition.Replace($"({index}*)", valueFound);

                    data[key] = new WordInfo(valueNew, valueOld.AnswerIsProbablyNotNoun, valueOld.AnswerNeedToIncludePlural);

                    Console.WriteLine(valueFound);
                    Console.WriteLine(valueNew);

                    Console.WriteLine();
                }
            }

            return data;
        }
    }
}
