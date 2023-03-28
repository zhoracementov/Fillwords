using DataLoaderConsoleTest.Data;

namespace DataLoaderConsoleTest.Table
{
    internal class FillwordTable
    {
        private readonly FillwordTableItem[,] table;

        public FillwordTableItem this[Point point]
        {
            get => this[point.X, point.Y];
            set => this[point.X, point.Y] = value;
        }

        public FillwordTableItem this[int x, int y]
        {
            get => table[x, y];
            set => table[x, y] = value;
        }

        public int Size => table.GetLength(0);

        public FillwordTable(FillwordTableItem[,] table)
        {
            this.table = table;
        }
    }
}
