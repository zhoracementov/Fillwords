using DataLoaderConsoleTest.Data;
using System.Collections.Generic;

namespace DataLoaderConsoleTest.Table
{
    internal class FilwordTableItem
    {
        public string Word { get; set; }
        public WordInfo Info { get; set; }
        public Queue<Point> Points { get; set; }
    }
}
