using FillwordWPF.Models;

namespace FillwordWPF.Game
{
    internal interface IFillwordTableBuilder
    {
        public FillwordItem[,] Build();
    }
}