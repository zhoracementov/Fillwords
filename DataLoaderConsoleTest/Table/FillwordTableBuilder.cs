using DataLoaderConsoleTest.Data;
using System.Collections.Generic;

namespace DataLoaderConsoleTest.Table
{
    internal abstract class FillwordTableBuilder
    {
        protected readonly IDictionary<string, WordInfo> words;
        protected readonly Difficulty difficulty;

        public FillwordTableBuilder(IDictionary<string, WordInfo> words, Difficulty difficulty)
        {
            this.words = words;
            this.difficulty = difficulty;
        }

        public abstract FillwordTable Build();
    }
}