using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CompositeKeyDictionary
{
    /// <summary>
    /// Коллекция для хранения элементов, имеющих составной ключ. Для быстрого получения всех элементов по "полуключам" используется два словаря.
    /// Не эффективно по памяти.
    /// </summary>
    /// <typeparam name="TKey1">Первая половина ключа</typeparam>
    /// <typeparam name="TKey2">Вторая половина ключа</typeparam>
    /// <typeparam name="TValue">Значение</typeparam>
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
                    AddValueIntoKey2Dict(key1, key2, value);
                    AddValueIntoKey1Dict(key1, key2, value);
                }
            }
        }
        /// <summary>
        /// Возвращает число элементов в коллекции
        /// </summary>
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
        /// <summary>
        /// Добавляет элемент в словарь Key1Dict
        /// </summary>
        /// <param name="key1">Первая половина ключа</param>
        /// <param name="key2">Вторая половина ключа</param>
        /// <param name="value">Значение</param>
        /// <param name="bThrowIfAlreadyExists">Определяет, бросается ли исключение, если элемент с атким значением уже существует</param>
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
        /// <summary>
        /// Добавляет элемент в словарь Key2Dict
        /// </summary>
        /// <param name="key1">Первая половина ключа</param>
        /// <param name="key2">Вторая половина ключа</param>
        /// <param name="value">Значение</param>
        /// <param name="bThrowIfAlreadyExists">Определяет, бросается ли исключение, если элемент с атким значением уже существует</param>
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
        /// <summary>
        /// Удаляет элемент из словаря Key1Dict
        /// </summary>
        /// <param name="key1">Первая половина ключа</param>
        /// <param name="key2">Вторая половина ключа</param>
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
        /// <summary>
        /// Удаляет элемент из словаря Key2Dict
        /// </summary>
        /// <param name="key1">Первая половина ключа</param>
        /// <param name="key2">Вторая половина ключа</param>
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
        /// <summary>
        /// Возвращает значение элемента, если он находится в коллекции
        /// </summary>
        /// <param name="key1">Первая половина ключа</param>
        /// <param name="key2">Вторая половина ключа</param>
        /// <param name="value">Значение элемента, хранящееся в коллекции</param>
        /// <returns>True, если элемент существует, в противном случае false</returns>
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
        /// <summary>
        /// Добавляет элемент в коллекцию. Если элемент с указанным ключамом существует, то генерируется исключение.
        /// </summary>
        /// <param name="key1">Первая половина ключа</param>
        /// <param name="key2">Вторая половина ключа</param>
        /// <param name="value">Значение элемента</param>
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
                catch   // Восстанавливаем состояние коллекции, которое было до вызова метода
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
                finally // Выполняется в блоке finally, чтобы не быть прерваным асинхронными исключениями
                {
                    RemoveKeyFromKey1Dict(key1, key2);
                    RemoveKeyFromKey2Dict(key1, key2);
                }
            }
        }
        /// <summary>
        /// Удалить все элементы из коллекции
        /// </summary>
        public void Clear()
        {
            lock (_dictLock)
            {
                _key1Dict.Clear();
                _key2Dict.Clear();
            }
        }
        /// <summary>
        /// Проверяет существование элемента в коллекции
        /// </summary>
        /// <param name="key1">Первая половина ключа</param>
        /// <param name="key2">Вторая половина ключа</param>
        /// <returns>true, если элемент существует, в противном случае - false</returns>
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
        /// <summary>
        /// Проверяет существование элемента в коллекции
        /// </summary>
        /// <param name="value">Значение элемента</param>
        /// <returns></returns>
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
        /// <summary>
        /// Возвращает все элементы, имеющий key1 в качестве первой половины ключа
        /// </summary>
        /// <param name="key1">Первая половина ключа</param>
        /// <returns>Возвращается потоко-безобасный объект IEnumerable</returns>
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
        /// <summary>
        /// Возвращает все элементы, имеющий key2 в качестве первой половины ключа
        /// </summary>
        /// <param name="key2">Вторая половина ключа</param>
        /// <returns>Возвращается потоко-безобасный объект IEnumerable</returns>
        public IEnumerable<TValue> GetValuesByKey2(TKey2 key2)
        {
            if (EqualityComparer<TKey2>.Default.Equals(key2, default(TKey2)))
            {
                throw new ArgumentNullException(nameof(key2));
            }
            lock (_dictLock)
            {
                Dictionary<TKey1, TValue> dict;
                if (!_key2Dict.TryGetValue(key2, out dict))
                {
                    return Enumerable.Empty<TValue>();
                }

                return dict.Values.AsConcurrentEnumerable(_dictLock);
            }
        }

        #endregion

        #region IEnumerable
        /// <summary>
        /// Возвращает все элементы коллекции в формате Tuple(TKey1, TKey2, TValue)
        /// </summary>
        /// <returns>Возвращается потоко-безобасный enumerator</returns>
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
