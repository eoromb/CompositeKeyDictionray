using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositeKeyDictionray
{
    //class CompositeKeyDictionary<TKey1, TKey2, TValue> : Dictionary<Tuple<TKey1, TKey2>, TValue>
    //{
    //    #region Fields and properties
    //    private readonly Dictionary<Tuple<TKey1, TKey2>, TValue> _dict = new Dictionary<Tuple<TKey1, TKey2>, TValue>();
    //    public TValue this[TKey1 k1, TKey2 k2]
    //    {
    //        get { return _dict[Tuple.Create(k1, k2)]; }
    //        set { _dict[Tuple.Create(k1, k2)] = value; }
    //    }
    //    #endregion

    //    #region Public methods
    //    public bool TryGetValue(TKey1 k1, TKey2 k2, out TValue v)
    //    {
    //        lock (_dict)
    //        {
    //            return _dict.TryGetValue(Tuple.Create(k1, k2), out v);
    //        }
    //    }
    //    public void Add(TKey1 k1, TKey2 k2, TValue v)
    //    {
    //        lock (_dict)
    //        {
    //            _dict.Add(Tuple.Create(k1, k2), v);
    //        }
    //    }
    //    //public void Clear()
    //    //{
    //    //    lock (_dict)
    //    //    {
    //    //        _dict.Clear();
    //    //    }
    //    //}
    //    public bool ContainsKey(TKey1 k1, TKey2 k2)
    //    {
    //        lock (_dict)
    //        {
    //            return _dict.ContainsKey(Tuple.Create(k1, k2));
    //        }
    //    }
    //    //public bool ContainsValue(TValue v)
    //    //{
    //    //    lock (_dict)
    //    //    {
    //    //        return _dict.ContainsValue(v);
    //    //    }
    //    //}
    //    #endregion

    //    //#region IEnumerable
    //    //public IEnumerator<KeyValuePair<Tuple<TKey1, TKey2>, TValue>> GetEnumerator()
    //    //{
    //    //    return _dict.GetEnumerator();
    //    //}

    //    //IEnumerator IEnumerable.GetEnumerator()
    //    //{
    //    //    return GetEnumerator();
    //    //}
    //    //#endregion
    //}
}
