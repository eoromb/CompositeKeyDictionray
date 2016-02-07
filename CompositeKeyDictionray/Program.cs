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
            CompositeKeyDictionaryExt<int, int, string> dict = new CompositeKeyDictionaryExt<int, int, string>();
            dict[5, 5] = "Hello";
            
            Console.WriteLine(res);
            Console.ReadKey();
        }
    }
}
