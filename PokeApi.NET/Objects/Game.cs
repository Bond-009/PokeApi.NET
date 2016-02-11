using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LitJson;

namespace PokeAPI
{
    public partial class Game : ApiObject<Game>
    {
        readonly static string
            R_Y = "release_year",
            GEN = "generation";

        static Cache<int, Game> cache = new Cache<int, Game>(async i => Maybe.Just(Create(await DataFetcher.GetGame(i), new Game())));

        /// <summary>
        /// Gets the <see cref="Game" /> instance cache.
        /// </summary>
        public static CacheGetter<int, Game> Cache { get; } = new CacheGetter<int, Game>(cache);

        /// <summary>
        /// Gets the year the <see cref="Game" /> was released in.
        /// </summary>
        public int ReleaseYear
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the generation number of the <see cref="Game" />.
        /// </summary>
        public int Generation
        {
            get;
            private set;
        }

        Game()
        {
        }

        /// <summary>
        /// Does parsing stuff in the derived class.
        /// </summary>
        /// <param name="source">The JSON data to parse.</param>
        protected override void Create(JsonData source)
        {
            ReleaseYear = source.AsInt(R_Y);
            Generation  = source.AsInt(GEN);
        }

        /// <summary>
        /// Gets a <see cref="Game" /> instance from its <see cref="GameId" />.
        /// </summary>
        /// <param name="game">The <see cref="GameId" /> of the <see cref="Game" /> to get.</param>
        /// <returns>A task containing the <see cref="Game" /> instance.</returns>
        public static async Task<Game> GetInstanceAsync(GameId game) => await GetInstanceAsync((int)game);
        /// <summary>
        /// Gets a <see cref="Game" /> instance from its name.
        /// </summary>
        /// <param name="id">The name of the <see cref="Game" /> to get.</param>
        /// <returns>A task containing the <see cref="Game" /> instance.</returns>
        public static async Task<Game> GetInstanceAsync(string name) => await GetInstanceAsync(Ids[name.ToLowerInvariant()]);
        /// <summary>
        /// Gets a <see cref="Game" /> instance from its id.
        /// </summary>
        /// <param name="id">The id of the <see cref="Game" /> to get.</param>
        /// <returns>A task containing the <see cref="Game" /> instance.</returns>
        public static async Task<Game> GetInstanceAsync(int    id  ) => await cache.Get(id);

        /// <summary>
        /// Implicitely casts a <see cref="Game" /> to its id.
        /// </summary>
        /// <param name="game">The <see cref="Game" /> to cast.</param>
        public static implicit operator GameId(Game game) => (GameId)(game.Id - 1);
    }
}
