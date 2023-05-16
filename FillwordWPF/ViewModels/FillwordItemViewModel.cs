using FillwordWPF.Commands;
using FillwordWPF.Extenstions;
using FillwordWPF.Models;
using FillwordWPF.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace FillwordWPF.ViewModels
{
    internal class FillwordItemViewModel : ViewModel
    {
        private readonly GameProcessService gameProcessService;


        private string backgroungCurrentColor = "Gray";
        public string BackgroungCurrentColor
        {
            get => backgroungCurrentColor;
            set => Set(ref backgroungCurrentColor, value);
        }

        private int margin;
        public int Margin
        {
            get => margin;
            set => Set(ref margin, value);
        }

        private FillwordItem fillwordItem;
        public FillwordItem FillwordItem
        {
            get => fillwordItem;
            set
            {
                if (Set(ref fillwordItem, value))
                    SetColor(value);
            }
        }

        public ICommand SelectNextItemCommand { get; }
        public ICommand StartSelectCommand { get; }
        public ICommand EndSelectCommand { get; }

        public FillwordItemViewModel(GameProcessService gameProcessService, BrushesNamesLoopQueue brushesNamesLoopQueue)
        {
            SelectNextItemCommand = new RelayCommand(OnSelectNextItem);
            StartSelectCommand = new RelayCommand(OnStartSelectCommand);
            EndSelectCommand = new RelayCommand(OnEndSelectCommand);

            this.gameProcessService = gameProcessService;

            gameProcessService.GameSelectionFailed += SetColor;
            //gameProcessService.GameProgressChanged += OnSuccessfullySelected;
        }

        private void SetColor()
        {
            SetColor(FillwordItem);
        }

        private void SetColor(FillwordItem fillwordItem)
        {
            BackgroungCurrentColor = gameProcessService.ColorsMap.GetAt(fillwordItem.Point) ?? backgroungCurrentColor;
        }


        public void OnSelectNextItem(object parameter)
        {
            var res = gameProcessService.OnSelectNextItem(FillwordItem);

            if (res)
                OnSuccessfullySelected();

            SetColor();
        }

        public void OnEndSelectCommand(object parameter)
        {
            var res = gameProcessService.OnEndSelecting();
        }

        public void OnStartSelectCommand(object parameter)
        {
            var res = gameProcessService.OnStartSelecting(FillwordItem);

            SetColor();

            if (!res && gameProcessService.CheckSolvedItem(FillwordItem) && gameProcessService.IsGameActive)
            {
                MessageBox.Show($"{FillwordItem.Word}{Environment.NewLine}{FillwordItem.Info.Definition}");
                OnEndSelectCommand(parameter);
            }
        }

        private async void OnSuccessfullySelected()
        {
            const int offset = 5;
            const int delay = 75;

            var defValue = Margin;

            Margin += offset;

            await Task.Delay(delay);

            Margin = defValue;
        }
    }
}
