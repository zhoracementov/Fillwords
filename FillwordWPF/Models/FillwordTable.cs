namespace FillwordWPF.Models
{
    internal class FillwordTable
    {
        public FillwordTableItem[,] Table { get; }
        public int Size => Table.GetLength(0);

        public FillwordTable(FillwordTableItem[,] table)
        {
            Table = table;
        }
    }
}
