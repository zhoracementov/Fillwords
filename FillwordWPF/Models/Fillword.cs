namespace FillwordWPF.Models
{
    internal class Fillword
    {
        public FillwordItem[,] Table { get; }
        public int Size => Table.GetLength(0);

        public Fillword(FillwordItem[,] table)
        {
            Table = table;
        }
    }
}
