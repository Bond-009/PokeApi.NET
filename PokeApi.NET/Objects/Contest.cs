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
        }

        public ResourceName[] Names
        {
            get;
        }
    }
    public class ContestEffect : ApiObject
    {
        public int Appeal
        {
            get;
        }

        public int Jam
        {
            get;
        }

        [JsonPropertyName("effect_entries")]
        public Effect[] Effects
        {
            get;
        }
        [JsonPropertyName("flavor_text_entries")]
        public FlavorText[] FlavorTexts
        {
            get;
        }
    }
    public class SuperContestEffect : ApiObject
    {
        public int Appeal
        {
            get;
        }

        [JsonPropertyName("flavor_text_entries")]
        public FlavorText[] FlavorTexts
        {
            get;
        }

        public NamedApiResource<Move>[] Moves
        {
            get;
        }
    }
}
