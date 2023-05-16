using FillwordWPF.Game;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FillwordWPF.Models
{
    public class FillwordItem
    {
        public int Index { get; }
        public WordInfo Info { get; }
        public Point Point { get; }
        public string Word { get; }

        public FillwordItem(int index, WordInfo info, Point point, string word)
        {
            Index = index;
            Info = info;
            Point = point;
            Word = word;
        }

        protected FillwordItem()
        {
            //...
        }

        public static bool operator ==(FillwordItem l, FillwordItem r)
        {
            return l.Equals(r);
        }

        public static bool operator !=(FillwordItem l, FillwordItem r)
        {
            return !(l == r);
        }

        public override bool Equals(object obj)
        {
            return obj is FillwordItem item &&
                   Index == item.Index &&
                   EqualityComparer<Point>.Default.Equals(Point, item.Point) &&
                   Word == item.Word &&
                   EqualityComparer<WordInfo?>.Default.Equals(Info, item.Info);
        }

        public override string ToString()
        {
            return Word[Index].ToString();
        }
    }
}
