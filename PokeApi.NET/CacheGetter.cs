using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LitJson;

namespace PokeAPI
{
    public struct CacheGetter : IDictionary<int, JsonData>
    {
        Cache<int, JsonData> c;

        public ICollection<int     > Keys   => c.Keys  ;
        public ICollection<JsonData> Values => c.Values;

        public int Count => c.Count;
        public bool IsReadOnly => true;

        public JsonData this[int key] => c.TryGetDef(key);

        public CacheGetter(Cache<int, JsonData> cache)
        {
            c = cache;
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public void Clear()
        {
            c.Clear();
        }

        public IEnumerator<KeyValuePair<int, JsonData>> GetEnumerator() => c.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => c.GetEnumerator();

        JsonData IDictionary<int, JsonData>.this[int key]
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

        void IDictionary<int, JsonData>.Add(int key, JsonData value)
        {
            throw new NotImplementedException();
        }
        bool IDictionary<int, JsonData>.ContainsKey(int key)
        {
            throw new NotImplementedException();
        }
        bool IDictionary<int, JsonData>.Remove(int key)
        {
            throw new NotImplementedException();
        }
        bool IDictionary<int, JsonData>.TryGetValue(int key, out JsonData value)
        {
            throw new NotImplementedException();
        }
        void ICollection<KeyValuePair<int, JsonData>>.Add(KeyValuePair<int, JsonData> item)
        {
            throw new NotImplementedException();
        }
        bool ICollection<KeyValuePair<int, JsonData>>.Contains(KeyValuePair<int, JsonData> item)
        {
            throw new NotImplementedException();
        }
        void ICollection<KeyValuePair<int, JsonData>>.CopyTo(KeyValuePair<int, JsonData>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }
        bool ICollection<KeyValuePair<int, JsonData>>.Remove(KeyValuePair<int, JsonData> item)
        {
            throw new NotImplementedException();
        }
    }
}
