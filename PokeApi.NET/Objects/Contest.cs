using System;
using System.Collections.Generic;
using System.Linq;

namespace PokeAPI
{
    public class ContestType : NamedApiObject
    {
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

        public Effect[] Effects
        {
            get;
        }
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
