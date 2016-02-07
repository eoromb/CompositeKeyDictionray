using System.Collections.Generic;

namespace CompositeKeyDictionary
{
    static class ConcurrentEnumerableExtensions
    {
        #region Extension methods
        /// <summary>
        /// Возвращает потокобезопасный объект ConcurrentEnumerable для Enumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="enumerableLock"></param>
        /// <returns></returns>
        public static IEnumerable<T> AsConcurrentEnumerable<T>(this IEnumerable<T> enumerable, object enumerableLock)
        {
            return new ConcurrentEnumerable<T>(enumerable, enumerableLock);
        }
        #endregion
    }
}
