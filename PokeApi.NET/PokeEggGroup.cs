using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;

namespace PokeAPI.NET
{
    /// <summary>
    /// Represents an instance of a Pokémon Egg Group
    /// </summary>
    public class PokeEggGroup : PokeApiType
    {
        /// <summary>
        /// Wether it should cache egg group data or not
        /// </summary>
        public static bool ShouldCacheData = true;
        /// <summary>
        /// Gets all cached egg groups
        /// </summary>
        public static Dictionary<int, PokeEggGroup> CachedEggGroups = new Dictionary<int, PokeEggGroup>();

        #region public readonly static Dictionary<string, int> IDs = new Dictionary<string, int>() { [...] };
        /// <summary>
        /// The egg group string->ID maps
        /// </summary>
        public readonly static Dictionary<string, int> IDs = new Dictionary<string, int>()
        {
            {"Monster", 1},
            {"Water1", 2},
            {"Bug", 3},
            {"Flying", 4},
            {"Ground", 5},
            {"Fairy", 6},
            {"Plant", 7},
            {"Human-like", 8},
            {"Water3", 9},
            {"Mineral", 10},
            {"Indeterminate", 11},
            {"Water2", 12},
            {"Ditto", 13},
            {"Dragon", 14},
            {"Undiscovered", 15}
        };
        #endregion

        List<NameUriPair> pokemon = new List<NameUriPair>();
        /// <summary>
        /// A list of all the Pokemon in this PokeEggGroup
        /// </summary>
        public List<NameUriPair> Pokemon
        {
            get
            {
                return pokemon;
            }
        }

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
                pokemon.Add(ParseNameUriPair(data));

            if (ShouldCacheData && !CachedEggGroups.ContainsKey(ID))
                CachedEggGroups.Add(ID, this);
        }

        /// <summary>
        /// Creates an instance of a PokeEggGroup with the given EggGroup
        /// </summary>
        /// <param name="eggGroup">The EggGroup of the PokeEggGroup to instantiate</param>
        /// <returns>The created instance of the PokeEggGroup</returns>
        public static PokeEggGroup GetInstance(EggGroup eggGroup)
        {
            return GetInstance((int)eggGroup + 1);
        }
        /// <summary>
        /// Creates an instance of a PokeEggGroup with the given name
        /// </summary>
        /// <param name="name">The name of the PokeEggGroup to instantiate</param>
        /// <returns>The created instance of the PokeEggGroup</returns>
        public static PokeEggGroup GetInstance(string name)
        {
            return GetInstance(IDs[name]);
        }
        /// <summary>
        /// Creates an instance of a PokeEggGroup with the given ID
        /// </summary>
        /// <param name="id">The id of the PokeEggGroup to instantiate</param>
        /// <returns>The created instance of the PokeEggGroup</returns>
        public static PokeEggGroup GetInstance(int id)
        {
            if (CachedEggGroups.ContainsKey(id))
                return CachedEggGroups[id];

            PokeEggGroup p = new PokeEggGroup();
            Create(DataFetcher.GetEggGroup(id), p);

            if (ShouldCacheData)
                CachedEggGroups.Add(id, p);

            return p;
        }

        /// <summary>
        /// Converts a PokeEggGroup to an EggGroup
        /// </summary>
        /// <param name="eggGroup">The PokeEggGroup to convert from</param>
        public static implicit operator EggGroup(PokeEggGroup eggGroup)
        {
            return (EggGroup)(eggGroup.ID + 1);
        }
        /// <summary>
        /// Converts an EggGroup to a PokeEggGroup
        /// </summary>
        /// <param name="eggGroup">The EggGroup to convert from</param>
        public static explicit operator PokeEggGroup(EggGroup eggGroup)
        {
            return GetInstance(eggGroup);
        }
    }
}
