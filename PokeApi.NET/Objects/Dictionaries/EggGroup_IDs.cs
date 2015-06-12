using System;
using System.Collections.Generic;
using System.Linq;

namespace PokeAPI
{
    partial class EggGroup
    {
        #region public readonly static IDictionary<string, int> IDs = new Dictionary<string, int>() { [...] };
        public readonly static IDictionary<string, int> Ids = new Dictionary<string, int>()
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
    }
}
