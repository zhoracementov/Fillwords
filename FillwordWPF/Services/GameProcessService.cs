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
        private bool isEnter;

        private bool isGameActive;
        [JsonIgnore]
        public bool IsGameActive
        {
            get => isGameActive;
            set
            {
                if (isGameActive != value)
                {
                    if (value)
                        OnGameStarts();
                    else
                        OnGameEnds();
                }
                isGameActive = value;
            }
        }

        private bool[,] solvedMap;
        public bool[,] SolvedMap
        {
            get => solvedMap;
            set => solvedMap = value;
        }

        public event Action GameStartsEvent;
        public event Action GameProgressChangedEvent;
        public event Action GameEndsEvent;

        public GameProcessService()
        {
            selectedList = new LinkedList<FillwordItem>();
        }

        public bool OnStartSelecting(FillwordItem fillwordItem)
        {
            if (!IsGameActive)
                return false;

            isEnter = true;

            return Add(fillwordItem);
        }

        public (bool SolvedThis, bool SolvedAll, IList<Point> Points) OnEndSelecting()
        {
            if (!IsGameActive)
                return (false, false, null);

            isEnter = false;

            var solvedThis = CheckSolvedWord() && !CheckSolvedBefore();
            var solvedAll = false;

            if (solvedThis)
            {
                foreach (var result in selectedList)
                {
                    var point = result.Point;
                    SolvedMap[point.X, point.Y] = true;
                }

                solvedAll = CheckSolvedMap();

                if (solvedAll)
                    IsGameActive = false;
                else
                    OnGameProgressChanged();
            }

            var list = selectedList.Select(x => x.Point).ToList();

            selectedList.Clear();

            return (solvedThis, solvedAll, list);
        }

        public bool OnSelectNextItem(FillwordItem fillwordItem)
        {
            if (!IsGameActive)
                return false;

            return Add(fillwordItem);
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
            GameStartsEvent?.Invoke();
        }

        private void OnGameEnds()
        {
            GameEndsEvent?.Invoke();
        }

        private void OnGameProgressChanged()
        {
            GameProgressChangedEvent?.Invoke();
        }
    }
}
