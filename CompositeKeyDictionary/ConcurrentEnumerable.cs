using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositeKeyDictionary
{
    class ConcurrentEnumerable<T> : IEnumerable<T>
    {
        #region Fields and properties
        private readonly IEnumerable<T> _enumerable;
        private readonly object _enumerableLock;
        #endregion

        #region Constructor
        public ConcurrentEnumerable(IEnumerable<T> enumerable, object enumerableLock)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }
            if (enumerableLock == null)
            {
                throw new ArgumentNullException(nameof(enumerableLock));
            }
            _enumerable = enumerable;
            _enumerableLock = enumerableLock;
        }
        #endregion

        #region IEnumerable
        public IEnumerator<T> GetEnumerator()
        {
            return new ConcurrentEnumeratorDecorator<T>(_enumerable.GetEnumerator(), _enumerableLock);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        
    }
}
