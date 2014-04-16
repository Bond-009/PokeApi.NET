using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;

namespace PokeAPI.NET
{
    /// <summary>
    /// Represents an evolution of a Pokemon
    /// Not an API class
    /// </summary>
    public class PokeEvolution : PokeApiType
    {
        object methodPrecision;
        string method, to;

        /// <summary>
        /// When or how the Pokemon evolves with the Method, eg. the level needed.
        /// </summary>
        public object MethodPrecision
        {
            get
            {
                return methodPrecision;
            }
        }
        /// <summary>
        /// How the Pokemon evolves
        /// </summary>
        public string Method
        {
            get
            {
                return method;
            }
        }
        /// <summary>
        /// The name of the Pokemon it will evolve to
        /// </summary>
        public string EvolveTo
        {
            get
            {
                return to;
            }
        }

        /// <summary>
        /// The Pokemon it evolves to, as a Pokemon
        /// </summary>
        public Pokemon ToPokemon
        {
            get
            {
                return Pokemon.GetInstance(to);
            }
        }

        /// <summary>
        /// Creates a new instance from a JSON object
        /// </summary>
        /// <param name="source">The JSON object where to create the new instance from</param>
        protected override void Create(JsonData source)
        {
            if (source.Keys.Contains("level"))
                methodPrecision = (int)source["level"];

            method = source["method"].ToString();
            ResourceUri = new Uri("http://www.pokeapi.co/" + source["resource_uri"]);
            to = source["to"].ToString();

            //Name = source["name"].ToString();
            //Created = ParseDateString(source["created"].ToString());
            //LastModified = ParseDateString(source["modified"].ToString());
        }


        /// <summary>
        /// Wether to override default parsing (creation, name, id, ...) or not
        /// </summary>
        /// <returns>true if default parsing should be overridden, false otherwise.</returns>
        protected override bool OverrideDefaultParsing()
        {
            return true;
        }
    }
}
