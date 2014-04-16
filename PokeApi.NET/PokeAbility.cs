using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;

namespace PokeAPI.NET
{
    /// <summary>
    /// Represents an instance of a Pokémon Ability
    /// </summary>
    public class PokeAbility : PokeApiType
    {
        /// <summary>
        /// Wether it should cache ability data or not
        /// </summary>
        public static bool ShouldCacheData = true;
        /// <summary>
        /// All cached abilities
        /// </summary>
        public static Dictionary<int, PokeAbility> CachedAbilities = new Dictionary<int, PokeAbility>();

        #region public readonly static Dictionary<string, int> IDs = new Dictionary<string, int>() { [...] };
        /// <summary>
        /// All ability string->ID maps
        /// </summary>
        public readonly static Dictionary<string, int> IDs = new Dictionary<string, int>()
        {
            {"Stench", 1},
            {"Drizzle", 2},
            {"Speed boost", 3},
            {"Battle armor", 4},
            {"Sturdy", 5},
            {"Damp", 6},
            {"Limber", 7},
            {"Sand veil", 8},
            {"Static", 9},
            {"Volt absorb", 10},
            {"Water absorb", 11},
            {"Oblivious", 12},
            {"Cloud nine", 13},
            {"Compoundeyes", 14},
            {"Insomnia", 15},
            {"Color change", 16},
            {"Immunity", 17},
            {"Flash fire", 18},
            {"Shield dust", 19},
            {"Own tempo", 20},
            {"Suction cups", 21},
            {"Intimidate", 22},
            {"Shadow tag", 23},
            {"Rough skin", 24},
            {"Wonder guard", 25},
            {"Levitate", 26},
            {"Effect spore", 27},
            {"Synchronize", 28},
            {"Clear body", 29},
            {"Natural cure", 30},
            {"Lightningrod", 31},
            {"Serene grace", 32},
            {"Swift swim", 33},
            {"Chlorophyll", 34},
            {"Illuminate", 35},
            {"Trace", 36},
            {"Huge power", 37},
            {"Poison point", 38},
            {"Inner focus", 39},
            {"Magma armor", 40},
            {"Water veil", 41},
            {"Magnet pull", 42},
            {"Soundproof", 43},
            {"Rain dish", 44},
            {"Sand stream", 45},
            {"Pressure", 46},
            {"Thick fat", 47},
            {"Early bird", 48},
            {"Flame body", 49},
            {"Run away", 50},
            {"Keen eye", 51},
            {"Hyper cutter", 52},
            {"Pickup", 53},
            {"Truant", 54},
            {"Hustle", 55},
            {"Cute charm", 56},
            {"Plus", 57},
            {"Minus", 58},
            {"Forecast", 59},
            {"Sticky hold", 60},
            {"Shed skin", 61},
            {"Guts", 62},
            {"Marvel scale", 63},
            {"Liquid ooze", 64},
            {"Overgrow", 65},
            {"Blaze", 66},
            {"Torrent", 67},
            {"Swarm", 68},
            {"Rock head", 69},
            {"Drought", 70},
            {"Arena trap", 71},
            {"Vital spirit", 72},
            {"White smoke", 73},
            {"Pure power", 74},
            {"Shell armor", 75},
            {"Air lock", 76},
            {"Tangled feet", 77},
            {"Motor drive", 78},
            {"Rivalry", 79},
            {"Steadfast", 80},
            {"Snow cloak", 81},
            {"Gluttony", 82},
            {"Anger point", 83},
            {"Unburden", 84},
            {"Heatproof", 85},
            {"Simple", 86},
            {"Dry skin", 87},
            {"Download", 88},
            {"Iron fist", 89},
            {"Poison heal", 90},
            {"Adaptability", 91},
            {"Skill link", 92},
            {"Hydration", 93},
            {"Solar power", 94},
            {"Quick feet", 95},
            {"Normalize", 96},
            {"Sniper", 97},
            {"Magic guard", 98},
            {"No guard", 99},
            {"Stall", 100},
            {"Technician", 101},
            {"Leaf guard", 102},
            {"Klutz", 103},
            {"Mold breaker", 104},
            {"Super luck", 105},
            {"Aftermath", 106},
            {"Anticipation", 107},
            {"Forewarn", 108},
            {"Unaware", 109},
            {"Tinted lens", 110},
            {"Filter", 111},
            {"Slow start", 112},
            {"Scrappy", 113},
            {"Storm drain", 114},
            {"Ice body", 115},
            {"Solid rock", 116},
            {"Snow warning", 117},
            {"Honey gather", 118},
            {"Frisk", 119},
            {"Reckless", 120},
            {"Multitype", 121},
            {"Flower gift", 122},
            {"Bad dreams", 123},
            {"Pickpocket", 124},
            {"Sheer force", 125},
            {"Contrary", 126},
            {"Unnerve", 127},
            {"Defiant", 128},
            {"Defeatist", 129},
            {"Cursed body", 130},
            {"Healer", 131},
            {"Friend guard", 132},
            {"Weak armor", 133},
            {"Heavy metal", 134},
            {"Light metal", 135},
            {"Multiscale", 136},
            {"Toxic boost", 137},
            {"Flare boost", 138},
            {"Harvest", 139},
            {"Telepathy", 140},
            {"Moody", 141},
            {"Overcoat", 142},
            {"Poison touch", 143},
            {"Regenerator", 144},
            {"Big pecks", 145},
            {"Sand rush", 146},
            {"Wonder skin", 147},
            {"Analytic", 148},
            {"Illusion", 149},
            {"Imposter", 150},
            {"Infiltrator", 151},
            {"Mummy", 152},
            {"Moxie", 153},
            {"Justified", 154},
            {"Rattled", 155},
            {"Magic bounce", 156},
            {"Sap sipper", 157},
            {"Prankster", 158},
            {"Sand force", 159},
            {"Iron barbs", 160},
            {"Zen mode", 161},
            {"Victory star", 162},
            {"Turboblaze", 163},
            {"Teravolt", 164},
            {"Aerilate", 165},
            {"Aroma veil", 166},
            {"Aura break", 167},
            {"Bulletproof", 168},
            {"Cheek pouch", 169},
            {"Competitive", 170},
            {"Dark aura", 171},
            {"Fairy aura", 172},
            {"Flower veil", 173},
            {"Fur coat", 174},
            {"Gale wings", 175},
            {"Gooey", 176},
            {"Grass pelt", 177},
            {"Magician", 178},
            {"Mega launcher", 179},
            {"Parental bond", 180},
            {"Pixilate", 181},
            {"Protean", 182},
            {"Refrigerate", 183},
            {"Stance change", 184},
            {"Strong jaw", 185},
            {"Sweet veil", 186},
            {"Symbiosis", 187},
            {"Tough claws", 188},
            {"Mountaineer", 189},
            {"Wave rider", 190},
            {"Skater", 191},
            {"Thrust", 192},
            {"Perception", 193},
            {"Parry", 194},
            {"Instinct", 195},
            {"Dodge", 196},
            {"Jagged edge", 197},
            {"Frostbite", 198},
            {"Tenacity", 199},
            {"Pride", 200},
            {"Deep sleep", 201},
            {"Power nap", 202},
            {"Spirit", 203},
            {"Warm blanket", 204},
            {"Gulp", 205},
            {"Herbivore", 206},
            {"Sandpit", 207},
            {"Hot blooded", 208},
            {"Medic", 209},
            {"Life force", 210},
            {"Lunchbox", 211},
            {"Nurse", 212},
            {"Melee", 213},
            {"Sponge", 214},
            {"Bodyguard", 215},
            {"Hero", 216},
            {"Last bastion", 217},
            {"Stealth", 218},
            {"Vanguard", 219},
            {"Nomad", 220},
            {"Sequence", 221},
            {"Grass cloak", 222},
            {"Celebrate", 223},
            {"Lullaby", 224},
            {"Calming", 225},
            {"Daze", 226},
            {"Frighten", 227},
            {"Interference", 228},
            {"Mood maker", 229},
            {"Confidence", 230},
            {"Fortune", 231},
            {"Bonanza", 232},
            {"Explode", 233},
            {"Omnipotent", 234},
            {"Share", 235},
            {"Black hole", 236},
            {"Shadow dash", 237},
            {"Sprint", 238},
            {"Disgust", 239},
            {"High rise", 240},
            {"Climber", 241},
            {"Flame boost", 242},
            {"Aqua boost", 243},
            {"Run up", 244},
            {"Conqueror", 245},
            {"Shackle", 246},
            {"Decoy", 247},
            {"Shield", 248}
        };
        #endregion

