using System;

namespace DataLoaderConsoleTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IObjectSerializer jsonObjectSerializer = new JsonObjectSerializer();
            Console.WriteLine(jsonObjectSerializer.GetType().FullName);
            Console.ReadKey();
        }
    }
}
