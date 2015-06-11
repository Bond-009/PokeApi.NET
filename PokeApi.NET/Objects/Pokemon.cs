using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using LitJson;

namespace PokeAPI
{
    public partial class Pokemon : ApiObject<Pokemon>
    {
        static Cache<int, Pokemon> cache = new Cache<int, Pokemon>(async i => Maybe.Just(Create(await DataFetcher.GetPokemon(i), new Pokemon())));

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

        public ApiResource[] Abilities
        {
            get;
            private set;
        }
        public ApiResource[] EggGroups
        {
            get;
            private set;
        }
        public Evolution[] Evolutions
        {
            get;
            private set;
        }
        public LearnableMove[] Moves
        {
            get;
            private set;
        }
        public ApiResource[] Descriptions
        {
            get;
            private set;
        }

        public string Species
        {
            get;
            private set;
        }
        public string GrowthRate
        {
            get;
            private set;
        }

        public int HP
        {
            get;
            private set;
        }
        public int Attack
        {
            get;
            private set;
        }
        public int Defense
        {
            get;
            private set;
        }
        public int CatchRate
        {
            get;
            private set;
        }
        public int SpecialAttack
        {
            get;
            private set;
        }
        public int SpecialDefense
        {
            get;
            private set;
        }
        public int Speed
        {
            get;
            private set;
        }
        public int EggCycles
        {
            get;
            private set;
        }
        public int Height
        {
            get;
            private set;
        }
        public int Mass
        {
            get;
            private set;
        }
        public int BaseHappiness
        {
            get;
            private set;
        }

        public EvYield EvYield
        {
            get;
            private set;
        }
        public int? ExpYield
        {
            get;
            private set;
        }

        public TypeFlags Type
        {
            get;
            private set;
        }

        public Tuple<double, double> MaleFemaleRatio
        {
            get;
            private set;
        }

        private Pokemon() { }

        protected override void Create(JsonData source)
        {
            Id = (int)source["national_id"];

            Abilities = source["abilities"].Map<JsonData, ApiResource>(ParseResource).ToArray();
            EggGroups = source["egg_groups"].Map<JsonData, ApiResource>(ParseResource).ToArray();
            Descriptions = source["descriptions"].Map<JsonData, ApiResource>(ParseResource).ToArray();
            Moves = source["moves"].Map<JsonData, LearnableMove>(LearnableMove.Parse).OrderBy(t => t.Id).ToArray();

            if (source.Keys.Contains("evolutions"))
                Evolutions = source["evolutions"].Map<JsonData, Evolution>(data => new Evolution(data)).ToArray();
            else
                Evolutions = new Evolution[0];

            Type = source["types"].Map<JsonData, ApiResource>(ParseResource).Select(r => (TypeFlags)r.Id).Aggregate((a, b) => a | b);

            CatchRate = (int)source["catch_rate"];
            Species = source["species"].ToString();
            HP = (int)source["hp"];
            Attack = (int)source["attack"];
            Defense = (int)source["defense"];
            SpecialAttack = (int)source["sp_atk"];
            SpecialDefense = (int)source["sp_def"];
            Speed = (int)source["speed"];
            EggCycles = (int)source["egg_cycles"];
            EvYield = EvYield.Parse(source);
            ExpYield = source.AsNullInt("exp");
            GrowthRate = source["growth_rate"].ToString();
            Height = source.AsInt("height");
            Mass = source.AsInt("weight");
            BaseHappiness = (int)source["happiness"];


            MaleFemaleRatio = String.IsNullOrEmpty(source["male_female_ratio"].ToString())
                ? new Tuple<double, double>(Double.NaN, Double.NaN)
                : new Tuple<double, double>
            (
                Convert.ToDouble(source["male_female_ratio"].ToString().Split('/')[0], CultureInfo.InvariantCulture) / 100d,
                Convert.ToDouble(source["male_female_ratio"].ToString().Split('/')[1], CultureInfo.InvariantCulture) / 100d
            );
        }

        public async Task<Ability> RefAbility(int index) => await Ability.GetInstance(Abilities[index].Name);
        public async Task<EggGroup> RefEggGroup(int index) => await EggGroup.GetInstance(EggGroups[index].Name);
        public async Task<Move> RefMove(int index) => await Move.GetInstance(Moves[index].Id);
        public async Task<Description> RefDescription(int index) => await Description.GetInstance(Descriptions[index].Id);

        public static async Task<Pokemon> GetInstance(string name) => await GetInstance(IDs[name.ToLowerInvariant()]);
        public static async Task<Pokemon> GetInstance(int id) => await cache.Get(id);
    }
}
