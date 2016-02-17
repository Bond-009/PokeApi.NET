using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LitJson;

namespace PokeAPI
{
    public class ApiResource<T> where T : ApiObject
    {
        /// <summary>
        /// The URL of the referenced resource.
        /// </summary>
        public Uri Url
        {
            get;
            internal set;
        }

        public virtual async Task<T> GetObject() => JsonMapper.ToObject<T>(await DataFetcher.GetJsonOf<T>(Url));
    }
    public class NamedApiResource<T> : ApiResource<T> where T : NamedApiObject
    {
        /// <summary>
        /// The name of the referenced resource.
        /// </summary>
        public string Name
        {
            get;
            internal set;
        }
    }
}
