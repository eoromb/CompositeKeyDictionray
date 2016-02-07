using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositeKeyDictionray
{
    class CompositeKeyDictionaryExt<TKey1, TKey2, TValue>
    {
        #region Fields and properties
        private readonly object _dictLock = new object();
        private readonly Dictionary<TKey2, Dictionary<TKey1, TValue>> _key2Dict = new Dictionary<TKey2, Dictionary<TKey1, TValue>>();
        private readonly Dictionary<TKey1, Dictionary<TKey2, TValue>> _key1Dict = new Dictionary<TKey1, Dictionary<TKey2, TValue>>();
        private readonly Dictionary<TValue, int> _valueCounters = new Dictionary<TValue, int>();
        public TValue this[TKey1 k1, TKey2 k2]
        {
            get { return _key1Dict[k1][k2]; }
            set
            {
                lock (_dictLock)
                {
                    AddValueIntoKey2Dict(k1, k2, value);
                    AddValueIntoKey1Dict(k1, k2, value);
                }
            }
        }
        #endregion

        #region Private methods
        private void AddValueIntoKey1Dict(TKey1 k1, TKey2 k2, TValue v, bool bThrowIfAlreadyExists=false)
        {
            Dictionary<TKey2, TValue> dict;
            if (!_key1Dict.TryGetValue(k1, out dict))
            {
                dict = new Dictionary<TKey2, TValue>();
                _key1Dict.Add(k1, dict);
            }
            if (bThrowIfAlreadyExists)
            {
                dict.Add(k2, v);
            }
            else
            {
                dict[k2] = v;
            }
        }
        private void AddValueIntoKey2Dict(TKey1 k1, TKey2 k2, TValue v, bool bThrowIfAlreadyExists = false)
        {
            Dictionary<TKey1, TValue> dict;
            if (!_key2Dict.TryGetValue(k2, out dict))
            {
                dict = new Dictionary<TKey1, TValue>();
                _key2Dict.Add(k2, dict);
            }
            if (bThrowIfAlreadyExists)
            {
                dict.Add(k1, v);
            }
            else
            {
                dict[k1] = v;
            }
        } 
        #endregion

        #region Public methods
        public bool TryGetValue(TKey1 k1, TKey2 k2, out TValue v)
        {
            lock (_dictLock)
            {
                v = default(TValue);
                Dictionary<TKey2, TValue> dict;
                if (!_key1Dict.TryGetValue(k1, out dict))
                {
                    return false;
                }
                return dict.TryGetValue(k2, out v);
            }
        }
        public void Add(TKey1 k1, TKey2 k2, TValue v)
        {
            lock (_dictLock)
            {
                AddValueIntoKey1Dict(k1, k2, v, true);
                AddValueIntoKey2Dict(k1, k2, v, true);
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
        public bool ContainsKey(TKey1 k1, TKey2 k2)
        {
            lock (_dictLock)
            {
                Dictionary<TKey2, TValue> dict;
                if (!_key1Dict.TryGetValue(k1, out dict))
                {
                    return false;
                }
                return dict.ContainsKey(k2);
            }
        }
        public bool ContainsValue(TValue v)
        {
            lock (_dictLock)
            {
                foreach (var dict in _key1Dict.Values)
                {
                    if (dict.ContainsValue(v))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        #endregion

        //#region IEnumerable
        //public IEnumerator<KeyValuePair<Tuple<TKey1, TKey2>, TValue>> GetEnumerator()
        //{
        //    return _dict.GetEnumerator();
        //}

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return GetEnumerator();
        //}
        //#endregion
    }
}
