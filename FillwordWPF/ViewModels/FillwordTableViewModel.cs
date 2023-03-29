using FillwordWPF.Models;

namespace FillwordWPF.ViewModels
{
    internal class FillwordTableViewModel : ViewModel
    {
        public FillwordTable Table { get; set; }

        public FillwordTableViewModel(FillwordTable table)
        {
            Table = table;
        }
    }
}
