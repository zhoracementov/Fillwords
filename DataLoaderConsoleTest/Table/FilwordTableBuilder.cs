using System;
using System.Collections.Generic;

namespace DataLoaderConsoleTest
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

        public FilwordTable Create(params string[] keys)
        {
            throw new NotImplementedException();
        }
    }
}
