using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DataLoaderConsoleTest
{
    internal class WordsData : Dictionary<string, WordInfo>
    {
        public WordsData() : base()
        {
        }

        public WordsData(IDictionary<string, WordInfo> dictionary) : base(dictionary)
        {
        }

        public WordsData(IEnumerable<KeyValuePair<string, WordInfo>> collection) : base(collection)
        {
        }

        public WordsData(IEqualityComparer<string> comparer) : base(comparer)
        {
        }

        public WordsData(int capacity) : base(capacity)
        {
        }

        public WordsData(IDictionary<string, WordInfo> dictionary, IEqualityComparer<string> comparer) : base(dictionary, comparer)
        {
        }

        public WordsData(IEnumerable<KeyValuePair<string, WordInfo>> collection, IEqualityComparer<string> comparer) : base(collection, comparer)
        {
        }

        public WordsData(int capacity, IEqualityComparer<string> comparer) : base(capacity, comparer)
        {
        }

        protected WordsData(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
