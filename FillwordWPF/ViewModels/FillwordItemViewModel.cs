using FillwordWPF.Commands;
using FillwordWPF.Extenstions;
using FillwordWPF.Models;
using FillwordWPF.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace FillwordWPF.ViewModels
{
    internal class FillwordItemViewModel : ViewModel
    {
        private readonly GameProcessService gameProcessService;


        private string currentColor;
        public string CurrentColor
        {
            get => currentColor;
            set => Set(ref currentColor, value);
        }

        private FillwordItem fillwordItem;
        public FillwordItem FillwordItem
        {
            get => fillwordItem;
            set
            {
                if (Set(ref fillwordItem, value))
                    CurrentColor = gameProcessService.ColorsMap.GetAt(value.Point);
            }
        }

        public ICommand SelectNextItemCommand { get; }
        public ICommand StartSelectCommand { get; }
        public ICommand EndSelectCommand { get; }

        public FillwordItemViewModel(GameProcessService gameProcessService, BrushQueue brushQueue)
        {
            SelectNextItemCommand = new RelayCommand(OnSelectNextItem);
            StartSelectCommand = new RelayCommand(OnStartSelectCommand);
            EndSelectCommand = new RelayCommand(OnEndSelectCommand);

            this.gameProcessService = gameProcessService;


            gameProcessService.GameFailedSelection += SetColor;
        }

        private void SetColor()
        {
            CurrentColor = gameProcessService.ColorsMap.GetAt(FillwordItem.Point);
        }

        public void OnSelectNextItem(object parameter)
        {
            var res = gameProcessService.OnSelectNextItem(FillwordItem);
            SetColor();
        }

        public void OnEndSelectCommand(object parameter)
        {
            var res = gameProcessService.OnEndSelecting();

            if (res.SolvedThis)
            {
                MessageBox.Show($"{FillwordItem.Word}{Environment.NewLine}{FillwordItem.Info.Definition}");
            }
        }

        public void OnStartSelectCommand(object parameter)
        {
            var res = gameProcessService.OnStartSelecting(FillwordItem);
            SetColor();
        }
    }
}
