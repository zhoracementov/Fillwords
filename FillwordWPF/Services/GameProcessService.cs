using FillwordWPF.Extenstions;
using FillwordWPF.Game;
using FillwordWPF.Models;
using FillwordWPF.Services.WritableOptions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FillwordWPF.Services
{
    internal class GameProcessService
    {
        private readonly LinkedList<FillwordItem> selectedList;
        private readonly BrushQueue brushQueue;

        private string defaultColor;
        private string selectionColor;

        private bool isEnter;

        private bool isGameActive;

        public void StartGame(bool isRestart = false)
        {
            if (!isRestart)
            {
                for (int i = 0; i < ColorsMap.GetLength(0); i++)
                {
                    for (int j = 0; j < ColorsMap.GetLength(1); j++)
                    {
                        ColorsMap[i, j] = brushQueue.StartString;
                    }
                }
            }

            OnGameStarts();
            isGameActive = true;
        }

        public void StopGame()
        {
            if (isGameActive)
            {
                isGameActive = false;
                OnGameEnds();
            }
        }

        private bool[,] solvedMap;
        public bool[,] SolvedMap
        {
            get => solvedMap;
            set => solvedMap = value;
        }

        private string[,] colorsMap;
        public string[,] ColorsMap
        {
            get => colorsMap;
            set => colorsMap = value;
        }

        public event Action GameStarts;
        public event Action GameProgressChanged;
        public event Action GameEnds;

        public event Action GameFailedSelection;

        public GameProcessService(BrushQueue brushQueue)
        {
            selectedList = new LinkedList<FillwordItem>();
            this.brushQueue = brushQueue;
            this.defaultColor = brushQueue.StartString;
            this.selectionColor = brushQueue.NextString;
        }

        public GameProcessService() : this(new BrushQueue())
        {

        }

        public bool OnStartSelecting(FillwordItem fillwordItem)
        {
            if (!isGameActive)
                return false;

            isEnter = true;

            var res = Add(fillwordItem);

            if (res && !SolvedMap.GetAt(fillwordItem.Point))
                colorsMap.SetAt(fillwordItem.Point, selectionColor);

            return res;
        }

        public (bool SolvedThis, bool SolvedAll) OnEndSelecting()
        {
            if (!isGameActive)
                return (false, false);

            isEnter = false;

            var solvedThis = CheckSolvedWord() && !CheckSolvedBefore();
            var solvedAll = false;

            if (solvedThis)
            {
                foreach (var result in selectedList)
                {
                    SolvedMap.SetAt(result.Point, true);
                }

                solvedAll = CheckSolvedMap();

                if (solvedAll)
                    StopGame();
                else
                    OnGameProgressChanged();

                selectionColor = brushQueue.NextString;
            }
            else
            {
                OnGameFailedSelection();
            }

            selectedList.Clear();

            return (solvedThis, solvedAll);
        }

        private void OnGameFailedSelection()
        {
            foreach (var result in selectedList)
            {
                ColorsMap.SetAt(result.Point, defaultColor);
            }

            GameFailedSelection?.Invoke();
        }

        public bool OnSelectNextItem(FillwordItem fillwordItem)
        {
            if (!isGameActive)
                return false;

            var res = Add(fillwordItem);

            if (res && !SolvedMap.GetAt(fillwordItem.Point))
                colorsMap.SetAt(fillwordItem.Point, selectionColor);

            return res;
        }

        //public bool OnUnSelectNextItem(FillwordItem fillwordItem)
        //{
        //    if (!IsGameActive)
        //        return false;

        //    return Remove(fillwordItem);
        //}

        //private bool Remove(FillwordItem fillwordItem)
        //{
        //    if (!isEnter)
        //        return false;

        //    return selectedList.Remove(fillwordItem);
        //}

        private bool Add(FillwordItem fillwordItem)
        {
            if (!isEnter)
                return false;

            if (selectedList.Count > 0 && selectedList.Last.Value == fillwordItem)
                return false;

            if (selectedList.Count > 0 && selectedList.Last.Value.Point.GetDistance(fillwordItem.Point) > 1)
                return false;

            if (SolvedMap.GetAt(fillwordItem.Point))
                return false;

            selectedList.AddLast(fillwordItem);
            return true;
        }

        private bool CheckSolvedBefore()
        {
            return selectedList
                .Select(x => x.Point)
                .Any(pt => SolvedMap[pt.X, pt.Y]);
        }

        private bool CheckSolvedMap()
        {
            return SolvedMap.AsLinear().All(x => x);
        }

        private bool CheckSolvedWord()
        {
            if (selectedList.Count == 0)
                return false;

            var first = selectedList.First.Value;
            var firstLen = first.Word.Length;

            if (selectedList.Count != firstLen)
                return false;

            return selectedList
                .All(x => x.Word == first.Word)
                && selectedList
                .Select(x => x.Index)
                .SequenceEqual(Enumerable.Range(0, firstLen));
        }

        private void OnGameStarts()
        {
            GameStarts?.Invoke();
        }

        private void OnGameEnds()
        {
            GameEnds?.Invoke();
        }

        private void OnGameProgressChanged()
        {
            GameProgressChanged?.Invoke();
        }
    }
}
