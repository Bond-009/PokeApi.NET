using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LitJson;

namespace PokeAPI
{
    using PTIDT = Tuple<TypeId, TypeId>;

    public partial class PokemonType : ApiObject<PokemonType>
    {
        readonly static ApiResource[] EmptyResArr = { };

        readonly static string
            NO_EFF = "no_effect",
            INEFF  = "ineffective",
            RESIST = "resistance",
            SUPERE = "super_effective",
            WEAKNS = "weakness",

            QMARKS  = "???",
            UNKNOWN = "Unknown";

        static Cache<int, PokemonType> cache = new Cache<int, PokemonType>(
            async i => Maybe.Just(Create(await DataFetcher.GetType(i), new PokemonType())),
            defValues: new Dictionary<int, PokemonType>
            {
                [0] = new PokemonType()
                {
                    Created      = DateTime.Now.Date,
                    LastModified = DateTime.Now.Date,

                    Id   = 0     ,
                    Name = QMARKS,

                    Ineffective    = EmptyResArr,
                    NoEffect       = EmptyResArr,
                    Resistance     = EmptyResArr,
                    SuperEffective = EmptyResArr,
                    Weakness       = EmptyResArr,

                    ResourceUri = null
                }
            }
        );

        /// <summary>
        /// Gets the <see cref="PokemonType" /> instance cache.
        /// </summary>
        public static CacheGetter<int, PokemonType> Cache { get; } = new CacheGetter<int, PokemonType>(cache);

        /// <summary>
        /// Gets an array of <see cref="ApiResource" />s pointing to types this <see cref="PokemonType" /> has no effect against.
        /// </summary>
        public ApiResource[] NoEffect
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets an array of <see cref="ApiResource" />s pointing to types this <see cref="PokemonType" /> is ineffective against.
        /// </summary>
        public ApiResource[] Ineffective
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets an array of <see cref="ApiResource" />s pointing to types this <see cref="PokemonType" /> is resistent to.
        /// </summary>
        public ApiResource[] Resistance
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets an array of <see cref="ApiResource" />s pointing to types this <see cref="PokemonType" /> is super effective against.
        /// </summary>
        public ApiResource[] SuperEffective
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets an array of <see cref="ApiResource" />s pointing to types this <see cref="PokemonType" /> is weak to.
        /// </summary>
        public ApiResource[] Weakness
        {
            get;
            private set;
        }

        private PokemonType() { }

        /// <summary>
        /// Does parsing stuff in the derived class.
        /// </summary>
        /// <param name="source">The JSON data to parse.</param>
        protected override void Create(JsonData source)
        {
            NoEffect       = source[NO_EFF].Map<JsonData, ApiResource>(ParseResource).ToArray();
            Ineffective    = source[INEFF ].Map<JsonData, ApiResource>(ParseResource).ToArray();
            Resistance     = source[RESIST].Map<JsonData, ApiResource>(ParseResource).ToArray();
            SuperEffective = source[SUPERE].Map<JsonData, ApiResource>(ParseResource).ToArray();
            Weakness       = source[WEAKNS].Map<JsonData, ApiResource>(ParseResource).ToArray();
        }

        /// <summary>
        /// Gets the <see cref="PokemonType" /> instance represented by an element in the <see cref="Ineffective" /> array.
        /// </summary>
        /// <param name="index">The index of the element.</param>
        /// <returns>A task containing the <see cref="PokemonType" />.</returns>
        public async Task<PokemonType> RefIneffective   (int index) => await GetInstanceAsync(Ineffective   [index].Name);
        /// <summary>
        /// Gets the <see cref="PokemonType" /> instance represented by an element in the <see cref="NoEffect" /> array.
        /// </summary>
        /// <param name="index">The index of the element.</param>
        /// <returns>A task containing the <see cref="PokemonType" />.</returns>
        public async Task<PokemonType> RefNoEffect      (int index) => await GetInstanceAsync(NoEffect      [index].Name);
        /// <summary>
        /// Gets the <see cref="PokemonType" /> instance represented by an element in the <see cref="Resistance" /> array.
        /// </summary>
        /// <param name="index">The index of the element.</param>
        /// <returns>A task containing the <see cref="PokemonType" />.</returns>
        public async Task<PokemonType> RefResistance    (int index) => await GetInstanceAsync(Resistance    [index].Name);
        /// <summary>
        /// Gets the <see cref="PokemonType" /> instance represented by an element in the <see cref="SuperEffective" /> array.
        /// </summary>
        /// <param name="index">The index of the element.</param>
        /// <returns>A task containing the <see cref="PokemonType" />.</returns>
        public async Task<PokemonType> RefSuperEffective(int index) => await GetInstanceAsync(SuperEffective[index].Name);
        /// <summary>
        /// Gets the <see cref="PokemonType" /> instance represented by an element in the <see cref="Weakness" /> array.
        /// </summary>
        /// <param name="index">The index of the element.</param>
        /// <returns>A task containing the <see cref="PokemonType" />.</returns>
        public async Task<PokemonType> RefWeakness      (int index) => await GetInstanceAsync(Weakness      [index].Name);

