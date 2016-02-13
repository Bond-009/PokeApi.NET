using System;
using System.Collections.Generic;
using System.Linq;

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
        }
    }
    public class NamedApiResource<T> : ApiResource<T> where T : NamedApiObject
    {
        /// <summary>
        /// The name of the referenced resource.
        /// </summary>
        public string Name
        {
            get;
        }
    }
}
