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

        /// <summary>
        /// The abilities this pokemon can have
        /// </summary>
        public ApiResource[] Abilities
        {
            get;
            private set;
        }

        /// <summary>
        /// The egg groups this pokemon is in.
        /// </summary>
        public ApiResource[] EggGroups
        {
            get;
            private set;
        }

        /// <summary>
        ///  The evolutions this pokemon can evolve into.
        /// </summary>
        public Evolution[] Evolutions
        {
            get;
            private set;
        }

        /// <summary>
        ///  The moves this pokemon can learn.
        /// </summary>
        public LearnableMove[] Moves
        {
            get;
            private set;
        }

        /// <summary>
        ///  the pokedex descriptions this pokemon has.
        /// </summary>
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

        /// <summary>
        /// The growth rate of this pokemon.
        /// </summary>
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

        /// <summary>
        ///  This pokemon's catch rate.
        /// </summary>
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

        /// <summary>
        /// Number of egg cycles needed.
        /// </summary>
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

        /// <summary>
        /// Base happiness for this pokemon.
        /// </summary>
        public int BaseHappiness
        {
            get;
            private set;
        }

        /// <summary>
        /// The ev yield for this pokemon.
        /// </summary>
        public EvYield EvYield
        {
            get;
            private set;
        }

        /// <summary>
        /// The exp yield from this pokemon.
        /// </summary>
        public int? ExpYield
        {
            get;
            private set;
        }

        /// <summary>
        /// the type this pokemon is.
        /// </summary>
        public TypeFlags Type
        {
            get;
            private set;
        }

        /// <summary>
        /// Male/Female ratio.
        /// </summary>
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

        /// <summary>
        /// Returns an instance of the Pokemon by name.
        
        /// </summary>
        /// <example>
        /// You can get a pokemon using it the following way:
        /// <code>
        /// var bulbasaur = Pokemon.GetInstance("bulbasaur");
        /// </code>
        /// </example>
        /// <param name="name">The name of the Pokemon to retrieve. The search is case insensitive</param>
        /// <returns>Returns System.Threading.Tasks.Task`1.The task object representing the asynchronous
        //     operation.</returns>
        public static async Task<Pokemon> GetInstance(string name) => await GetInstance(IDs[name.ToLowerInvariant()]);

        /// <summary>
        /// Returns an instance of the Pokemon by id.
        /// </summary>
        /// <example>
        /// You can get a pokemon using it the following way:
        /// <code>
        /// var bulbasaur = Pokemon.GetInstance(1);
        /// </code>
        /// </example>
        /// <param name="id">The id of the Pokemon to retrieve</param>
        /// <returns>Returns System.Threading.Tasks.Task`1.The task object representing the asynchronous
        //     operation.</returns>
        public static async Task<Pokemon> GetInstance(int id) => await cache.Get(id);
    }
}
