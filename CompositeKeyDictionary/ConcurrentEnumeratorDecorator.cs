using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace CompositeKeyDictionary
{
    /// <summary>
    /// Реализует потокобезопасный декоратор надо Enumerator. При создании локает объект переданный в конструкторе. В методе Dispose освобождает его.
    /// ВАЖНО, вызывать Dispose при использовании этого класса
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class ConcurrentEnumeratorDecorator<T> : IEnumerator<T>
    {
        #region Fields and properties
        private readonly IEnumerator<T> _innerEnumerator;
        private readonly object _lockForEnumerator;
        #endregion

        #region Constructor
        public ConcurrentEnumeratorDecorator(IEnumerator<T> innerEnumerator, object lockForEnumerator)
        {
            if (innerEnumerator == null)
            {
                throw new ArgumentNullException(nameof(innerEnumerator));
            }
            if (lockForEnumerator == null)
            {
                throw new ArgumentNullException(nameof(lockForEnumerator));
            }
            _lockForEnumerator = lockForEnumerator;
            Monitor.Enter(_lockForEnumerator);
            _innerEnumerator = innerEnumerator;
        }
        #endregion

        #region IEnumerator
        public T Current
        {
            get { return _innerEnumerator.Current; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public void Dispose() => Monitor.Exit(_lockForEnumerator);
        public bool MoveNext() => _innerEnumerator.MoveNext();
        public void Reset() => _innerEnumerator.Reset();
        #endregion
    }
}
