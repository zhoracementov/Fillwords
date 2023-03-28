using DataLoaderConsoleTest.Data;
using System.Collections.Generic;

namespace DataLoaderConsoleTest.Table
{
    internal abstract class FillwordTableBuilder
    {
        protected readonly int size;
        protected readonly IDictionary<string, WordInfo> words;
        protected readonly Difficulty difficulty;

        public FillwordTableBuilder(IDictionary<string, WordInfo> words, int size, Difficulty difficulty)
        {
            this.words = words;
            this.difficulty = difficulty;
            this.size = size;
        }

        public abstract FillwordTable Build();
    }
}