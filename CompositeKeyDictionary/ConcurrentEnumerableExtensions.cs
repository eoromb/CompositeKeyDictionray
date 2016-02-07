using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositeKeyDictionary
{
    static class ConcurrentEnumerableExtensions
    {
        #region Extension methods
        public static IEnumerable<T> AsConcurrentEnumerable<T>(this IEnumerable<T> enumerable, object enumerableLock)
        {
            return new ConcurrentEnumerable<T>(enumerable, enumerableLock);
        }
        #endregion
    }
}
