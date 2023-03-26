using DataLoaderConsoleTest.Data;
using System;
using System.Collections.Generic;

namespace DataLoaderConsoleTest.Table
{
    internal class FillwordTableBuilder
    {
        private readonly IDictionary<string, WordInfo> data;

        public FillwordTableBuilder(IDictionary<string, WordInfo> data)
        {
            this.data = data;
        }

        public FillwordTable CreateRandomly(int size, FillwordDifficulty difficulty, FillwordWordsDirection direction)
        {
            var table = new Point[size, size];
            var bufferCharsSize = size * size;
            var keysList = new List<string>();

            while (bufferCharsSize >= 0)
            {
                var placement = data.PickRandom(1, x => x.Key.Length == (int)difficulty);
                foreach (var item in placement)
                {
                }
            }


            throw new NotImplementedException();
        }

        public FillwordTable Create(int size, FillwordWordsDirection direction, params string[] keys)
        {
            throw new NotImplementedException();
        }
    }
}
