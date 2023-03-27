using DataLoaderConsoleTest.Data;

namespace DataLoaderConsoleTest.Table
{
    internal class FillwordTableItem
    {
        public FillwordTable Table { get; set; }
        public char? Letter { get; set; }
        public Point Point { get; set; }
        public string Word { get; set; }
        public WordInfo Info { get; set; }
    }
}
