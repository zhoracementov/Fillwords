namespace DataLoaderConsoleTest.Data
{
    internal class Node<T>
    {
        public Node<T> Next { get; set; }
        public Node<T> Previous { get; set; }
        public T Value { get; set; }

        public bool IsHead => Previous == null;
        public bool IsTail => Next == null;
        public bool IsBody => !IsHead && !IsTail;

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator Node<T>(T value)
        {
            return new Node<T> { Value = value };
        }

        public static explicit operator T(Node<T> node)
        {
            return node.Value;
        }
    }
}
