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

        public bool IsEnter { get; set; }

        private bool isGameActive;
        public bool IsGameActive
        {
            get => isGameActive;
            set
            {
                if (value)
                {
                    isGameActive = value;
                    OnRestart();
                }
                else
                {
                    throw new NotImplementedException();
                }
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

        public bool OnEndSelecting()
        {
            if (!IsGameActive)
                return false;

            IsEnter = false;

            var ans = CheckSolvedWord() && !CheckSolvedBefore();

            if (ans)
            {
                OnSolve();
                return CheckSolvedMap();
            }

            selectedList.Clear();

            return ans;
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

        private bool CheckSolvedBefore()
        {
            return selectedList.Select(x => x.Point).Any(pt => solvedMap[pt.X, pt.Y]);
        }

        private void OnSolve()
        {
            foreach (var result in selectedList)
            {
                var point = result.Point;
                solvedMap[point.X, point.Y] = true;
            }
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

            var seq = Enumerable.Range(0, firstLen);

            return selectedList.All(x => x.Word == first.Word) && selectedList.Select(x => x.Index).SequenceEqual(seq);
        }

        private void OnRestart()
        {
            solvedMap = new bool[settings.Value.Size, settings.Value.Size];
        }
    }
}
