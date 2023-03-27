using System.Collections.Generic;

namespace DataLoaderConsoleTest.Table
{
    internal class Node<T>
    {
        public Node<T> Next { get; set; }
        public Node<T> Previous { get; set; }
        public T Value { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
