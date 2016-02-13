using System;
using System.Collections.Generic;
using System.Linq;

namespace PokeAPI
{
    public class EncounterMethod : NamedApiObject
    {
        public int Order
        {
            get;
        }

        public ResourceName[] Names
        {
            get;
        }
    }

    public class EncounterCondition : NamedApiObject
    {
        public ResourceName[] Names
        {
            get;
        }

        public NamedApiResource<EncounterConditionValue>[] Values
        {
            get;
        }
    }

    public class EncounterConditionValue : NamedApiObject
    {
        public NamedApiResource<EncounterCondition>[] Conditions
        {
            get;
        }

        public ResourceName[] Names
        {
            get;
        }
    }
}
