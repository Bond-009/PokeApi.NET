using System;
using System.Collections.Generic;
using System.Linq;

namespace PokeAPI
{
    public abstract class ApiObject
    {
        /// <summary>
        /// The identifier for this <see cref="ApiObject" />.
        /// </summary>
        public int ID
        {
            get;
        }
    }
    public abstract class NamedApiObject : ApiObject
    {
        /// <summary>
        /// The name for this <see cref="ApiObject" />.
        /// </summary>
        public string Name
        {
            get;
        }
    }
}
