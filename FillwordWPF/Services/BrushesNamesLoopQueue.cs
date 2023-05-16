using FillwordWPF.Extenstions;
using FillwordWPF.Services.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace FillwordWPF.Services
{
    public class BrushesNamesLoopQueue
    {
        private readonly Queue<string> colorsQueue;
        public string StartString { get; }
        public string NextString => DequeueEnqueue();

        public BrushesNamesLoopQueue()
        {
            colorsQueue = new Queue<string>(LoadBasicColors(new JsonObjectSerializer()));
            StartString = "Gray";
        }

        private string DequeueEnqueue()
        {
            var next = colorsQueue.Dequeue();
            colorsQueue.Enqueue(next);
            return next;
        }

        private static IEnumerable<string> LoadBasicColors(ObjectSerializer objectSerializer)
        {
            var filePath = App.BrushesNamesFileName;
            return objectSerializer.Deserialize<string[]>(filePath).ShakeAll();
        }

        private static IEnumerable<string> GetBasicColors()
        {
            return typeof(Brushes)
                .GetProperties()
                .Select(x => x.Name)
                .ShakeAll();
        }
    }
}
