using System;
using System.Collections.Generic;
using System.Linq;

namespace PokeAPI
{
    partial class Game
    {
        #region public readonly static IDictionary<string, int> IDs = new Dictionary<string, int>() { [...] };
        public readonly static IDictionary<string, int> Ids = new Dictionary<string, int>()
        {
            {"red (jpn)", 1},
            {"green (jpn)", 2},
            {"blue (jpn)", 3},
            {"red", 4},
            {"blue", 5},
            {"yellow", 6},
            {"gold", 7},
            {"silver", 8},
            {"crystal", 9},
            {"ruby", 10},
            {"sapphire", 11},
            {"firered", 12},
            {"leafgreen", 13},
            {"emerald", 14},
            {"diamond", 15},
            {"pearl", 16},
            {"platinum", 17},
            {"heartgold", 18},
            {"soulsilver", 19},
            {"black", 20},
            {"white", 21},
            {"black 2", 22},
            {"white 2", 23},
            {"x", 24},
            {"y", 25}
        };
        #endregion
    }
}
