using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;

namespace PokeAPI.NET
{
    /// <summary>
    /// Represents an instance of a Pokémon Type
    /// </summary>
    public class PokeType : PokeApiType
    {
        /// <summary>
        /// Wether it should cache types or not
        /// </summary>
        public static bool ShouldCacheData = true;
        /// <summary>
        /// Gets all cached types
        /// </summary>
        public static Dictionary<int, PokeType> CachedTypes = new Dictionary<int, PokeType>();

        #region public readonly static Dictionary<string, int> IDs = new Dictionary<string, int>() { [...] };
        /// <summary>
        /// All pokemon type string->ID maps
        /// </summary>
        public readonly static Dictionary<string, int> IDs = new Dictionary<string, int>()
        { 
            {"Normal", 1},
            {"Fighting", 2},
            {"Flying", 3},
            {"Poison", 4},
            {"Ground", 5},
            {"Rock", 6},
            {"Bug", 7},
            {"Ghost", 8},
            {"Steel", 9},
            {"Fire", 10},
            {"Water", 11},
            {"Grass", 12},
            {"Electric", 13},
            {"Pyschic", 14},
            {"Ice", 15},
            {"Dragon", 16},
            {"Dark", 17},
            {"Fairy", 18},
        };
        #endregion

        List<NameUriPair>
            ineffective = new List<NameUriPair>(),
            noEffect = new List<NameUriPair>(),
            resistance = new List<NameUriPair>(),
            superEffective = new List<NameUriPair>(),
            weakness = new List<NameUriPair>();

        /// <summary>
        /// The types this PokeType instance is ineffective against
        /// </summary>
        public List<NameUriPair> Ineffective
        {
            get
            {
                return ineffective;
            }
        }
        /// <summary>
        /// The types this PokeType instance has no effect against
        /// </summary>
        public List<NameUriPair> NoEffect
        {
            get
            {
                return noEffect;
            }
        }
        /// <summary>
        /// The types this PokeType instance is resistant to
        /// </summary>
        public List<NameUriPair> Resistance
        {
            get
            {
                return resistance;
            }
        }
        /// <summary>
        /// The types this PokeType instance is super effective against
        /// </summary>
        public List<NameUriPair> SuperEffective
        {
            get
            {
                return superEffective;
            }
        }
        /// <summary>
        /// The types this PokeType instance is weak to
        /// </summary>
        public List<NameUriPair> Weakness
        {
            get
            {
                return weakness;
            }
        }

        /// <summary>
        /// Gets an entry of the Ineffective list as a PokeType
        /// </summary>
        /// <param name="index">The index of the entry</param>
        /// <returns>The entry of the Ineffective list as a PokeType</returns>
        public PokeType RefIneffective(int index)
        {
            return GetInstance(Ineffective[index].Name);
        }
        /// <summary>
        /// Gets an entry of the NoEffect list as a PokeType
        /// </summary>
        /// <param name="index">The index of the entry</param>
        /// <returns>The entry of the NoEffect list as a PokeType</returns>
        public PokeType RefNoEffect(int index)
        {
            return GetInstance(NoEffect[index].Name);
        }
        /// <summary>
        /// Gets an entry of the Resistance list as a PokeType
        /// </summary>
        /// <param name="index">The index of the entry</param>
        /// <returns>The entry of the Resistance list as a PokeType</returns>
        public PokeType RefResistance(int index)
        {
            return GetInstance(Resistance[index].Name);
        }
        /// <summary>
        /// Gets an entry of the SuperEffective list as a PokeType
        /// </summary>
        /// <param name="index">The index of the entry</param>
        /// <returns>The entry of the SuperEffective list as a PokeType</returns>
        public PokeType RefSuperEffective(int index)
        {
            return GetInstance(SuperEffective[index].Name);
        }
        /// <summary>
        /// Gets an entry of the Weakness list as a PokeType
        /// </summary>
        /// <param name="index">The index of the entry</param>
        /// <returns>The entry of the Weakness list as a PokeType</returns>
        public PokeType RefWeakness(int index)
        {
            return GetInstance(Weakness[index].Name);
        }

        /// <summary>
        /// Creates a new instance from a JSON object
        /// </summary>
        /// <param name="source">The JSON object where to create the new instance from</param>
        protected override void Create(JsonData source)
        {
            foreach (JsonData data in source["ineffective"])
                ineffective.Add(ParseNameUriPair(data));
            foreach (JsonData data in source["no_effect"])
                noEffect.Add(ParseNameUriPair(data));
            foreach (JsonData data in source["resistance"])
                resistance.Add(ParseNameUriPair(data));
            foreach (JsonData data in source["super_effective"])
                superEffective.Add(ParseNameUriPair(data));
            foreach (JsonData data in source["weakness"])
                weakness.Add(ParseNameUriPair(data));

            if (ShouldCacheData && !CachedTypes.ContainsKey(ID))
                CachedTypes.Add(ID, this);
        }

        /// <summary>
        /// Creates an instance of a PokeType with the given name
        /// </summary>
        /// <param name="name">The name of the PokeType to instantiate</param>
        /// <returns>The created instance of the PokeType</returns>
        public static PokeType GetInstance(string name)
        {
            return GetInstance(IDs[name]);
        }
        /// <summary>
        /// Creates an instance of a PokeType with the given PokemonType
        /// </summary>
        /// <param name="type">The type of the PokeType to instantiate</param>
        /// <returns>The created instance of the PokeType</returns>
        public static PokeType GetInstance(PokemonType type)
        {
            return GetInstance(type.ID());
        }
        /// <summary>
        /// Creates an instance of a PokeType with the given ID
        /// </summary>
        /// <param name="id">The id of the PokeType to instantiate</param>
        /// <returns>The created instance of the PokeType</returns>
        public static PokeType GetInstance(int id)
        {
            if (CachedTypes.ContainsKey(id))
                return CachedTypes[id];

            PokeType p = new PokeType();
            Create(DataFetcher.GetType(id), p);

            if (ShouldCacheData)
                CachedTypes.Add(id, p);

            return p;
        }

        /// <summary>
        /// Converts a PokeType into a PokemonType
        /// </summary>
        /// <param name="type">The PokeType to convert from</param>
        public static implicit operator PokemonType(PokeType type)
        {
            // lazy<me>
            PokemonType ret = 0;
            Enum.TryParse(type.Name.Replace(' ', '_'), false, out ret);
            return ret;
        }
        /// <summary>
        /// Converts a PokemonType into a PokeType
        /// </summary>
        /// <param name="type">The PokemonType to convert from</param>
        public static explicit operator PokeType(PokemonType type)
        {
            return GetInstance(type);
        }
    }
}
