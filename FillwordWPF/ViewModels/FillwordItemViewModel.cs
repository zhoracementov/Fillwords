using FillwordWPF.Commands;
using FillwordWPF.Extenstions;
using FillwordWPF.Models;
using FillwordWPF.Services;
using System;
using System.Collections.Generic;
using System.Text;
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
            set => Set(ref fillwordItem, value);
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
            this.currentColor = brushQueue.NextString;
        }

        public void OnSelectNextItem(object parameter)
        {
            gameProcessService.OnSelectNextItem(FillwordItem);
        }

        public void OnEndSelectCommand(object parameter)
        {
            gameProcessService.OnEndSelecting();
        }

        public void OnStartSelectCommand(object parameter)
        {
            gameProcessService.OnStartSelecting(FillwordItem);
        }
    }
}
