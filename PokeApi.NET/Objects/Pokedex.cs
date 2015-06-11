using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LitJson;

namespace PokeAPI
{
    public class Pokedex : ApiObject<Pokedex>
    {
        static Cache<Pokedex> cache = new Cache<Pokedex>(async () => Maybe.Just(Create(await DataFetcher.GetPokedex(), new Pokedex())));

        public static bool ShouldCacheData = true;

        public IDictionary<int, ApiResource> Pokemon
        {
            get;
            private set;
        }

        private Pokedex() { }

        protected override void Create(JsonData source)
        {
            Pokemon = source["pokemon"].Map<JsonData, ApiResource>(ParseResource).OrderBy(r => r.Id).ToDictionary(r => r.Id);
        }

        public async Task<Pokemon> RefPokemon(int index) => await PokeAPI.Pokemon.GetInstance(Pokemon[index].Id);

        public static async Task<Pokedex> GetInstance() => await cache.Get();
    }
}
