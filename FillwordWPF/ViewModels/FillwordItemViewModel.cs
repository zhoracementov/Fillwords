using FillwordWPF.Models;

namespace FillwordWPF.ViewModels
{
    internal class FillwordItemViewModel : ViewModel
    {
        private bool isClicked;
        public bool IsClicked
        {
            get => isClicked;
            set => Set(ref isClicked, value);
        }

        public FillwordItem FillwordTableItem { get; }

        public FillwordItemViewModel(FillwordItem fillwordTableItem)
        {
            FillwordTableItem = fillwordTableItem;
        }
    }
}
