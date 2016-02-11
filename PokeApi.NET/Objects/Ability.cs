using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LitJson;

namespace PokeAPI
{
    public partial class Ability : ApiObject<Ability>
    {
        readonly static string DESCR = "description";

        static Cache<int, Ability> cache = new Cache<int, Ability>(async i => Maybe.Just(Create(await DataFetcher.GetAbility(i), new Ability())));

        /// <summary>
        /// Gets the <see cref="Ability" /> instance cache.
        /// </summary>
        public static CacheGetter<int, Ability> Cache { get; } = new CacheGetter<int, Ability>(cache);

        /// <summary>
        /// Gets the description of the <see cref="Ability" />.
        /// </summary>
        public string Description
        {
            get;
            private set;
        }

        Ability()
        {
        }

        /// <summary>
        /// Does parsing stuff in the derived class.
        /// </summary>
        /// <param name="source">The JSON data to parse.</param>
        protected override void Create(JsonData source)
        {
            Description = source[DESCR].ToString();
        }

        /// <summary>
        /// Gets an <see cref="Ability" /> instance from <see cref="AbilityId" /> equivalent asynchronously.
        /// </summary>
        /// <param name="ability">The <see cref="AbilityId" /> equivalent of the <see cref="Ability" />.</param>
        /// <returns>A task containing the <see cref="Ability" /> instance.</returns>
        public static async Task<Ability> GetInstance(AbilityId ability) => await GetInstance((int)ability);
        /// <summary>
        /// Gets an <see cref="Ability" /> instance from its name asynchronously.
        /// </summary>
        /// <param name="name">The name of the <see cref="Ability" />.</param>
        /// <returns>A task containing the <see cref="Ability" /> instance.</returns>
        public static async Task<Ability> GetInstance(string    name   ) => await GetInstance(Ids[name.ToLowerInvariant()]);
        /// <summary>
        /// Gets an <see cref="Ability" /> instance from its id asynchronously.
        /// </summary>
        /// <param name="id">The id of the <see cref="Ability" />.</param>
        /// <returns>A task containing the <see cref="Ability" /> instance.</returns>
        public static async Task<Ability> GetInstance(int       id     ) => await cache.Get(id);

        /// <summary>
        /// Implicitely casts an <see cref="Ability" /> to its <see cref="AbilityId" /> equivalent.
        /// </summary>
        /// <param name="ability">The <see cref="Ability" /> to cast.</param>
        public static implicit operator AbilityId(Ability ability)
        {
            // lazy<me>
            AbilityId ret = 0;
            Enum.TryParse(ability.Name.Replace(' ', '_'), false, out ret);
            return ret;
        }
    }
}
