using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositeKeyDictionray
{
    class Program
    {
        static void Main(string[] args)
        {
            CompositeKeyDictionary<int, int, string> dict = new CompositeKeyDictionary<int, int, string>();
            dict.Add(5, 5, "Hello");
            string res;
            dict.TryGetValue(5, 5, out res);
            Console.WriteLine(res);
            Console.ReadKey();
        }
    }
}
