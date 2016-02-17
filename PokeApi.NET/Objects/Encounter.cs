﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace PokeAPI
{
    public class EncounterMethod : NamedApiObject
    {
        public int Order
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

    public class EncounterCondition : NamedApiObject
    {
        public ResourceName[] Names
        {
            get;
            internal set;
        }

        public NamedApiResource<EncounterConditionValue>[] Values
        {
            get;
            internal set;
        }
    }

    public class EncounterConditionValue : NamedApiObject
    {
        public NamedApiResource<EncounterCondition>[] Conditions
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
}
