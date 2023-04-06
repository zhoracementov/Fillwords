using FillwordWPF.Game;

namespace FillwordWPF.Models
{
    internal class FillwordItem
    {
        public char? CurrentLetter { get; set; }
        public Point Point { get; set; }
        public string Word { get; set; }
        public WordInfo Info { get; set; }
    }
}
