using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;

namespace PokeAPI.NET
{
    /// <summary>
    /// Represents an evolution of a Pokemon.
    /// </summary>
    public class Evolution : PokeApiType
    {
        /// <summary>
        /// When or how the Pokemon evolves with the Method, eg. the level needed.
        /// </summary>
        public object MethodPrecision
        {
            get;
            private set;
        }
        /// <summary>
        /// How the Pokemon evolves
        /// </summary>
        public string Method
        {
            get;
            private set;
        }
        /// <summary>
        /// The name of the Pokemon it will evolve to
        /// </summary>
        public string EvolveTo
        {
            get;
            private set;
        }

        /// <summary>
        /// The Pokemon it evolves to, as a Pokemon
        /// </summary>
        public Pokemon ToPokemon
        {
            get
            {
                return Pokemon.GetInstance(EvolveTo);
            }
        }

        /// <summary>
        /// Creates a new instance from a JSON object
        /// </summary>
        /// <param name="source">The JSON object where to create the new instance from</param>
        protected override void Create(JsonData source)
        {
            if (source.Keys.Contains("level"))
                MethodPrecision = (int)source["level"];

            Method = source["method"].ToString().Replace('_', ' ');
            ResourceUri = new Uri("http://www.pokeapi.co/" + source["resource_uri"]);
            EvolveTo = source["to"].ToString();

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
