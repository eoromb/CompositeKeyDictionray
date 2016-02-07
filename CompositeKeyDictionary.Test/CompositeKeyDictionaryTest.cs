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
        public CompositeKeyDictionaryTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
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
        public void Indexer_ModifiedExistingKeys()
        {
            _dict[_john, _firstAddr] = _redHouse;
            _dict[_john, _firstAddr] = _blueHouse;
            Assert.AreEqual(_dict[_john, _firstAddr], _blueHouse);
        }
    }
}
