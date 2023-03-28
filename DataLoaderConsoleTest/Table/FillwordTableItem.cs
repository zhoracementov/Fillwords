using DataLoaderConsoleTest.Data;

namespace DataLoaderConsoleTest.Table
{
    internal class FillwordTableItem
    {
        public char? CurrentLetter { get; set; }
        public Point Point { get; set; }
        public string Word { get; set; }
        public WordInfo Info { get; set; }
    }
}
