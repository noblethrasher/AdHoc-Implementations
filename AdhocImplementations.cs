using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prelude.AdhocImplementations
{
    public class AdHocComparable<T> : IComparable<T>
    {
        Func<T, int> compare;

        public AdHocComparable(Func<T, int> comparer)
        {
            this.compare = comparer;
        }
        
        public int CompareTo(T other)
        {
            return compare (other);
        }
    }

    public class AdHocComparer<T> : IComparer<T>
    {
        Func<T, T, int> compare;

        public AdHocComparer(Func<T, T, int> compare) { this.compare = compare; }

        public int Compare(T x, T y)
        {
            return compare (x, y);
        }
    }

    public class AdHocEnumerable<T> : IEnumerable<T>
    {
        Func<IEnumerator<T>> getEnumerator;

        public AdHocEnumerable(Func<IEnumerator<T>> getEnumerator)
        {
            this.getEnumerator = getEnumerator;
        }        
        
        public IEnumerator<T> GetEnumerator()
        {
            return getEnumerator ();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator ();
        }
    }

    public class AdHocEnumerator<T> : IEnumerator<T>
    {

        Func<bool> moveNext;
        Func<T> current;
        Action reset;
        Action dispose;


        public AdHocEnumerator(Func<T> current, Func<bool> moveNext, Action reset, Action dispose)
        {
            this.moveNext = moveNext;
            this.current = current;
            this.reset = reset;
            this.dispose = dispose;
        }

        public AdHocEnumerator(Func<T> current, Func<bool> moveNext, Action reset) : this(current, moveNext, reset, null)
        {

        }

        public AdHocEnumerator(Func<T> current, Func<bool> moveNext, Action dispose)
            : this (current, moveNext, null, dispose)
        {

        }

        public AdHocEnumerator(Func<T> current, Func<bool> moveNext)
            : this (current, moveNext, null, null)
        {

        }
        
        
        public T Current
        {
            get { return current(); }
        }

        public void Dispose()
        {
            if (dispose != null)
                dispose ();
        }

        object System.Collections.IEnumerator.Current
        {
            get { return current(); }
        }

        public bool MoveNext()
        {
            return moveNext ();
        }

        public void Reset()
        {
            if (reset != null)
                reset ();
        }
    }

    public class AdhocEqualityComparer<T> : IEqualityComparer<T>
    {

        Func<T, T, bool> equals;
        Func<T, int> getHashCode;


        public AdhocEqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode)
        {
            this.equals = equals;

            if (getHashCode != null)
            {
                this.getHashCode = getHashCode;
            }
            else
            {
                var method = typeof (T).GetMethod ("GetHashCode");
                this.getHashCode = t => (int) method.Invoke (t, null);
            }
        }

        public AdhocEqualityComparer(Func<T, T, bool> equals) : this (equals, null)
        {

        }
        
        
        public bool Equals(T x, T y)
        {
            return equals (x, y);
        }

        public int GetHashCode(T obj)
        {
            return getHashCode (obj);
        }
    }

    public class AdHocEquatable<T> : IEquatable<T>
    {

        Func<T, bool> equals;

        public AdHocEquatable(Func<T, bool> equals)
        {
            this.equals = equals;
        }
        
        public bool Equals(T other)
        {
            return equals (other);
        }
    }

    public class AdHocGrouping<TKey, TElement> : IGrouping<TKey, TElement>
    {

        Func<TKey> getKey;
        Func<IEnumerator<TElement>> getEnumerator;

        public AdHocGrouping(Func<TKey> getKey, Func<IEnumerator<TElement>> getEnumerator)
        {
            this.getKey = getKey;
            this.getEnumerator = getEnumerator;
        }
        
        public TKey Key
        {
            get { return getKey(); }
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return getEnumerator ();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator ();
        }
    }

    public class AdHocLookup<TKey, TElement> : ILookup<TKey, TElement>
    {

        Func<TKey, bool> contains;
        Func<int> count;
        Func<TKey, IEnumerable<TElement>> indexer;
        Func<IEnumerator<IGrouping<TKey, TElement>>> getEnumerator;



        public AdHocLookup(Func<TKey, bool> contains, Func<int> count, Func<TKey, IEnumerable<TElement>> indexer, Func<IEnumerator<IGrouping<TKey, TElement>>> getEnumerator)
        {
            this.contains = contains;
            this.count = count;
            this.indexer = indexer;
            this.getEnumerator = getEnumerator;
        }
        
        
        
        public bool Contains(TKey key)
        {
            return contains (key);
        }

        public int Count
        {
            get { return count (); }
        }

        public IEnumerable<TElement> this[TKey key]
        {
            get { return indexer(key); }
        }

        public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
        {
            return getEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator ();
        }
    }
}
