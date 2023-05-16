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
    public class GameProcessService
    {
        private readonly LinkedList<FillwordItem> selectedList;
        private readonly BrushesNamesLoopQueue brushesNamesLoopQueue;

        private string defaultColor;
        private string selectionColor;

        private bool isEnter;
        private bool isGameActive;

        public bool IsGameActive => isGameActive;

        private string[,] colorsMap;
        public string[,] ColorsMap
        {
            get => colorsMap;
            set => colorsMap = value;
        }

        public string DefaultColor
        {
            get => defaultColor;
            set => defaultColor = value;
        }

        public string SelectionColor
        {
            get => selectionColor;
            set => selectionColor = value;
        }

        public event Action GameStarted;
        public event Action GameProgressChanged;
        public event Action GameStoped;
        public event Action GameSelectionFailed;

        public GameProcessService(BrushesNamesLoopQueue brushQueue)
        {
            selectedList = new LinkedList<FillwordItem>();
            this.brushesNamesLoopQueue = brushQueue;
            this.DefaultColor = brushQueue.StartString;
            this.SelectionColor = brushQueue.NextString;
        }

        public GameProcessService() : this(new BrushesNamesLoopQueue())
        {

        }

        public void StartGame(bool isRestart = false)
        {
            if (!isRestart)
            {
                for (int i = 0; i < ColorsMap.GetLength(0); i++)
                {
                    for (int j = 0; j < ColorsMap.GetLength(1); j++)
                    {
                        ColorsMap[i, j] = brushesNamesLoopQueue.StartString;
                    }
                }
            }

            OnGameStarts();
            isGameActive = true;
        }

        public void StopGame(bool IsBreak = false)
        {
            if (isGameActive)
            {
                isGameActive = false;

                if (!IsBreak)
                    OnGameEnds();
            }
        }

        public bool OnStartSelecting(FillwordItem fillwordItem)
        {
            if (!isGameActive)
                return false;

            isEnter = true;

            var res = Add(fillwordItem);

            if (res && !CheckSolvedItem(fillwordItem))
                ColorsMap.SetAt(fillwordItem.Point, SelectionColor);

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
                solvedAll = CheckSolvedMap();

                OnGameProgressChanged();

                if (solvedAll)
                    StopGame();

                SelectionColor = brushesNamesLoopQueue.NextString;
            }
            else
            {
                OnGameFailedSelection();
            }

            selectedList.Clear();

            return (solvedThis, solvedAll);
        }

        public bool OnSelectNextItem(FillwordItem fillwordItem)
        {
            if (!isGameActive)
                return false;

            var res = Add(fillwordItem);

            if (res && !CheckSolvedItem(fillwordItem))
                ColorsMap.SetAt(fillwordItem.Point, SelectionColor);

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

            if (CheckSolvedItem(fillwordItem))
                return false;

            selectedList.AddLast(fillwordItem);
            return true;
        }

        public bool CheckSolvedItem(FillwordItem fillwordItem)
        {
            return CheckSolvedItem(fillwordItem.Point);
        }

        public bool CheckSolvedItem(Point point)
        {
            return ColorsMap.GetAt(point) != DefaultColor;
        }

        private bool CheckSolvedBefore()
        {
            return selectedList
                .Select(x => x.Point)
                .Any(pt => !CheckSolvedItem(pt));
        }

        private bool CheckSolvedMap()
        {
            return ColorsMap
                .AsLinear()
                .All(x => x != DefaultColor);
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
            GameStarted?.Invoke();
        }

        private void OnGameEnds()
        {
            GameStoped?.Invoke();
        }

        private void OnGameProgressChanged()
        {
            GameProgressChanged?.Invoke();
        }

        private void OnGameFailedSelection()
        {
            foreach (var result in selectedList)
            {
                ColorsMap.SetAt(result.Point, DefaultColor);
            }

            OnGameSelectionFailed();
        }

        private void OnGameSelectionFailed()
        {
            GameSelectionFailed?.Invoke();
        }

    }
}
