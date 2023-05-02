using FillwordWPF.Extenstions;
using FillwordWPF.Game;
using FillwordWPF.Models;
using FillwordWPF.Services.WriteableOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FillwordWPF.Services
{
    internal class GameProcessService
    {
        private readonly IWritableOptions<GameSettings> settings;
        private readonly LinkedList<FillwordItem> selectedList;
        private bool[,] solvedMap;
        private bool isGameActive;

        public bool IsEnter { get; set; }
        public bool IsGameActive
        {
            get => isGameActive;
            set
            {
                isGameActive = value;
                OnRestart();
            }
        }

        public GameProcessService(IWritableOptions<GameSettings> settings)
        {
            selectedList = new LinkedList<FillwordItem>();
            this.settings = settings;
        }

        public void OnStartSelecting(FillwordItem fillwordItem)
        {
            if (!IsGameActive)
                return;

            IsEnter = true;

            Add(fillwordItem);
        }

        public void OnEndSelecting()
        {
            if (!IsGameActive)
                return;

            IsEnter = false;

            if (CheckSolvedWord())
            {
                OnSolve();
                var win = CheckSolvedMap();

                if (win)
                {
                    //...
                    var b = 1;
                }
            }
        }

        public void OnSelectNextItem(FillwordItem fillwordItem)
        {
            if (!IsGameActive)
                return;

            Add(fillwordItem);
        }

        private void Add(FillwordItem fillwordItem)
        {
            if (!IsEnter)
                return;

            if (selectedList.Count > 0 && selectedList.Last.Value == fillwordItem)
                return;

            selectedList.AddLast(fillwordItem);
        }

        private void OnSolve()
        {
            foreach (var result in selectedList)
            {
                var point = result.Point;
                solvedMap[point.X, point.Y] = true;
            }

            selectedList.Clear();
        }

        private bool CheckSolvedMap()
        {
            return solvedMap.AsLinear().All(x => x);
        }

        private bool CheckSolvedWord()
        {
            var first = selectedList.First.Value;
            var firstLen = first.Word.Length;

            if (selectedList.Count != firstLen)
                return false;

            return selectedList.All(x => x.Word == first.Word);
        }

        private void OnRestart()
        {
            solvedMap = new bool[settings.Value.Size, settings.Value.Size];
        }
    }
}
