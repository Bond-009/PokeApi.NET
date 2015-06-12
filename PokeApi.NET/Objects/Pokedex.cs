using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LitJson;

namespace PokeAPI
{
    /// <summary>
    /// A Pokedex returns the names and ResourceUri for all Pokemon.
    /// </summary>
    public class Pokedex : ApiObject<Pokedex>
    {
        static string PKMN = "pokemon";

        static Cache<Pokedex> cache = new Cache<Pokedex>(async () => Maybe.Just(Create(await DataFetcher.GetPokedex(), new Pokedex())));

        /// <summary>
        /// Gets or sets whether the <see cref="Pokedex" /> object should be cached or not.
        /// </summary>
        public static bool ShouldCacheData = true;
        /// <summary>
        /// Gets or sets the cached <see cref="Pokedex" />.
        /// </summary>
        /// <remarks>Calling the setter clears the cache, but does not do anything else. Set it to 'null' for readability.</remarks>
        public static Pokedex Cached
        {
            get
            {
                return cache.TryGetDef();
            }
            set
            {
                cache.Clear();
            }
        }

        /// <summary>
        /// Gets all Pokemon in the <see cref="Pokedex" />.
        /// </summary>
        public IDictionary<int, ApiResource> Pokemon
        {
            get;
            private set;
        }

        private Pokedex() { }

        /// <summary>
        /// Does parsing stuff in the derived class.
        /// </summary>
        /// <param name="source">The JSON data to parse.</param>
        protected override void Create(JsonData source)
        {
            Pokemon = source[PKMN].Map<JsonData, ApiResource>(ParseResource).OrderBy(r => r.Id).ToDictionary(r => r.Id);
        }

        /// <summary>
        /// Gets the <see cref="PokeAPI.Pokemon" /> instance in the <see cref="Pokedex" /> asynchronously.
        /// </summary>
        /// <param name="index">The national id of the <see cref="PokeAPI.Pokemon" />.</param>
        /// <returns>A task containing the <see cref="PokeAPI.Pokemon" />.</returns>
        public async Task<Pokemon> RefPokemon(int index) => await PokeAPI.Pokemon.GetInstance(Pokemon[index].Id);

        /// <summary>
        /// Gets the <see cref="Pokedex" /> instance asynchronously.
        /// </summary>
        /// <returns>A task containing the <see cref="Pokedex" />.</returns>
        public static async Task<Pokedex> GetInstance() => await cache.Get();
    }
}
