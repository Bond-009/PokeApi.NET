using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LitJson;

namespace PokeAPI
{
    public partial class EggGroup : ApiObject<EggGroup>
    {
        readonly static string PKMN = "pokemon";

        static Cache<int, EggGroup> cache = new Cache<int, EggGroup>(async i => Maybe.Just(Create(await DataFetcher.GetEggGroup(i), new EggGroup())));

        /// <summary>
        /// Gets the <see cref="EggGroup" /> instance cache.
        /// </summary>
        public static CacheGetter<int, EggGroup> Cache { get; } = new CacheGetter<int, EggGroup>(cache);

        /// <summary>
        /// Gets all Pokemon in this <see cref="EggGroup" />.
        /// </summary>
        public ApiResource[] Pokemon
        {
            get;
            private set;
        }

        EggGroup()
        {
        }

        /// <summary>
        /// Does parsing stuff in the derived class.
        /// </summary>
        /// <param name="source">The JSON data to parse.</param>
        protected override void Create(JsonData source)
        {
            Pokemon = source[PKMN].Map<JsonData, ApiResource>(ParseResource).ToArray();
        }

        /// <summary>
        /// Gets the <see cref="PokeAPI.Pokemon" /> instance represented by <see cref="Pokemon" /> asynchronously.
        /// </summary>
        /// <returns>A task containing the <see cref="PokeAPI.Pokemon" />.</returns>
        public async Task<Pokemon> RefPokemon(int index) => await PokeAPI.Pokemon.GetInstance(Pokemon[index].Name);

        /// <summary>
        /// Gets an <see cref="EggGroup" /> instance from its <see cref="EggGroupId" /> equivalent asynchronously.
        /// </summary>
        /// <param name="eggGroup">The <see cref="EggGroupId" /> equivalent of the <see cref="EggGroup" />.</param>
        /// <returns>A task containing the <see cref="EggGroup" /> instance.</returns>
        public static async Task<EggGroup> GetInstance(EggGroupId eggGroup) => await GetInstance((int)eggGroup);
        /// <summary>
        /// Gets an <see cref="EggGroup" /> instance from its name asynchronously.
        /// </summary>
        /// <param name="name">The name of the <see cref="EggGroup" />.</param>
        /// <returns>A task containing the <see cref="EggGroup" /> instance.</returns>
        public static async Task<EggGroup> GetInstance(string     name    ) => await GetInstance(Ids[name.ToLowerInvariant()]);
        /// <summary>
        /// Gets an <see cref="EggGroup" /> instance from its id asynchronously.
        /// </summary>
        /// <param name="id">The id of the <see cref="EggGroup" />.</param>
        /// <returns>A task containing the <see cref="EggGroup" /> instance.</returns>
        public static async Task<EggGroup> GetInstance(int        id      ) => await cache.Get(id);

        /// <summary>
        /// Implicitely casts an <see cref="EggGroup" /> to its <see cref="EggGroupId" />.
        /// </summary>
        /// <param name="eggGroup">The <see cref="EggGroup" /> to cast.</param>
        public static implicit operator EggGroupId(EggGroup eggGroup) => (EggGroupId)(eggGroup.Id + 1);
    }
}
