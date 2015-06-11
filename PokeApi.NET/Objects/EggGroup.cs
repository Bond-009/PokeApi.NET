using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LitJson;

namespace PokeAPI
{
    public partial class EggGroup : ApiObject<EggGroup>
    {
        static Cache<int, EggGroup> cache = new Cache<int, EggGroup>(async i => Maybe.Just(Create(await DataFetcher.GetEggGroup(i), new EggGroup())));

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

        public ApiResource[] Pokemon
        {
            get;
            private set;
        }

        private EggGroup() { }

        protected override void Create(JsonData source)
        {
            Pokemon = source["pokemon"].Map<JsonData, ApiResource>(ParseResource).ToArray();
        }

        public static async Task<EggGroup> GetInstance(EggGroupID eggGroup) => await GetInstance((int)eggGroup);
        public static async Task<EggGroup> GetInstance(string name) => await GetInstance(IDs[name.ToLower()]);
        public static async Task<EggGroup> GetInstance(int id) => await cache.Get(id);

        public async Task<Pokemon> RefPokemon(int index) => await PokeAPI.Pokemon.GetInstance(Pokemon[index].Name);

        public static implicit operator EggGroupID(EggGroup eggGroup) => (EggGroupID)(eggGroup.Id + 1);
    }
}
