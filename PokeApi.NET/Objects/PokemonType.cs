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
        static Cache<int, PokemonType> cache = new Cache<int, PokemonType>(
            async i => Maybe.Just(Create(await DataFetcher.GetType(i), new PokemonType())),
            defValues: new Dictionary<int, PokemonType>
            {
                [0] = new PokemonType()
                {
                    Created      = DateTime.Now.Date,
                    LastModified = DateTime.Now.Date,

                    Id   = 0    ,
                    Name = "???",

                    Ineffective    = new List<ApiResource>(),
                    NoEffect       = new List<ApiResource>(),
                    Resistance     = new List<ApiResource>(),
                    SuperEffective = new List<ApiResource>(),
                    Weakness       = new List<ApiResource>(),

                    ResourceUri = null
                }
            }
        );

        public static bool ShouldCacheData
        {
            get
            {
                return cache.IsActive;
            }
            set
            {
                cache.IsActive = value;
            }
        }

        public List<ApiResource> NoEffect
        {
            get;
            private set;
        }
        public List<ApiResource> Ineffective
        {
            get;
            private set;
        }
        public List<ApiResource> Resistance
        {
            get;
            private set;
        }
        public List<ApiResource> SuperEffective
        {
            get;
            private set;
        }
        public List<ApiResource> Weakness
        {
            get;
            private set;
        }

        private PokemonType() { }

        protected override void Create(JsonData source)
        {
            NoEffect       = source["no_effect"      ].Map<JsonData, ApiResource>(ParseResource).ToList();
            Ineffective    = source["ineffective"    ].Map<JsonData, ApiResource>(ParseResource).ToList();
            Resistance     = source["resistance"     ].Map<JsonData, ApiResource>(ParseResource).ToList();
            SuperEffective = source["super_effective"].Map<JsonData, ApiResource>(ParseResource).ToList();
            Weakness       = source["weakness"       ].Map<JsonData, ApiResource>(ParseResource).ToList();
        }

        public async Task<PokemonType> RefIneffective   (int index) => await GetInstance(Ineffective   [index].Name);
        public async Task<PokemonType> RefNoEffect      (int index) => await GetInstance(NoEffect      [index].Name);
        public async Task<PokemonType> RefResistance    (int index) => await GetInstance(Resistance    [index].Name);
        public async Task<PokemonType> RefSuperEffective(int index) => await GetInstance(SuperEffective[index].Name);
        public async Task<PokemonType> RefWeakness      (int index) => await GetInstance(Weakness      [index].Name);

        public static async Task<PokemonType> GetInstance(string name)
        {
            if (name.Trim() == "???")
                name = "Unknown";

            TypeId id;

            if (Enum.TryParse(name.Trim(), true, out id))
                return await GetInstance((int)id);

            return null;
        }
        public static async Task<PokemonType> GetInstance(TypeFlags flags) => await GetInstance(flags.Id());
        public static async Task<PokemonType> GetInstance(TypeId    type ) => await GetInstance((int)type );
        public static async Task<PokemonType> GetInstance(int       id   ) => await cache.Get  (id        );

        public static double GetDamageMultiplier(TypeId attacking, TypeFlags defending)
        {
            List<TypeId> analyzed = defending.AnalyzeIDs();

            double ret = 1d;

            for (int i = 0; i < analyzed.Count; i++)
                ret *= DamageMultipliers[new PTIDT(attacking, analyzed[i])];

            return ret;
        }

        public static TypeFlags Combine(IEnumerable<PokemonType> types) => types.Select(t => (TypeFlags)t).Aggregate((a, b) => a | b);
        public static TypeFlags Combine(params PokemonType[] types) => Combine((IEnumerable<PokemonType>)types);

        public static implicit operator TypeFlags(PokemonType type) => (TypeFlags)(1 << type.Id);
        public static implicit operator TypeId   (PokemonType type) => (TypeId   )      type.Id ;
    }
}