        /// <summary>
        /// Gets a <see cref="PokemonType" /> instance from its name asynchronously.
        /// </summary>
        /// <param name="name">The name of the <see cref="PokemonType" /> to get.</param>
        /// <returns>A task containing the <see cref="PokemonType" />.</returns>
        public static async Task<PokemonType> GetInstanceAsync(string name)
        {
            if (name.Trim() == QMARKS)
                name = UNKNOWN;

            TypeId id;
            if (Enum.TryParse(name.Trim(), true, out id))
                return await GetInstanceAsync((int)id);

            return null;
        }
        /// <summary>
        /// Gets a <see cref="PokemonType" /> instance from the <see cref="TypeFlags" /> equivalent asynchronously.
        /// </summary>
        /// <param name="flags">The <see cref="TypeFlags" /> representing the <see cref="PokemonType" /> to get.</param>
        /// <returns>A task containing the <see cref="PokemonType" />.</returns>
        public static async Task<PokemonType> GetInstanceAsync(TypeFlags flags) => await GetInstanceAsync(flags.Id());
        /// <summary>
        /// Gets a <see cref="PokemonType" /> instance from its id asynchronously.
        /// </summary>
        /// <param name="id">The <see cref="TypeId" /> representing the <see cref="PokemonType" /> to get.</param>
        /// <returns>A task containing the <see cref="PokemonType" />.</returns>
        public static async Task<PokemonType> GetInstanceAsync(TypeId    type ) => await GetInstanceAsync((int)type );
        /// <summary>
        /// Gets a <see cref="PokemonType" /> instance from its id asynchronously.
        /// </summary>
        /// <param name="id">The id of the <see cref="PokemonType" /> to get.</param>
        /// <returns>A task containing the <see cref="PokemonType" />.</returns>
        public static async Task<PokemonType> GetInstanceAsync(int       id   ) => await cache.Get  (id        );

        /// <summary>
        /// Calculates the damage multiplier of an attacking and defending type.
        /// </summary>
        /// <param name="attacking">The attacking type.</param>
        /// <param name="defending">The defending type. Can be multiple flags combined.</param>
        /// <returns>The damage multiplier.</returns>
        public static double GetDamageMultiplier(TypeId    attacking, TypeFlags defending)
        {
            List<TypeId> analyzed = defending.AnalyzeIds();

            if (attacking == TypeId.Unknown || (analyzed.Count == 1 && analyzed[0] == TypeId.Unknown))
                return 1d;

            return analyzed.Select(t => DamageMultipliers[Tuple.Create(attacking, t)]).Aggregate((a, b) => a * b);
        }

        /// <summary>
        /// Combines multiple <see cref="PokemonType" />s to its flags representation.
        /// </summary>
        /// <param name="types">The types to combine.</param>
        /// <returns>The combined type as a <see cref="TypeFlags" />.</returns>
        public static TypeFlags Combine(IEnumerable<PokemonType>  types) => types.Select(t => (TypeFlags)t).Aggregate((a, b) => a | b);
        /// <summary>
        /// Combines multiple <see cref="PokemonType" />s to its flags representation.
        /// </summary>
        /// <param name="types">The types to combine.</param>
        /// <returns>The combined type as a <see cref="TypeFlags" />.</returns>
        public static TypeFlags Combine(params      PokemonType[] types) => Combine((IEnumerable<PokemonType>)types);

        /// <summary>
        /// Implicitely converts a <see cref="PokemonType" /> to its flags representation.
        /// </summary>
        /// <param name="type">The <see cref="PokemonType" /> to convert.</param>
        public static implicit operator TypeFlags(PokemonType type) => (TypeFlags)(1 << (type.Id - 1));
        /// <summary>
        /// Implicitely converts a <see cref="PokemonType" /> to its id.
        /// </summary>
        /// <param name="type">The <see cref="PokemonType" /> to convert.</param>
        public static implicit operator TypeId   (PokemonType type) => (TypeId   )       type.Id      ;
    }
}
