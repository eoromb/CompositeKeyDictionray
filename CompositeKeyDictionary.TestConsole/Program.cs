using CompositeKeyDictionary.Test;
using System;

namespace CompositeKeyDictionary.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Пример использования. 
            var dict = new CompositeKeyDictionary<Person, Address, House>();
            var ind = 1;
            var addressToSelect = new Address($"Country_{ind}", $"City{ind}", $"Street_{ind}", ind);
            Person personToSelect;

            FillDictionary(dict, out personToSelect);

            Console.WriteLine("All items:");
            foreach (var item in dict)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine();
           
            Console.WriteLine($"Selected by address: {addressToSelect}");
            foreach (var item in dict.GetValuesByKey2(addressToSelect))
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine();

            Console.WriteLine($"Selected by person: {personToSelect}");
            foreach (var item in dict.GetValuesByKey1(personToSelect))
            {
                Console.WriteLine(item.ToString());
            }

            Console.ReadLine();
        }
        private static void FillDictionary(CompositeKeyDictionary<Person, Address, House> dict, out Person personToSelect)
        {
            Person person = new Person(0, $"Name_{0}");
            personToSelect = person;
            const int cycleConst = 5;
            for (int i = 1; i <= 15; ++i)
            {
                if (i % cycleConst == 0)
                {
                    person = new Person(i / cycleConst, $"Name_{i / cycleConst}");
                }
                dict.Add(person,
                    new Address($"Country_{i % cycleConst}", $"City{i % cycleConst}", $"Street_{i % cycleConst}", i % cycleConst),
                    new House($"Color_{i}", i));
            }
        }
    }
}
