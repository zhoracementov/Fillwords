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

        public event EventHandler SuccessfullySolved;
        public List<FillwordItem> SelectedList { get; set; }
        public bool[,] SolvedMap { get; set; }

        public GameProcessService(IWritableOptions<GameSettings> settings)
        {
            SelectedList = new List<FillwordItem>();
            this.settings = settings;
            SolvedMap = new bool[settings.Value.Size, settings.Value.Size];
        }

        public bool Add(FillwordItem fillwordItem)
        {
            SelectedList.Add(fillwordItem);

            var res = IsSolvedWord();

            if (res)
            {
                foreach (var result in SelectedList)
                {
                    var point = result.Point;
                    SolvedMap[point.X, point.Y] = true;
                }

                SelectedList.Clear();

                SuccessfullySolved?.Invoke(null, new EventArgs());
            }
            return res;
        }

        public bool CheckSolvedMap()
        {
            return SolvedMap.All(x => x);
        }

        public bool IsSolvedWord()
        {
            var first = SelectedList.First();
            var firstLen = first.Word.Length;

            if (SelectedList.Count != firstLen)
                return false;

            for (int i = 1; i < firstLen; i++)
            {
                if (SelectedList[i].Word != first.Word)
                    return false;
            }

            return true;

        }
    }
}
