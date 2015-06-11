using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LitJson;

namespace PokeAPI
{
    public partial class Game : ApiObject<Game>
    {
        static Cache<int, Game> cache = new Cache<int, Game>(async i => Maybe.Just(Create(await DataFetcher.GetGame(i), new Game())));

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

        public int ReleaseYear
        {
            get;
            private set;
        }
        public int Generation
        {
            get;
            private set;
        }

        private Game() { }

        protected override void Create(JsonData source)
        {
            ReleaseYear = (int)source["release_year"];
            Generation  = (int)source["generation"  ];
        }

        public static async Task<Game> GetInstance(GameID game) => await GetInstance((int)game);
        public static async Task<Game> GetInstance(string name) => await GetInstance(IDs[name.ToLowerInvariant()]);
        public static async Task<Game> GetInstance(int id) => await cache.Get(id);

        public static implicit operator GameID(Game game) => (GameID)(game.Id - 1);
    }
}
