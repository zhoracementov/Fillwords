using FillwordWPF.Models;

namespace FillwordWPF.Infrastructure
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

        public abstract Fillword Build();
    }
}