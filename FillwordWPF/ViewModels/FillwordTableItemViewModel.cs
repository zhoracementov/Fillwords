using FillwordWPF.Infrastructure;
using FillwordWPF.Models;

namespace FillwordWPF.ViewModels
{
    internal class FillwordTableItemViewModel : ViewModel
    {
        private bool isClicked;
        public bool IsClicked
        {
            get => isClicked;
            set => Set(ref isClicked, value);
        }

        public FillwordTableItem FillwordTableItem { get; }
        public Point Point => FillwordTableItem?.Point;

        public FillwordTableItemViewModel(FillwordTableItem fillwordTableItem)
        {
            FillwordTableItem = fillwordTableItem;
        }
    }
}
