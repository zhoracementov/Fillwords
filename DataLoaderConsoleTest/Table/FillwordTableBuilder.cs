using DataLoaderConsoleTest.Data;
using System.Collections.Generic;

namespace DataLoaderConsoleTest.Table
{
    internal abstract class FillwordTableBuilder
    {
        protected readonly WordsData words;
        protected readonly Difficulty difficulty;

        public FillwordTableBuilder(WordsData words, Difficulty difficulty)
        {
            this.words = words;
            this.difficulty = difficulty;
        }

        public abstract FillwordTable Build();
    }
}