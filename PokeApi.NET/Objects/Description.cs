using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LitJson;

namespace PokeAPI
{
    public class Description : ApiObject<Description>
    {
        readonly static string
            GAMES = "games",
            PKMN  = "pokemon",
            TEXT  = "description";

        static Cache<int, Description> cache = new Cache<int, Description>(async i => Maybe.Just(Create(await DataFetcher.GetDescription(i), new Description())));

        /// <summary>
        /// Gets the <see cref="Description" /> cache.
        /// </summary>
        public static CacheGetter<int, Description> Cache { get; } = new CacheGetter<int, Description>(cache);

        /// <summary>
        /// Gets the games this <see cref="Description" /> appears in.
        /// </summary>
        public ApiResource[] Games
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the Pokemon this <see cref="Description" /> belongs to.
        /// </summary>
        public ApiResource Pokemon
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the description text.
        /// </summary>
        public string Text
        {
            get;
            private set;
        }

        Description()
        {
        }

        /// <summary>
        /// Does parsing stuff in the derived class.
        /// </summary>
        /// <param name="source">The JSON data to parse.</param>
        protected override void Create(JsonData source)
        {
            Games = source[GAMES].Map<JsonData, ApiResource>(ParseResource).ToArray();

            Pokemon = ParseResource(source[PKMN]);
            Text = (string)source[TEXT];
        }

        /// <summary>
        /// Gets the <see cref="Game" /> instance represented by <see cref="Games" /> asynchronously.
        /// </summary>
        /// <param name="index">The index of the <see cref="Game" /> to return.</param>
        /// <returns>A task containing the <see cref="Game" />.</returns>
        public async Task<Game> RefGame(int index) => await Game.GetInstance(Games[index].Name);

        /// <summary>
        /// Gets a <see cref="Description" /> instance from its id asynchronously.
        /// </summary>
        /// <param name="id">The id of the <see cref="Description" />.</param>
        /// <returns>A task containing the <see cref="Description" /> instance.</returns>
        public static async Task<Description> GetInstance(int id) => await cache.Get(id);

        /// <summary>
        /// Implicitely casts a <see cref="Description" /> to its text <see cref="string" />.
        /// </summary>
        /// <param name="descr">The <see cref="Description" /> to cast.</param>
        public static implicit operator string (Description descr) => descr.Text;
    }
}
