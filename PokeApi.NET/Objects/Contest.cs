using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;

namespace PokeAPI
{
    public class ContestType : NamedApiObject
    {
        [JsonPropertyName("berry_flavor")]
        public NamedApiResource<BerryFlavor> BerryFlavor
        {
            get;
            internal set;
        }

        public ResourceName[] Names
        {
            get;
            internal set;
        }
    }
    public class ContestEffect : ApiObject
    {
        public int Appeal
        {
            get;
            internal set;
        }

        public int Jam
        {
            get;
            internal set;
        }

        [JsonPropertyName("effect_entries")]
        public Effect[] Effects
        {
            get;
            internal set;
        }
        [JsonPropertyName("flavor_text_entries")]
        public FlavorText[] FlavorTexts
        {
            get;
            internal set;
        }
    }
    public class SuperContestEffect : ApiObject
    {
        public int Appeal
        {
            get;
            internal set;
        }

        [JsonPropertyName("flavor_text_entries")]
        public FlavorText[] FlavorTexts
        {
            get;
            internal set;
        }

        public NamedApiResource<Move>[] Moves
        {
            get;
            internal set;
        }
    }
}
