using DataLoaderConsoleTest.Data;

namespace DataLoaderConsoleTest.Table
{
    internal class FillwordTable
    {
        private readonly Node<FillwordTableItem>[,] table;

        public Node<FillwordTableItem> this[Point point]
        {
            get => this[point.X, point.Y];
            set => this[point.X, point.Y] = value;
        }

        public Node<FillwordTableItem> this[int x, int y]
        {
            get => table[x, y];
            set => table[x, y] = value;
        }

        public FillwordTable(Node<FillwordTableItem>[,] table)
        {
            this.table = table;
        }
    }
}
