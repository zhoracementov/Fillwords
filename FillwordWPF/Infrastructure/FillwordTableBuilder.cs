﻿using FillwordWPF.Models;

namespace FillwordWPF.Infrastructure
{
    internal abstract class FillwordTableBuilder
    {
        public const int MinWordLength = 2;

        protected readonly WordsData words;
        protected readonly Difficulty difficulty;
        protected readonly int minWordLength;

        public FillwordTableBuilder(WordsData words, GameSettings gameSettings)
        {
            this.words = words;
            this.difficulty = gameSettings.Difficulty;
            this.minWordLength = gameSettings.MinWordLength;
        }

        public abstract Fillword Build();
    }
}