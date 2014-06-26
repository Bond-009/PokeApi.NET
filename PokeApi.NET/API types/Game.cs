using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;

namespace PokeAPI.NET
{
    /// <summary>
    /// Represents an instance of a Pokémon Game.
    /// </summary>
    /// <remarks>Can be converted to a GameID enumeration (and vice versa).</remarks>
    public class Game : PokeApiType
    {
        /// <summary>
        /// Wether it should cache games or not
        /// </summary>
        public static bool ShouldCacheData = true;
        /// <summary>
        /// Gets all cached games
        /// </summary>
        public static Dictionary<int, Game> CachedGames = new Dictionary<int, Game>();

        #region public readonly static Dictionary<string, int> IDs = new Dictionary<string, int>() { [...] };
        /// <summary>
        /// All game ID string->ID maps
        /// </summary>
        public readonly static Dictionary<string, int> IDs = new Dictionary<string, int>()
        {
            {"red (jpn)", 1},
            {"green (jpn)", 2},
            {"blue (jpn)", 3},
            {"red", 4},
            {"blue", 5},
            {"yellow", 6},
            {"gold", 7},
            {"silver", 8},
            {"crystal", 9},
            {"ruby", 10},
            {"sapphire", 11},
            {"firered", 12},
            {"leafgreen", 13},
            {"emerald", 14},
            {"diamond", 15},
            {"pearl", 16},
            {"platinum", 17},
            {"heartgold", 18},
            {"soulsilver", 19},
            {"black", 20},
            {"white", 21},
            {"black 2", 22},
            {"white 2", 23},
            {"x", 24},
            {"y", 25}
        };
        #endregion

        /// <summary>
        /// The year the Game instance was released
        /// </summary>
        public int ReleaseYear
        {
            get;
            private set;
        }
        /// <summary>
        /// The generation this Game instance belongs to
        /// </summary>
        public int Generation
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a new instance from a JSON object
        /// </summary>
        /// <param name="source">The JSON object where to create the new instance from</param>
        protected override void Create(JsonData source)
        {
            ReleaseYear = (int)source["release_year"];
            Generation = (int)source["generation"];
        }

        /// <summary>
        /// Creates an instance of a Game with the given PokemonGame
        /// </summary>
        /// <param name="game">The PokemonGame of the Game to instantiate</param>
        /// <returns>The created instance of the Game</returns>
        public static Game GetInstance(GameID game)
        {
            return GetInstance((int)game + 1);
        }
        /// <summary>
        /// Creates an instance of a Game with the given name
        /// </summary>
        /// <param name="name">The name of the Game to instantiate</param>
        /// <returns>The created instance of the Game</returns>
        public static Game GetInstance(string name)
        {
            return GetInstance(IDs[name.ToLower()]);
        }
        /// <summary>
        /// Creates an instance of a Game with the given ID
        /// </summary>
        /// <param name="id">The id of the Game to instantiate</param>
        /// <returns>The created instance of the Game</returns>
        public static Game GetInstance(int id)
        {
            if (CachedGames.ContainsKey(id))
                return CachedGames[id];

            Game p = new Game();
            Create(DataFetcher.GetGame(id), p);

            if (ShouldCacheData)
                CachedGames.Add(id, p);

            return p;
        }

        /// <summary>
        /// Converts a Game to a PokemonGame
        /// </summary>
        /// <param name="game">The Game to convert from</param>
        public static implicit operator GameID(Game game)
        {
            return (GameID)(game.ID - 1);
        }
        /// <summary>
        /// Converts a PokemonGame to a Game
        /// </summary>
        /// <param name="game">The PokemonGame to convert from</param>
        public static explicit operator Game(GameID game)
        {
            return GetInstance(game);
        }
    }
}
