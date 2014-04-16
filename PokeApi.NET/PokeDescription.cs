using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;

namespace PokeAPI.NET
{
    /// <summary>
    /// Represents an isntance of a description of a Pokémon in a Game
    /// </summary>
    [Obsolete("This part of the database is still WIP.")]
    public class PokeDescription : PokeApiType
    {
        /// <summary>
        /// Wether it should cache descriptions or not
        /// </summary>
        public static bool ShouldCacheData = true;
        /// <summary>
        /// Gets all cached descriptions
        /// </summary>
        public static Dictionary<int, PokeDescription> CachedDescriptions = new Dictionary<int, PokeDescription>();

        List<NameUriPair> games = new List<NameUriPair>();
        NameUriPair pokemon = new NameUriPair();
        /// <summary>
        /// A list of games this PokeDescription instance is in
        /// </summary>
        public List<NameUriPair> Games
        {
            get
            {
                return games;
            }
        }
        /// <summary>
        /// The pokemon this PokeDescription instance is for
        /// </summary>
        NameUriPair Pokemon
        {
            get
            {
                return pokemon;
            }
        }

        /// <summary>
        /// Gets an entry of the Games list as a PokeGame
        /// </summary>
        /// <param name="index">The index of the entry</param>
        /// <returns>The entry of the Games list as a PokeGame</returns>
        public PokeGame RefGame(int index)
        {
            return PokeGame.GetInstance(Games[index].Name);
        }

        /// <summary>
        /// Creates a new instance from a JSON object
        /// </summary>
        /// <param name="source">The JSON object where to create the new instance from</param>
        protected override void Create(JsonData source)
        {
            foreach (JsonData data in source["games"])
                games.Add(ParseNameUriPair(data));

            pokemon = ParseNameUriPair(source["pokemon"]);

            if (ShouldCacheData && !CachedDescriptions.ContainsKey(ID))
                CachedDescriptions.Add(ID, this);
        }

        /// <summary>
        /// Creates an instance of a PokeDescription with the given ID
        /// </summary>
        /// <param name="id">The id of the PokeDescription to instantiate</param>
        /// <returns>The created instance of the PokeDescription</returns>
        public static PokeDescription GetInstance(int id)
        {
            if (CachedDescriptions.ContainsKey(id))
                return CachedDescriptions[id];

            PokeDescription p = new PokeDescription();
            Create(DataFetcher.GetDescription(id), p);

            if (ShouldCacheData)
                CachedDescriptions.Add(id, p);

            return p;
        }
    }
}
