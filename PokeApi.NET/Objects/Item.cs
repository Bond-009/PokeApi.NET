using System;
using System.Collections.Generic;
using System.Linq;

namespace PokeAPI
{
    public class Item : NamedApiObject
    {
        public int Cost
        {
            get;
        }

        public int FlingPower
        {
            get;
        }

        public NamedApiResource<ItemFlingEffect> FlingEffect
        {
            get;
        }

        public NamedApiResource<ItemAttribute> Attributes
        {
            get;
        }

        public NamedApiResource<ItemCategory> Category
        {
            get;
        }

        public VerboseEffect[] Effects
        {
            get;
        }

        public VersionGroupFlavorText[] FlavorTexts
        {
            get;
        }

        public GenerationGameIndex[] GameIndices
        {
            get;
        }

        public ResourceName[] Names
        {
            get;
        }

        public NamedApiResource<Pokemon>[] HeldBy
        {
            get;
        }

        public ApiResource<EvolutionChain>[] BabyTriggerFor
        {
            get;
        }
    }

    public class ItemAttribute : NamedApiObject
    {
        public NamedApiResource<Item>[] Items
        {
            get;
        }

        public ResourceName[] Names
        {
            get;
        }

        public Description[] Descriptions
        {
            get;
        }
    }

    public class ItemCategory : NamedApiObject
    {
        public NamedApiResource<Item>[] Items
        {
            get;
        }

        public ResourceName[] Names
        {
            get;
        }

        public NamedApiResource<ItemPocket> Pocket
        {
            get;
        }
    }

    public class ItemFlingEffect : NamedApiObject
    {
        public Effect[] Effects
        {
            get;
        }

        public NamedApiResource<Item>[] Items
        {
            get;
        }
    }

    public class ItemPocket : NamedApiObject
    {
        public NamedApiResource<ItemCategory>[] Categories
        {
            get;
        }

        public ResourceName[] Names
        {
            get;
        }
    }
}
