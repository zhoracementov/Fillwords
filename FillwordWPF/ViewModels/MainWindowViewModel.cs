namespace FillwordWPF.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Заголовок Окна

        private string title = "Fillwords";

        /// <summary>
        /// Заголовок главного окна
        /// </summary>
        public string Title
        {
            get => title;
            private set => Set(ref title, value);
        }

        #endregion

        public MainWindowViewModel()
        {
        }
    }
}
