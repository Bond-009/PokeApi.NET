using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#pragma warning disable RECS0083

namespace PokeAPI
{
    public class Cache<TKey, TValue> : IDictionary<TKey, TValue>
    {
        readonly Dictionary<TKey, TValue> dict;
        Func<TKey, Task<Maybe<TValue>>> get;

        public bool IsActive
        {
            get;
            set;
        }

        public Cache(Func<TKey, Task<Maybe<TValue>>> getNew, bool active = true, IDictionary<TKey, TValue> defValues = null)
        {
            get = getNew;

            IsActive = active;

            if (defValues != null)
                dict = new Dictionary<TKey, TValue>(defValues);
            else
                dict = new Dictionary<TKey, TValue>();
        }

        public async Task<TValue> Get(TKey key)
        {
            TValue v;
            if (dict.TryGetValue(key, out v))
                return v;

            return await get(key).ContinueWith(t =>
            {
                var m = t.Result;

                if (m.HasValue)
                {
                    if (IsActive)
                        dict.Add(key, m.Value);

                    return m.Value;
                }

                throw new KeyNotFoundException();
            });
        }
        public Maybe<TValue> TryGet(TKey key)
        {
            TValue v;
            if (dict.TryGetValue(key, out v))
                return new Maybe<TValue>(v);

            return Maybe<TValue>.Nothing;
        }
        public TValue TryGetDef(TKey key)
        {
            var mv = TryGet(key);

            return mv.HasValue ? mv.Value : default(TValue);
        }

        public int Count => dict.Count;
        public bool IsReadOnly => true;
        public ICollection<TKey  > Keys   => dict.Keys  ;
        public ICollection<TValue> Values => dict.Values;

        public void Clear()
        {
            dict.Clear();
        }

        public bool ContainsKey(TKey key) => dict.ContainsKey(key);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dict.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => dict.GetEnumerator();

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) => ((IDictionary<TKey, TValue>)dict).Contains(item);
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((IDictionary<TKey, TValue>)dict).CopyTo(array, arrayIndex);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }
        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }
        bool IDictionary<TKey, TValue>.Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        TValue IDictionary<TKey, TValue>.this[TKey key]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value) => dict.TryGetValue(key, out value);
    }
    public class Cache<T> : Cache<Unit, T>
    {
        public Cache(Func<Unit, Task<Maybe<T>>> getNew)
            : base(getNew)
        {

        }
        public Cache(Func<Task<Maybe<T>>> getNew)
            : base(_ => getNew())
        {

        }

        public Task <T> Get      () => Get      (0);
        public Maybe<T> TryGet   () => TryGet   (0);
        public       T  TryGetDef() => TryGetDef(0);
    }
}
