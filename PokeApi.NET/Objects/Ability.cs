using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LitJson;

namespace PokeAPI
{
    public partial class Ability : ApiObject<Ability>
    {
        static Cache<int, Ability> cache = new Cache<int, Ability>(async i => Maybe.Just(Create(await DataFetcher.GetAbility(i), new Ability())));

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

        public string Description
        {
            get;
            private set;
        }

        private Ability() { }

        protected override void Create(JsonData source)
        {
            Description = source["description"].ToString();
        }

        public static async Task<Ability> GetInstance(AbilityId ability) => await GetInstance((int)ability);
        public static async Task<Ability> GetInstance(string name) => await GetInstance(IDs[name.ToLowerInvariant()]);
        public static async Task<Ability> GetInstance(int id) => await cache.Get(id);

        public static implicit operator AbilityId(Ability ability)
        {
            // lazy<me>
            AbilityId ret = 0;
            Enum.TryParse(ability.Name.Replace(' ', '_'), false, out ret);
            return ret;
        }
    }
}
