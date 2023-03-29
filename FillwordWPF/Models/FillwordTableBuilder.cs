
using FillwordWPF.Infrastructure;

namespace FillwordWPF.Models
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