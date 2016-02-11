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
        readonly static string
            NID = "national_id",
            ABS = "abilities",
            EGS = "egg_groups",
            DSS = "descriptions",
            MVS = "moves",
            EVS = "evolutions",
            TPS = "types",
            CRT = "catch_rate",
            SCS = "species",
            HP_ = "hp",
            ATK = "attack",
            DEF = "defense",
            SAT = "sp_atk",
            SDF = "sp_def",
            SPD = "speed",
            EGC = "egg_cycles",
            EXP = "exp",
            GRT = "growth_rate",
            HT  = "height",
            MS  = "weight", // let's be technically correct
            BHP = "happiness",
            MFR = "male_female_ratio";

        readonly static Evolution[] EmptyEvoArr = { };
        readonly static Tuple<double, double> NanPair = Tuple.Create(Double.NaN, Double.NaN);

        static readonly Cache<int, Pokemon> cache = new Cache<int, Pokemon>(async i => Maybe.Just(Create(await DataFetcher.GetPokemon(i), new Pokemon())));

        /// <summary>
        /// Gets the <see cref="Pokemon" /> instance cache.
        /// </summary>
        public static CacheGetter<int, Pokemon> Cache { get; } = new CacheGetter<int, Pokemon>(cache);

        /// <summary>
        /// Gets the abilities this Pokemon can have.
        /// </summary>
        public ApiResource[] Abilities
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the egg groups this Pokemon is in.
        /// </summary>
        public ApiResource[] EggGroups
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the evolutions this Pokemon can evolve into.
        /// </summary>
        public Evolution[] Evolutions
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the moves this Pokemon can learn.
        /// </summary>
        public LearnableMove[] Moves
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the pokedex descriptions this Pokemon has.
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
        /// Gets the growth rate of this Pokemon.
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
        /// Gets the Pokemon's catch rate.
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
        /// Gets the number of egg cycles needed.
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
        /// Ges the base happiness for this Pokemon.
        /// </summary>
        public int BaseHappiness
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the EV yield for this Pokemon.
        /// </summary>
        public EvYield EvYield
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Exp. yield from this Pokemon.
        /// </summary>
        public int? ExpYield
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the types this Pokemon is.
        /// </summary>
        public TypeFlags Type
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Male/Female ratio.
        /// </summary>
        public Tuple<double, double> MaleFemaleRatio
        {
            get;
            private set;
        }

        Pokemon()
        {
        }

        /// <summary>
        /// Does parsing stuff in the derived class.
        /// </summary>
        /// <param name="source">The JSON data to parse.</param>
        protected override void Create(JsonData source)
        {
            Id = (int)source[NID];

            Abilities    = source[ABS].Map<JsonData, ApiResource>(ParseResource).ToArray();
            EggGroups    = source[EGS].Map<JsonData, ApiResource>(ParseResource).ToArray();
            Descriptions = source[DSS].Map<JsonData, ApiResource>(ParseResource).ToArray();

            Moves        = source[MVS].Map<JsonData, LearnableMove>(LearnableMove.Parse).OrderBy(t => t.Level).ToArray();

            Evolutions = source.Keys.Contains(EVS)
                ? source[EVS].Map<JsonData, Evolution>(data => new Evolution(data)).ToArray() : EmptyEvoArr;
            
            Type = source[TPS].Map<JsonData, ApiResource>(ParseResource).Select(r => (TypeFlags)(1 << r.Id - 1)).Aggregate((a, b) => a | b);
            
            Species = source[SCS].ToString();

            CatchRate      = source.AsInt(CRT);
            HP             = source.AsInt(HP_);
            Attack         = source.AsInt(ATK);
            Defense        = source.AsInt(DEF);
            SpecialAttack  = source.AsInt(SAT);
            SpecialDefense = source.AsInt(SDF);
            Speed          = source.AsInt(SPD);
            EggCycles      = source.AsInt(EGC);
            Height         = source.AsInt(HT );
            Mass           = source.AsInt(MS );

            EvYield = EvYield.Parse(source);

            ExpYield      = source.AsNullInt(EXP);
            GrowthRate    = source[GRT].ToString();
            BaseHappiness = (int)source[BHP];

            var mfr = source[MFR].ToString();
            var split = mfr.Split('/');

            MaleFemaleRatio = String.IsNullOrEmpty(mfr) ? NanPair
                : Tuple.Create(Double.Parse(split[0], CultureInfo.InvariantCulture) / 100d,
                               Double.Parse(split[1], CultureInfo.InvariantCulture) / 100d);
        }

        /// <summary>
        /// Gets the <see cref="Ability" /> instance represented by <see cref="Abilities" /> asynchronously.
        /// </summary>
        /// <param name="index">The array element index.</param>
        /// <returns>A task containing the <see cref="Ability" />.</returns>
        public async Task<Ability> RefAbility(int index) => await Ability.GetInstanceAsync(Abilities[index].Name);
        /// <summary>
        /// Gets the <see cref="EggGroup" /> instance represented by <see cref="EggGroups" /> asynchronously.
        /// </summary>
        /// <param name="index">The array element index.</param>
        /// <returns>A task containing the <see cref="EggGroup" />.</returns>
        public async Task<EggGroup> RefEggGroup(int index) => await EggGroup.GetInstanceAsync(EggGroups[index].Name);
        /// <summary>
        /// Gets the <see cref="Move" /> instance represented by <see cref="Moves" /> asynchronously.
        /// </summary>
        /// <param name="index">The array element index.</param>
        /// <returns>A task containing the <see cref="Move" />.</returns>
        public async Task<Move> RefMove(int index) => await Move.GetInstanceAsync(Moves[index].Id);
        /// <summary>
        /// Gets the <see cref="Description" /> instance represented by <see cref="Descriptions" /> asynchronously.
        /// </summary>
        /// <param name="index">The array element index.</param>
        /// <returns>A task containing the <see cref="Description" />.</returns>
        public async Task<Description> RefDescription(int index) => await Description.GetInstanceAsync(Descriptions[index].Id);

        /// <summary>
        /// Returns an instance of the Pokemon by name.
        /// </summary>
        /// <example>
        /// You can get a pokemon using it the following way:
        /// <code>
        /// var bulbasaur = Pokemon.GetInstanceAsync("bulbasaur");
        /// </code>
        /// </example>
        /// <param name="name">The name of the Pokemon to get. The search is case insensitive.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static async Task<Pokemon> GetInstanceAsync(string name) => await GetInstanceAsync(Ids[name.ToLowerInvariant()]);
        /// <summary>
        /// Returns an instance of the Pokemon by id.
        /// </summary>
        /// <example>
        /// You can get a pokemon using it the following way:
        /// <code>
        /// var bulbasaur = Pokemon.GetInstanceAsync(1);
        /// </code>
        /// </example>
        /// <param name="id">The id of the Pokemon to get.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static async Task<Pokemon> GetInstanceAsync(int    id  ) => await cache.Get(id);
    }
}
