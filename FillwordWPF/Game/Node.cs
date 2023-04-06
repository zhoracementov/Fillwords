using System.Collections.Generic;
using System.Linq;

namespace FillwordWPF.Game
{
    internal class Node<T>
    {
        public Node<T> Next { get; set; }
        public Node<T> Previous { get; set; }
        public T Value { get; set; }

        public bool IsHead => Previous == null;
        public bool IsTail => Next == null;
        public bool IsBody => !IsHead && !IsTail;

        public IEnumerable<Node<T>> TraceAll()
        {
            var curr = this;

            while (curr.Previous != null)
            {
                curr = curr.Previous;
            }

            while (curr != null)
            {
                yield return curr;
                curr = curr.Next;
            }
        }

        public IEnumerable<T> TraceAllValues()
        {
            return TraceAll().Select(x => x.Value);
        }

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
