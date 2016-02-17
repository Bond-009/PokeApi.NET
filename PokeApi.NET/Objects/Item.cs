using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;

namespace PokeAPI
{
    public class Item : NamedApiObject
    {
        public int Cost
        {
            get;
        }

        [JsonPropertyName("fling_power")]
        public int FlingPower
        {
            get;
        }

        [JsonPropertyName("fling_effect")]
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

        [JsonPropertyName("flavor_text_entries")]
        public VersionGroupFlavorText[] FlavorTexts
        {
            get;
        }

        [JsonPropertyName("game_indices")]
        public GenerationGameIndex[] GameIndices
        {
            get;
        }

        public ResourceName[] Names
        {
            get;
        }

        [JsonPropertyName("held_by_pokemon")]
        public NamedApiResource<Pokemon>[] HeldBy
        {
            get;
        }

        [JsonPropertyName("baby_trigger_for")]
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
        [JsonPropertyName("effect_entries")]
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
