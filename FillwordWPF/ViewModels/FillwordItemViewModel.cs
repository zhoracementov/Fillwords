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
        private readonly BrushQueue brushQueue;

        private static string selectionColor;
        private static string defaultColor;
        private static List<FillwordItemViewModel> selectionList;

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
            this.brushQueue = brushQueue;

            defaultColor = brushQueue.StartString;
            selectionColor = brushQueue.NextString;
            selectionList = new List<FillwordItemViewModel>();

            this.currentColor = defaultColor;

        }

        public void OnSelectNextItem(object parameter)
        {
            var res = gameProcessService.OnSelectNextItem(FillwordItem);

            if (res && CurrentColor == defaultColor)
            {
                if (!selectionList.Contains(this))
                    selectionList.Add(this);

                CurrentColor = selectionColor;
            }
        }

        public void OnEndSelectCommand(object parameter)
        {
            var res = gameProcessService.OnEndSelecting();

            if (res.SolvedThis)
            {
                selectionColor = brushQueue.NextString;
            }
            else
            {
                foreach (var item in selectionList)
                {
                    item.CurrentColor = defaultColor;
                }
            }

            selectionList.Clear();
        }

        public void OnStartSelectCommand(object parameter)
        {
            var res = gameProcessService.OnStartSelecting(FillwordItem);
            
            if (res && CurrentColor == defaultColor)
            {
                if (!selectionList.Contains(this))
                    selectionList.Add(this);
                
                CurrentColor = selectionColor;
            }
        }
    }
}
