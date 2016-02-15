using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PokeAPI
{
    struct ResourceListFragment<T, TInner>
        where TInner : ApiObject
        where T : ApiResource<TInner>
    {
        public int Count
        {
            get;
        }

        public Uri Next
        {
            get;
        }
        public Uri Previous
        {
            get;
        }

        public T[] Results
        {
            get;
        }
    }

    struct ResourceListEnumerator<T, TInner> : IEnumerator<T>
        where TInner : ApiObject
        where T : ApiResource<TInner>
    {
        readonly static string LIMIT = "limit", OFFSET = "offset";

        ResourceListFragment<T, TInner> current, start;
        int index, limit;

        public T Current
        {
            get
            {
                if (index == -1)
                    throw new InvalidOperationException();

                return current.Results[index];
            }
        }

        object IEnumerator.Current => Current;

        internal ResourceListEnumerator(int limit = 20)
        {
            index = 0;
            this.limit = limit;

            var t = DataFetcher.GetListJsonOf<TInner>(0, limit);
            t.RunSynchronously();
            if (t.IsFaulted)
                throw t.Exception;

            var j = t.Result;

            throw new NotImplementedException();
            //TODO: deserialize JSON
            // start = current = <?>;
        }

        public void Dispose()
        {
            current = new ResourceListFragment<T, TInner>();
            index = -1;
        }

        public bool MoveNext()
        {
            if (index == -1)
                return false;

            if (++index >= limit)
            {
                if (current.Next == null)
                {
                    index = -1;
                    return false;
                }

                index = 0;

                var qd = PokeExtensions.ParseQuery(current.Next.Query);
                var t = DataFetcher.GetListJsonOf<TInner>(Int32.Parse(qd[OFFSET]), Int32.Parse(qd[LIMIT]));
                t.RunSynchronously();
                if (t.IsFaulted)
                    throw t.Exception;

                var j = t.Result;

                throw new NotImplementedException();
                //TODO: deserialize JSON
                // current = <?>;
            }

            return true;
        }

        public void Reset()
        {
            index = 0;
            current = start;
        }
    }

    public abstract class ResourceList<T, TInner> : IEnumerable<T>
        where TInner : ApiObject
        where T : ApiResource<TInner>
    {
        public int Count
        {
            get;
        }

        public int Limit
        {
            get;
            set;
        }

        public IEnumerator<T> GetEnumerator() => new ResourceListEnumerator<T, TInner>(Limit);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class      ApiResourceList<T> : ResourceList<     ApiResource<T>, T> where T :      ApiObject { }
    public class NamedApiResourceList<T> : ResourceList<NamedApiResource<T>, T> where T : NamedApiObject { }
}
