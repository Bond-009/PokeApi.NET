using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using LitJson;

namespace PokeAPI
{
    /// <summary>
    /// Represents an instance of a Pokémon Move
    /// </summary>
    public partial class Move : ApiObject<Move>
    {
        static Cache<int, Move> cache = new Cache<int, Move>(async i => Maybe.Just(Create(await DataFetcher.GetMove(i), new Move())));

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
        public int Power
        {
            get;
            private set;
        }
        public int PP
        {
            get;
            private set;
        }
        public double Accurracy
        {
            get;
            private set;
        }
        public MoveCategory Category
        {
            get;
            private set;
        }

        private Move() { }

        protected override void Create(JsonData source)
        {
            Power = (int)source["power"];
            PP    = (int)source["pp"   ];

            Description = source["description"].ToString();
            Accurracy = Convert.ToDouble(source["accuracy"].ToString(), CultureInfo.InvariantCulture) / 100d;

            MoveCategory cat;
            Enum.TryParse(source["category"].ToString(), true, out cat);

            Category = cat;
        }

        public static async Task<Move> GetInstance(string name) => await GetInstance(IDs[name]);
        public static async Task<Move> GetInstance(int    id  ) => await cache.Get(id);
    }
}
