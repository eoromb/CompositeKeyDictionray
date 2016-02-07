using System;

namespace CompositeKeyDictionary.Test
{
    class Address : IEquatable<Address>
    {
        #region Fields and properties
        private string _country;
        public string Country
        {
            private get { return _country; }
            set { _country = value; }
        }
        private string _city;
        public string City
        {
            get { return _city; }
            private set { _city = value; }
        }
        private string _street;
        public string Street
        {
            get { return _street; }
            set { _street = value; }
        }

        private int _houseNumber;
        public int HouseNumber
        {
            get { return _houseNumber; }
            private set { _houseNumber = value; }
        }
        #endregion

        #region Constructors
        public Address(string country, string city, string street, int houseNumber)
        {
            _country = country;
            _city = city;
            _street = street;
            _houseNumber = houseNumber;
        }
        #endregion

        #region IEquatable
        public bool Equals(Address other)
        {
            return other._country == _country &&
                other._city == _city &&
                other._street == _street &&
                other._houseNumber == _houseNumber;
        }
        #endregion

        #region Overrides
        public override bool Equals(object obj)
        {
            return Equals(this);
        }
        public override int GetHashCode()
        {
            return _country.GetHashCode() ^ _city.GetHashCode() ^ _street.GetHashCode() ^ _houseNumber.GetHashCode();
        }
        #endregion
    }
}
