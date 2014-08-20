using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;

namespace PokeAPI.NET
{
    /// <summary>
    /// Represents an isntance of a description of a Pokémon in a Game
    /// </summary>
    public class Description : PokeApiType
    {
        /// <summary>
        /// Wether it should cache descriptions or not.
        /// </summary>
        public static bool ShouldCacheData = true;
        /// <summary>
        /// All cached descriptions1;
        /// </summary>
        public static Dictionary<int, Description> CachedDescriptions = new Dictionary<int, Description>();

        /// <summary>
        /// Gets the list of games this Description instance is in.
        /// </summary>
        public NameUriPair[] Games
        {
            get;
            private set;
        } = new NameUriPair[0];
        /// <summary>
        /// Gets the Pokémon this Description instance is for.
        /// </summary>
        public NameUriPair Pokemon
        {
            get;
            private set;
        } = new NameUriPair(String.Empty, String.Empty);
        /// <summary>
        /// Gets the description text.
        /// </summary>
        public string Text
        {
            get;
            private set;
        } = String.Empty;

        /// <summary>
        /// Gets an entry of the Games list as a Game
        /// </summary>
        /// <param name="index">The index of the entry</param>
        /// <returns>The entry of the Games list as a Game</returns>
        public Game RefGame(int index)
        {
            return Game.GetInstance(Games[index].Name);
        }

        /// <summary>
        /// Creates a new instance from a JSON object
        /// </summary>
        /// <param name="source">The JSON object where to create the new instance from</param>
        protected override void Create(JsonData source)
        {
            List<NameUriPair> games = new List<NameUriPair>();
            foreach (JsonData data in source["games"])
                games.Add(ParseNameUriPair(data));
            Games = games.ToArray();

            Pokemon = ParseNameUriPair(source["pokemon"]);
            Text = (string)source["description"];
        }

        /// <summary>
        /// Creates an instance of a Description with the given ID
        /// </summary>
        /// <param name="id">The id of the Description to instantiate</param>
        /// <returns>The created instance of the Description</returns>
        public static Description GetInstance(int id)
        {
            if (CachedDescriptions.ContainsKey(id))
                return CachedDescriptions[id];

            Description p = new Description();
            Create(DataFetcher.GetDescription(id), p);

            if (ShouldCacheData)
                CachedDescriptions.Add(id, p);

            return p;
        }

        /// <summary>
        /// Casts a Description to a string implicitely.
        /// </summary>
        /// <param name="descr">The Description to cast.</param>
        public static implicit operator string(Description descr)
        {
            return descr.Text;
        }
    }
}
