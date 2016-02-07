using System;

namespace CompositeKeyDictionary.Test
{
    [Serializable]
    public struct Person : IEquatable<Person>
    {
        #region Fields and properties
        private readonly int _age;
        public int Age
        {
            get { return _age; }
        }
        private readonly string _name;
        public string Name
        {
            get { return _name; }
        }
        private readonly Guid _ssn;
        public Guid SSN
        {
            get { return _ssn; }
        }

        #endregion

        #region Constructor
        public Person(int age, string name)
        {
            _age = age;
            _name = name;
            _ssn = Guid.NewGuid();
        }
        #endregion

        #region IEquatable
        public bool Equals(Person other)
        {
            return other._ssn.Equals(_ssn) &&
                other._age == _age &&
                other._name == _name;
        }
        #endregion

        #region Overrides
        public override bool Equals(object obj)
        {
            return Equals(this);
        }
        public override int GetHashCode()
        {
            return _ssn.GetHashCode() ^ _age.GetHashCode() ^ _name.GetHashCode();
        }
        public override string ToString() => $"Age = {_age}, Name = {_name}, SSN = {_ssn}";        
        #endregion
    }
}