        string description;

        /// <summary>
        /// The description of this PokeAbility instance
        /// </summary>
        public string Description
        {
            get
            {
                return description;
            }
        }

        /// <summary>
        /// Creates a new instance from a JSON object
        /// </summary>
        /// <param name="source">The JSON object where to create the new instance from</param>
        protected override void Create(JsonData source)
        {
            description = source["description"].ToString();

            if (ShouldCacheData && !CachedAbilities.ContainsKey(ID))
                CachedAbilities.Add(ID, this);
        }

        /// <summary>
        /// Creates an instance of a PokeAbility with the given Ability
        /// </summary>
        /// <param name="ability">The Ability of the PokeAbility to instantiate</param>
        /// <returns>The created instance of the PokeAbility</returns>
        public static PokeAbility GetInstance(Ability ability)
        {
            return GetInstance((int)ability + 1);
        }
        /// <summary>
        /// Creates an instance of a PokeAbility with the given name
        /// </summary>
        /// <param name="name">The name of the PokeAbility to instantiate</param>
        /// <returns>The created instance of the PokeAbility</returns>
        public static PokeAbility GetInstance(string name)
        {
            return GetInstance(IDs[name]);
        }
        /// <summary>
        /// Creates an instance of a PokeAbility with the given ID
        /// </summary>
        /// <param name="id">The id of the PokeAbility to instantiate</param>
        /// <returns>The created instance of the PokeAbility</returns>
        public static PokeAbility GetInstance(int id)
        {
            if (CachedAbilities.ContainsKey(id))
                return CachedAbilities[id];

            PokeAbility p = new PokeAbility();
            Create(DataFetcher.GetAbility(id), p);

            if (ShouldCacheData)
                CachedAbilities.Add(id, p);

            return p;
        }

        /// <summary>
        /// Converts the PokeAbility instance to an Ability
        /// </summary>
        /// <param name="ability">The PokeAbility to convert from</param>
        public static implicit operator Ability(PokeAbility ability)
        {
            // lazy<me>
            Ability ret = 0;
            Enum.TryParse(ability.Name.Replace(' ', '_'), false, out ret);
            return ret;
        }
        /// <summary>
        /// Converts the Ability instance to a PokeAbility
        /// </summary>
        /// <param name="ability">The Ability to convert from</param>
        public static explicit operator PokeAbility(Ability ability)
        {
            return GetInstance(ability);
        }
    }
}
