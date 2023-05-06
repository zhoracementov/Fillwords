using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FillwordWPF.Services
{
    internal class ColorQueue
    {
        private Queue<string> colorsQueue;

        public ColorQueue()
        {
            colorsQueue = new Queue<string>(GetBasicColors());
        }

        public Color Get()
        {
            var next = colorsQueue.Dequeue();
            colorsQueue.Enqueue(next);

            return Color.FromName(next);
        }

        private static IEnumerable<string> GetBasicColors()
        {
            var rnd = new Random(Environment.TickCount);

            return Enum
                .GetNames(typeof(KnownColor))
                .Where(x => !Color.FromName(x).IsSystemColor)
                .OrderBy(x => rnd.Next());
        }
    }
}
