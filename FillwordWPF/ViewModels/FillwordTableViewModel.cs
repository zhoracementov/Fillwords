using FillwordWPF.Models;

namespace FillwordWPF.ViewModels
{
    internal class FillwordTableViewModel : ViewModel
    {
        private FillwordTable table;

        public FillwordTable Table
        {
            get => table;
            private set => Set(ref table, value);
        }

        public FillwordTableViewModel(FillwordTable table)
        {
            Table = table;
        }
    }
}
