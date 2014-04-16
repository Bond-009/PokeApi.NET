using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;

namespace PokeAPI.NET
{
    /// <summary>
    /// Represents an instance of a Pokémon Game
    /// </summary>
    public class PokeGame : PokeApiType
    {
        /// <summary>
        /// Wether it should cache games or not
        /// </summary>
        public static bool ShouldCacheData = true;
        /// <summary>
        /// Gets all cached games
        /// </summary>
        public static Dictionary<int, PokeGame> CachedGames = new Dictionary<int, PokeGame>();

        #region public readonly static Dictionary<string, int> IDs = new Dictionary<string, int>() { [...] };
        /// <summary>
        /// All game ID string->ID maps
        /// </summary>
        public readonly static Dictionary<string, int> IDs = new Dictionary<string, int>()
        {
            {"Red (jpn)", 1},
            {"Green (jpn)", 2},
            {"Blue (jpn)", 3},
            {"Red", 4},
            {"Blue", 5},
            {"Yellow", 6},
            {"Gold", 7},
            {"Silver", 8},
            {"Crystal", 9},
            {"Ruby", 10},
            {"Sapphire", 11},
            {"Firered", 12},
            {"Leafgreen", 13},
            {"Emerald", 14},
            {"Diamond", 15},
            {"Pearl", 16},
            {"Platinum", 17},
            {"Heartgold", 18},
            {"Soulsilver", 19},
            {"Black", 20},
            {"White", 21},
            {"Black 2", 22},
            {"White 2", 23},
            {"X", 24},
            {"Y", 25}
        };
        #endregion

        int year, gen;
        /// <summary>
        /// The year the PokeGame instance was released
        /// </summary>
        public int ReleaseYear
        {
            get
            {
                return year;
            }
        }
        /// <summary>
        /// The generation this PokeGame instance belongs to
        /// </summary>
        public int Generation
        {
            get
            {
                return gen;
            }
        }

        /// <summary>
        /// Creates a new instance from a JSON object
        /// </summary>
        /// <param name="source">The JSON object where to create the new instance from</param>
        protected override void Create(JsonData source)
        {
            year = (int)source["release_year"];
            gen = (int)source["generation"];

            if (ShouldCacheData && !CachedGames.ContainsKey(ID))
                CachedGames.Add(ID, this);
        }

        /// <summary>
        /// Creates an instance of a PokeGame with the given PokemonGame
        /// </summary>
        /// <param name="game">The PokemonGame of the PokeGame to instantiate</param>
        /// <returns>The created instance of the PokeGame</returns>
        public static PokeGame GetInstance(PokemonGame game)
        {
            return GetInstance((int)game + 1);
        }
        /// <summary>
        /// Creates an instance of a PokeGame with the given name
        /// </summary>
        /// <param name="name">The name of the PokeGame to instantiate</param>
        /// <returns>The created instance of the PokeGame</returns>
        public static PokeGame GetInstance(string name)
        {
            return GetInstance(IDs[name]);
        }
        /// <summary>
        /// Creates an instance of a PokeGame with the given ID
        /// </summary>
        /// <param name="id">The id of the PokeGame to instantiate</param>
        /// <returns>The created instance of the PokeGame</returns>
        public static PokeGame GetInstance(int id)
        {
            if (CachedGames.ContainsKey(id))
                return CachedGames[id];

            PokeGame p = new PokeGame();
            Create(DataFetcher.GetGame(id), p);

            if (ShouldCacheData)
                CachedGames.Add(id, p);

            return p;
        }

        /// <summary>
        /// Converts a PokeGame to a PokemonGame
        /// </summary>
        /// <param name="game">The PokeGame to convert from</param>
        public static implicit operator PokemonGame(PokeGame game)
        {
            return (PokemonGame)(game.ID - 1);
        }
        /// <summary>
        /// Converts a PokemonGame to a PokeGame
        /// </summary>
        /// <param name="game">The PokemonGame to convert from</param>
        public static explicit operator PokeGame(PokemonGame game)
        {
            return GetInstance(game);
        }
    }
}
