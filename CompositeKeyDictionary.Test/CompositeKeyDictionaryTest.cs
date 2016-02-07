using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        private readonly Address _firstAddr = new Address("Russia", "Novosibirsk", "Street1", 14);
        private readonly Address _secondAddr = new Address("Russia", "Novosibirsk", "Street2", 22);
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
    }
}
