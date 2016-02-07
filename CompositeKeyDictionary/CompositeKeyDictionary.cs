using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CompositeKeyDictionary
{
    [Serializable]
    public class CompositeKeyDictionary<TKey1, TKey2, TValue> : IEnumerable<Tuple<TKey1, TKey2, TValue>>
    {
        #region Constants
        private const int DefaultCapacity = 30;
        #endregion

        #region Fields and properties
        private readonly object _dictLock = new object();
        private readonly Dictionary<TKey2, Dictionary<TKey1, TValue>> _key2Dict;
        private readonly Dictionary<TKey1, Dictionary<TKey2, TValue>> _key1Dict;
        public TValue this[TKey1 key1, TKey2 key2]
        {
            get { return _key1Dict[key1][key2]; }
            set
            {
                lock (_dictLock)
                {
                    AddValueIntoKey2Dict(key1, key2, value);
                    AddValueIntoKey1Dict(key1, key2, value);
                }
            }
        }
        public int Count
        {
            get
            {
                lock (_dictLock)
                {
                    return _key1Dict.SelectMany(p1 => p1.Value.Select(p2 => p2)).Count();
                }
            }
        }
        #endregion

        #region Constructor
        public CompositeKeyDictionary(int capacity=DefaultCapacity)
        {
            _key2Dict = new Dictionary<TKey2, Dictionary<TKey1, TValue>>(capacity);
            _key1Dict = new Dictionary<TKey1, Dictionary<TKey2, TValue>>(capacity);
        }
        #endregion

        #region Private methods
        private void AddValueIntoKey1Dict(TKey1 key1, TKey2 key2, TValue value, bool bThrowIfAlreadyExists = false)
        {
            Dictionary<TKey2, TValue> dict;
            if (!_key1Dict.TryGetValue(key1, out dict))
            {
                dict = new Dictionary<TKey2, TValue>();
                _key1Dict.Add(key1, dict);
            }
            if (bThrowIfAlreadyExists)
            {
                dict.Add(key2, value);
            }
            else
            {
                dict[key2] = value;
            }
        }
        private void AddValueIntoKey2Dict(TKey1 key1, TKey2 key2, TValue value, bool bThrowIfAlreadyExists = false)
        {
            Dictionary<TKey1, TValue> dict;
            if (!_key2Dict.TryGetValue(key2, out dict))
            {
                dict = new Dictionary<TKey1, TValue>();
                _key2Dict.Add(key2, dict);
            }
            if (bThrowIfAlreadyExists)
            {
                dict.Add(key1, value);
            }
            else
            {
                dict[key1] = value;
            }
        }
        private void RemoveKeyFromKey1Dict(TKey1 key1, TKey2 key2)
        {
            Dictionary<TKey2, TValue> dict;
            if (_key1Dict.TryGetValue(key1, out dict))
            {
                dict.Remove(key2);
                if (dict.Count == 0)
                {
                    _key1Dict.Remove(key1);
                }
            }
        }
        private void RemoveKeyFromKey2Dict(TKey1 key1, TKey2 key2)
        {
            Dictionary<TKey1, TValue> dict;
            if (_key2Dict.TryGetValue(key2, out dict))
            {
                dict.Remove(key1);
                if (dict.Count == 0)
                {
                    _key2Dict.Remove(key2);
                }
            }
        }
        #endregion

        #region Public methods
        public bool TryGetValue(TKey1 key1, TKey2 key2, out TValue value)
        {
            if (EqualityComparer<TKey1>.Default.Equals(key1, default(TKey1)))
            {
                throw new ArgumentNullException(nameof(key1));
            }
            if (EqualityComparer<TKey2>.Default.Equals(key2, default(TKey2)))
            {
                throw new ArgumentNullException(nameof(key2));
            }

            lock (_dictLock)
            {
                value = default(TValue);
                Dictionary<TKey2, TValue> dict;
                if (!_key1Dict.TryGetValue(key1, out dict))
                {
                    return false;
                }
                return dict.TryGetValue(key2, out value);
            }
        }
        public void Add(TKey1 key1, TKey2 key2, TValue value)
        {
            if (EqualityComparer<TKey1>.Default.Equals(key1, default(TKey1)))
            {
                throw new ArgumentNullException(nameof(key1));
            }
            if (EqualityComparer<TKey2>.Default.Equals(key2, default(TKey2)))
            {
                throw new ArgumentNullException(nameof(key2));
            }

            lock (_dictLock)
            {
                try
                {
                    AddValueIntoKey1Dict(key1, key2, value, true);
                    AddValueIntoKey2Dict(key1, key2, value, true);
                }
                catch
                {
                    RemoveKeyFromKey1Dict(key1, key2);
                    RemoveKeyFromKey2Dict(key1, key2);
                    throw;
                }
            }
        }
        public void Remove(TKey1 key1, TKey2 key2)
        {
            if (EqualityComparer<TKey1>.Default.Equals(key1, default(TKey1)))
            {
                throw new ArgumentNullException(nameof(key1));
            }
            if (EqualityComparer<TKey2>.Default.Equals(key2, default(TKey2)))
            {
                throw new ArgumentNullException(nameof(key2));
            }

            lock (_dictLock)
            {
                try
                {

                }
                finally
                {
                    RemoveKeyFromKey1Dict(key1, key2);
                    RemoveKeyFromKey2Dict(key1, key2);
                }
            }
        }
        public void Clear()
        {
            lock (_dictLock)
            {
                _key1Dict.Clear();
                _key2Dict.Clear();
            }
        }
        public bool ContainsKey(TKey1 key1, TKey2 key2)
        {
            if (EqualityComparer<TKey1>.Default.Equals(key1, default(TKey1)))
            {
                throw new ArgumentNullException(nameof(key1));
            }
            if (EqualityComparer<TKey2>.Default.Equals(key2, default(TKey2)))
            {
                throw new ArgumentNullException(nameof(key2));
            }

            lock (_dictLock)
            {
                Dictionary<TKey2, TValue> dict;
                if (!_key1Dict.TryGetValue(key1, out dict))
                {
                    return false;
                }
                return dict.ContainsKey(key2);
            }
        }
        public bool ContainsValue(TValue value)
        {
            lock (_dictLock)
            {
                foreach (var dict in _key1Dict.Values)
                {
                    if (dict.ContainsValue(value))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public IEnumerable<TValue> GetValuesByKey1(TKey1 key1)
        {
            if (EqualityComparer<TKey1>.Default.Equals(key1, default(TKey1)))
            {
                throw new ArgumentNullException(nameof(key1));
            }
           
            lock (_dictLock)
            {
                Dictionary<TKey2, TValue> dict;
                if (!_key1Dict.TryGetValue(key1, out dict))
                {
                    return Enumerable.Empty<TValue>();
                }
                return dict.Values.AsConcurrentEnumerable(_dictLock);
            }
        }
        public IEnumerable<TValue> GetValuesByKey2(TKey2 key2)
        {
            if (EqualityComparer<TKey2>.Default.Equals(key2, default(TKey2)))
            {
                throw new ArgumentNullException(nameof(key2));
            }
            lock (_dictLock)
            {
                Dictionary<TKey1, TValue> dict;
                if (_key2Dict.TryGetValue(key2, out dict))
                {
                    return Enumerable.Empty<TValue>();
                }

                return dict.Values.AsConcurrentEnumerable(_dictLock);
            }
        }

        #endregion

        #region IEnumerable
        public IEnumerator<Tuple<TKey1, TKey2, TValue>> GetEnumerator()
        {
            return _key1Dict.SelectMany(p1 => p1.Value.Select(p2 => Tuple.Create(p1.Key, p2.Key, p2.Value))).AsConcurrentEnumerable(_dictLock).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
        
    }
}
