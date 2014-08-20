using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;

namespace PokeAPI.NET
{
    /// <summary>
    /// Represents an instance of a Pokédex
    /// </summary>
    public class Pokedex : PokeApiType
    {
        /// <summary>
        /// Wether it should cache the pokédex or not
        /// </summary>
        public static bool ShouldCacheData = true;
        /// <summary>
        /// Gets the cached pokédex
        /// </summary>
        public static Pokedex CachedPokedex = null;

        /// <summary>
        /// A big list of Pokemon as NameUriPairs within this Pokedex instance
        /// </summary>
        public IDictionary<int, NameUriPair> PokemonList
        {
            get;
            private set;
        } = new Dictionary<int, NameUriPair>();

        /// <summary>
        /// Gets an entry of the PokemonList as a Pokemon
        /// </summary>
        /// <param name="index">The index of the entry</param>
        /// <returns>The entry of the PokemonList as a Pokemon</returns>
        public Pokemon RefPokemon(int index)
        {
            return Pokemon.GetInstance(PokemonList[ID].Name);
        }

        /// <summary>
        /// Creates a new instance from a JSON object
        /// </summary>
        /// <param name="source">The JSON object where to create the new instance from</param>
        protected override void Create(JsonData source)
        {
            foreach (JsonData data in source["pokemon"])
            {
                string[] num = data["resource_uri"].ToString().Split(new char[1] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                PokemonList.Add(Convert.ToInt32(num[num.Length - 1]), ParseNameUriPair(data));
            }

            var list2 = new Dictionary<int, NameUriPair>(PokemonList);
            PokemonList.Clear();

            foreach (var kvp in (from kvp in list2 orderby kvp.Key select kvp))
                PokemonList.Add(kvp.Key, kvp.Value);
        }

        /// <summary>
        /// Creates an instance of a Pokedex
        /// </summary>
        /// <returns>The created Pokedex instance</returns>
        public static Pokedex GetInstance()
        {
            if (CachedPokedex != null)
                return CachedPokedex;

            Pokedex p = new Pokedex();
            Create(DataFetcher.GetPokedex(), p);

            if (ShouldCacheData)
                CachedPokedex = p;

            return p;
        }
    }
}
