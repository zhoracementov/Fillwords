using DataLoaderConsoleTest.Data;
using System;
using System.Collections.Generic;

namespace DataLoaderConsoleTest.Table
{
    class FilwordTableBuilder
    {
        private readonly IDictionary<string, WordInfo> data;

        public FilwordTableBuilder(IDictionary<string, WordInfo> data)
        {
            this.data = data;
        }

        public FilwordTable CreateRandomly(int size)
        {
            throw new NotImplementedException();
        }

        public FilwordTable Create(int size, params string[] keys)
        {
            throw new NotImplementedException();
        }
    }
}
