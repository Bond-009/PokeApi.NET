using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;

namespace PokeAPI.NET
{
    /// <summary>
    /// Represents an instance of a Pokémon Egg Group
    /// </summary>
    public class EggGroup : PokeApiType
    {
        /// <summary>
        /// Wether it should cache egg group data or not
        /// </summary>
        public static bool ShouldCacheData = true;
        /// <summary>
        /// Gets all cached egg groups
        /// </summary>
        public static Dictionary<int, EggGroup> CachedEggGroups = new Dictionary<int, EggGroup>();

        #region public readonly static Dictionary<string, int> IDs = new Dictionary<string, int>() { [...] };
        /// <summary>
        /// The egg group string->ID maps
        /// </summary>
        public readonly static Dictionary<string, int> IDs = new Dictionary<string, int>()
        {
            {"monster", 1},
            {"water1", 2},
            {"bug", 3},
            {"flying", 4},
            {"ground", 5},
            {"fairy", 6},
            {"plant", 7},
            {"humanlike", 8},
            {"human like", 8},
            {"human-like", 8},
            {"water3", 9},
            {"mineral", 10},
            {"indeterminate", 11},
            {"water2", 12},
            {"ditto", 13},
            {"dragon", 14},
            {"undiscovered", 15}
        };
        #endregion

        /// <summary>
        /// A list of all the Pokemon in this EggGroup
        /// </summary>
        public List<NameUriPair> Pokemon
        {
            get;
            private set;
        } = new List<NameUriPair>();

        /// <summary>
        /// Gets an entry of the Pokemon list as a Pokemon
        /// </summary>
        /// <param name="index">The index of the entry</param>
        /// <returns>The entry of the Pokemon list as a Pokemon</returns>
        public Pokemon RefPokemon(int index)
        {
            return NET.Pokemon.GetInstance(Pokemon[index].Name);
        }

        /// <summary>
        /// Creates a new instance from a JSON object
        /// </summary>
        /// <param name="source">The JSON object where to create the new instance from</param>
        protected override void Create(JsonData source)
        {
            foreach (JsonData data in source["pokemon"])
                Pokemon.Add(ParseNameUriPair(data));
        }

        /// <summary>
        /// Creates an instance of a EggGroup with the given EggGroup
        /// </summary>
        /// <param name="eggGroup">The EggGroup of the EggGroup to instantiate</param>
        /// <returns>The created instance of the EggGroup</returns>
        public static EggGroup GetInstance(EggGroupID eggGroup)
        {
            return GetInstance((int)eggGroup);
        }
        /// <summary>
        /// Creates an instance of a EggGroup with the given name
        /// </summary>
        /// <param name="name">The name of the EggGroup to instantiate</param>
        /// <returns>The created instance of the EggGroup</returns>
        public static EggGroup GetInstance(string name)
        {
            return GetInstance(IDs[name.ToLower()]);
        }
        /// <summary>
        /// Creates an instance of a EggGroup with the given ID
        /// </summary>
        /// <param name="id">The id of the EggGroup to instantiate</param>
        /// <returns>The created instance of the EggGroup</returns>
        public static EggGroup GetInstance(int id)
        {
            if (CachedEggGroups.ContainsKey(id))
                return CachedEggGroups[id];

            EggGroup p = new EggGroup();
            Create(DataFetcher.GetEggGroup(id), p);

            return p;
        }

        /// <summary>
        /// Converts a EggGroup to an EggGroup
        /// </summary>
        /// <param name="eggGroup">The EggGroup to convert from</param>
        public static implicit operator EggGroupID(EggGroup eggGroup)
        {
            return (EggGroupID)(eggGroup.ID + 1);
        }
        /// <summary>
        /// Converts an EggGroup to a EggGroup
        /// </summary>
        /// <param name="eggGroup">The EggGroup to convert from</param>
        public static explicit operator EggGroup(EggGroupID eggGroup)
        {
            return GetInstance(eggGroup);
        }
    }
}
