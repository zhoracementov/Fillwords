using FillwordWPF.Game;

namespace FillwordWPF.Models
{
    internal class FillwordItem
    {
        public int Index { get; set; }
        public char Letter => Word[Index];
        public Point Point { get; set; }
        public string Word { get; set; }
        public WordInfo Info { get; set; }
    }
}
