using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace CompositeKeyDictionary.Test
{
    /// <summary>
    /// Тест для класса CompositeKeyDictionary из сборки CompositeKeyDictionary
    /// </summary>
    [TestClass]
    public class CompositeKeyDictionaryTest
    {
        #region Fields and properties
        private readonly Person _john = new Person(25, "John");
        private readonly Person _bill = new Person(33, "Bill");
        private readonly House _redHouse = new House("red", 13);
        private readonly House _blueHouse = new House("blue", 22);
        private readonly House _greenHouse = new House("green", 22);
        private readonly House _blackHouse = new House("black", 22);
        private readonly Address _firstAddr = new Address("Russia", "Novosibirsk", "Street1", 14);
        private readonly Address _secondAddr = new Address("Russia", "Novosibirsk", "Street2", 22);
        private readonly Address _thirdAddr = new Address("Russia", "Novosibirsk", "Street3", 23);
        private readonly Address _fourthAddr = new Address("Russia", "Novosibirsk", "Street4", 24);
        private CompositeKeyDictionary<Person, Address, House>  _dict = new CompositeKeyDictionary<Person, Address, House>();
        #endregion

        #region Constructor
        public CompositeKeyDictionaryTest()
        {
        } 
        #endregion

        #region Test initializers
        [TestInitialize()]
        public void MyTestInitialize()
        {
            _dict = new CompositeKeyDictionary<Person, Address, House>();
        }

        [TestCleanup()]
         public void MyTestCleanup()
        {
            _dict = null;
        }

        #endregion

        [TestMethod]
        public void Indexer_AddValue()
        {
            _dict[_john, _firstAddr] = _redHouse;

            Assert.AreEqual(_dict[_john, _firstAddr], _redHouse);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Indexer_GetNotExistingValue_KeyNotFoundException()
        {
            var house = _dict[_john, _firstAddr];
        }

        [TestMethod]
        public void Indexer_ModifyExistingKeys()
        {
            _dict[_john, _firstAddr] = _redHouse;
            _dict[_john, _firstAddr] = _blueHouse;

            Assert.AreEqual(_dict[_john, _firstAddr], _blueHouse);
        }

        [TestMethod]
        public void Count_TwoElementsAdded()
        {
            _dict[_john, _firstAddr] = _redHouse;
            _dict[_bill, _secondAddr] = _blueHouse;

            Assert.IsTrue(_dict.Count == 2);
        }

        [TestMethod]
        public void Add_AddValue()
        {
            _dict.Add(_john, _firstAddr, _redHouse);

            Assert.AreEqual(_dict[_john, _firstAddr], _redHouse);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Add_AddExistingKey_ArgumentException()
        {
            _dict.Add(_john, _firstAddr, _redHouse);
            _dict.Add(_john, _firstAddr, _blueHouse);
        }

        [TestMethod]
        public void TryGetValue_GetNotExisingElement()
        {
            House house;
            var res = _dict.TryGetValue(_john, _firstAddr, out house);

            Assert.IsFalse(res);
        }

        [TestMethod]
        public void TryGetValue_GetExistingItem()
        {
            _dict.Add(_john, _firstAddr, _redHouse);
            House house;
            var res = _dict.TryGetValue(_john, _firstAddr, out house);

            Assert.IsTrue(res);
            Assert.AreEqual(_redHouse, house);
        }

        [TestMethod]
        public void Remove_RemoveExistingElement()
        {
            _dict.Add(_john, _firstAddr, _redHouse);
            _dict.Remove(_john, _firstAddr);

            Assert.IsTrue(_dict.Count == 0);
        }

        [TestMethod]
        public void Clear_RemoveAllElements()
        {
            _dict.Add(_john, _firstAddr, _redHouse);
            _dict.Add(_john, _secondAddr, _redHouse);
            _dict.Clear();

            Assert.IsTrue(_dict.Count == 0);
        }

        [TestMethod]
        public void ContainsKey_EmptyDictionary_False()
        {
            Assert.IsFalse(_dict.ContainsKey(_john, _firstAddr));
        }
        [TestMethod]
        public void ContainsKey_True()
        {
            _dict.Add(_john, _firstAddr, _redHouse);

            Assert.IsTrue(_dict.ContainsKey(_john, _firstAddr));
        }

        [TestMethod]
        public void ContainsValue_EmptyDictionary_False()
        {
            Assert.IsFalse(_dict.ContainsValue(_blueHouse));
        }

        [TestMethod]
        public void ContainsValue_True()
        {
            _dict.Add(_john, _firstAddr, _redHouse);

            Assert.IsTrue(_dict.ContainsValue(_redHouse));
        }

        [TestMethod]
        public void GetValuesByKey1()
        {
            _dict.Add(_john, _firstAddr, _redHouse);
            _dict.Add(_john, _secondAddr, _blueHouse);
            _dict.Add(_bill, _thirdAddr, _greenHouse);
            _dict.Add(_bill, _fourthAddr, _blackHouse);

            var counter = 0;
            foreach (var item in _dict.GetValuesByKey1(_john))
            {
                counter++;
                Assert.IsTrue(item.Equals(_redHouse) ||
                    item.Equals(_blueHouse));
            }
            Assert.IsTrue(counter == 2);
        }
        [TestMethod]
        public void GetValuesByKey1_NoValue()
        {
            Assert.AreEqual(_dict.GetValuesByKey1(_john), Enumerable.Empty<House>());
            
        }
        [TestMethod]
        public void GetValuesByKey2()
        {
            _dict.Add(_john, _firstAddr, _redHouse);
            _dict.Add(_john, _secondAddr, _blueHouse);
            _dict.Add(_bill, _thirdAddr, _greenHouse);
            _dict.Add(_bill, _fourthAddr, _blackHouse);

            var counter = 0;
            foreach (var item in _dict.GetValuesByKey1(_bill))
            {
                counter++;
                Assert.IsTrue(item.Equals(_greenHouse) ||
                    item.Equals(_blackHouse));
            }
            Assert.IsTrue(counter == 2);
        }
        [TestMethod]
        public void GetValuesByKey2_NoValue()
        {
            Assert.AreEqual(_dict.GetValuesByKey2(_firstAddr), Enumerable.Empty<House>());

        }
        [TestMethod]
        public void GetEnumerator()
        {
            _dict.Add(_john, _firstAddr, _redHouse);
            _dict.Add(_john, _secondAddr, _blueHouse);
            _dict.Add(_bill, _thirdAddr, _greenHouse);
            _dict.Add(_bill, _fourthAddr, _blackHouse);

            var tuple1 = Tuple.Create(_john, _firstAddr, _redHouse);
            var tuple2 = Tuple.Create(_john, _secondAddr, _blueHouse);
            var tuple3 = Tuple.Create(_bill, _thirdAddr, _greenHouse);
            var tuple4 = Tuple.Create(_bill, _fourthAddr, _blackHouse);

            var counter = 0;
            foreach (var item in _dict)
            {
                counter++;
                Assert.IsTrue(item.Equals(tuple1) ||
                    item.Equals(tuple2) ||
                    item.Equals(tuple3) ||
                    item.Equals(tuple4));
            }
            Assert.IsTrue(counter == 4);
        }
        [TestMethod]
        public void Serialization()
        {
            _dict.Add(_john, _firstAddr, _redHouse);
            _dict.Add(_john, _secondAddr, _blueHouse);
            _dict.Add(_bill, _thirdAddr, _greenHouse);
            _dict.Add(_bill, _fourthAddr, _blackHouse);

            var tuple1 = Tuple.Create(_john, _firstAddr, _redHouse);
            var tuple2 = Tuple.Create(_john, _secondAddr, _blueHouse);
            var tuple3 = Tuple.Create(_bill, _thirdAddr, _greenHouse);
            var tuple4 = Tuple.Create(_bill, _fourthAddr, _blackHouse);

            CompositeKeyDictionary<Person, Address, House> deserializedDict;
            var formatter = new BinaryFormatter();
            using (MemoryStream s = new MemoryStream())
            {
                formatter.Serialize(s, _dict);
                s.Seek(0, SeekOrigin.Begin);
                deserializedDict = formatter.Deserialize(s) as CompositeKeyDictionary<Person, Address, House>;
                var counter = 0;
                foreach (var item in deserializedDict)
                {
                    counter++;
                    Assert.IsTrue(item.Equals(tuple1) ||
                        item.Equals(tuple2) ||
                        item.Equals(tuple3) ||
                        item.Equals(tuple4));
                }
                Assert.IsTrue(counter == 4);
            }
            
        }
       
    }
}
