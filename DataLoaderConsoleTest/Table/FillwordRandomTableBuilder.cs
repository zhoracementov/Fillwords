using DataLoaderConsoleTest.Data;
using System;

namespace DataLoaderConsoleTest.Table
{
    internal abstract class FillwordTableBuilder
    {
        protected readonly Difficulty difficulty;

        protected readonly int size;
        protected readonly int min;
        protected readonly int max;

        protected Node<Point>[,] table;

        public FillwordTableBuilder(Difficulty difficulty, int size, int min, int max)
        {
            this.size = size;
            this.difficulty = difficulty;
            this.min = Math.Min(min, size * size);
            this.max = Math.Min(max - 1, size * size);
            table = new Node<Point>[size, size];
        }

        public abstract FillwordTable Build();
    }
}
