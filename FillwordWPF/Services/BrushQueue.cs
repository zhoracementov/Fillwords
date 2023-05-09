using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace FillwordWPF.Services
{
    internal class BrushQueue
    {
        private readonly Queue<string> colorsQueue;
        private readonly Dictionary<string, Brush> createdBefore;

        public string DefaultString { get; }
        public Brush Default { get; }

        public string NextString => DequeueEnqueue();
        public Brush Next
        {
            get
            {
                var next = DequeueEnqueue();

                if (createdBefore.TryGetValue(next, out var value))
                {
                    return value;
                }
                else
                {
                    var brush = GetBrush(next);
                    createdBefore[next] = brush;
                    return brush;
                }
            }
        }

        public BrushQueue()
        {
            colorsQueue = new Queue<string>(GetBasicColors());
            createdBefore = new Dictionary<string, Brush>();

            DefaultString = DequeueEnqueue();
            Default = GetBrush(DefaultString);
        }

        private string DequeueEnqueue()
        {
            var next = colorsQueue.Dequeue();
            colorsQueue.Enqueue(next);
            return next;
        }

        private static Brush GetBrush(string name)
        {
            return (Brush)typeof(Brushes).GetProperty(name).GetValue(null);
        }

        private static IEnumerable<string> GetBasicColors()
        {
            var rnd = new Random(Environment.TickCount);

            return typeof(Brushes)
                .GetProperties()
                .Select(x => x.Name)
                .OrderBy(x => rnd.Next());
        }
    }
}
