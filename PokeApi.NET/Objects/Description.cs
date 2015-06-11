using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LitJson;

namespace PokeAPI
{
    public class Description : ApiObject<Description>
    {
        static Cache<int, Description> cache = new Cache<int, Description>(async i => Maybe.Just(Create(await DataFetcher.GetDescription(i), new Description())));

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

        public ApiResource[] Games
        {
            get;
            private set;
        }
        public ApiResource Pokemon
        {
            get;
            private set;
        }
        public string Text
        {
            get;
            private set;
        }

        private Description() { }

        protected override void Create(JsonData source)
        {
            Games = source["games"].Map<JsonData, ApiResource>(ParseResource).ToArray();

            Pokemon = ParseResource(source["pokemon"]);
            Text = (string)source["description"];
        }

        public async Task<Game> RefGame(int index) => await Game.GetInstance(Games[index].Name);

        public static async Task<Description> GetInstance(int id) => await cache.Get(id);

        public static implicit operator string (Description descr) => descr.Text;
    }
}
