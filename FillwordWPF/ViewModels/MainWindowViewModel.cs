namespace FillwordWPF.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Заголовок Окна

        private string _title = "Fillwords";

        /// <summary>
        /// Заголовок главного окна
        /// </summary>
        public string Title
        {
            get => _title;
            private set => Set(ref _title, value);
        }

        #endregion

        public MainWindowViewModel()
        {
        }
    }
}
