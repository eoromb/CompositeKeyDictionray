using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositeKeyDictionary.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
          
            var dict = new CompositeKeyDictionary<int, int, string>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; ++j)
                {
                    dict[i, j] = $"TestString_{i}_{j}";
                }
            }
            foreach (var item in dict)
            {
                Console.WriteLine($"key1 = {item.Item1}, key2 = {item.Item2}, value = {item.Item3}");
            }
            //for (int i = 0; i < 5; i++)
            //{
            //    foreach (var item in dict.GetValuesByKey1(i))
            //    {
            //        Console.WriteLine(item);
            //    }
            //}
            Console.ReadLine();
        }
    }
}
