using FillwordWPF.Extenstions;
using FillwordWPF.Game;
using FillwordWPF.Models;
using FillwordWPF.Services.WritableOptions;
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
        private readonly IWritableOptions<GameSettings> settings;
        private readonly LinkedList<FillwordItem> selectedList;
        private bool isEnter;

        private bool isGameActive;
        [JsonIgnore]
        public bool IsGameActive
        {
            get => isGameActive;
            set
            {
                isGameActive = value;

                if (value)
                    OnGameStarts();
                else
                    OnGameEnds();
            }
        }

        private bool[,] solvedMap;
        public bool[,] SolvedMap
        {
            get => solvedMap;
            set => solvedMap = value;
        }

        public event Action GameStartsEvent;
        public event Action GameEndsEvent;

        public GameProcessService(IWritableOptions<GameSettings> settings)
        {
            selectedList = new LinkedList<FillwordItem>();
            this.settings = settings;

            GameStartsEvent += OnRestart;
        }

        public void OnStartSelecting(FillwordItem fillwordItem)
        {
            if (!IsGameActive)
                return;

            isEnter = true;

            Add(fillwordItem);
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
                OnSolve();
                solvedAll = CheckSolvedMap();
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

        private void OnSolve()
        {
            foreach (var result in selectedList)
            {
                var point = result.Point;
                SolvedMap[point.X, point.Y] = true;
            }
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

            var seq = Enumerable.Range(0, firstLen);

            return selectedList.All(x => x.Word == first.Word) && selectedList.Select(x => x.Index).SequenceEqual(seq);
        }

        private void OnGameStarts()
        {
            GameStartsEvent?.Invoke();
        }

        private void OnGameEnds()
        {
            GameEndsEvent?.Invoke();
        }

        private void OnRestart()
        {
            SolvedMap = new bool[settings.Value.Size, settings.Value.Size];
        }
    }
}
