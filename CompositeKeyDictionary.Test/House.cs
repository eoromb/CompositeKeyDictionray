using System;

namespace CompositeKeyDictionary.Test
{
    [Serializable]
    public class House : IEquatable<House>
    {
        #region Fields and properties
        private string _color;
        public string Color
        {
            get { return _color; }
            private set { _color = value; }
        }
        private int _floorsNumber;
        public int FloorsNumber
        {
            get { return _floorsNumber; }
            private set { _floorsNumber = value; }
        }
        #endregion

        #region Constructors
        public House(string color, int floorsNumber)
        {
            _color = color;
            _floorsNumber = floorsNumber;
        }
        #endregion

        #region IEquatable
        public bool Equals(House other)
        {
            return other._color == _color &&
                other._floorsNumber == _floorsNumber;
        }
        #endregion

        #region Overrides
        public override bool Equals(object obj)
        {
            return Equals(this);
        }
        public override int GetHashCode()
        {
            return _color.GetHashCode() ^ _floorsNumber.GetHashCode();
        }
        public override string ToString() => $"Color = {_color}, FloorsNumber = {_floorsNumber}";
        #endregion
    }
}
