using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;

namespace PokeAPI
{
    public struct PokemonEntry
    {
        [JsonPropertyName("entry_number")]
        public int EntryNumber
        {
            get;
        }
        [JsonPropertyName("pokemon_species")]
        public NamedApiResource<PokemonSpecies> Species
        {
            get;
        }
    }

    public class Generation : NamedApiObject
    {
        public ResourceName[] Names
        {
            get;
        }

        [JsonPropertyName("main_region")]
        public NamedApiResource<Region> MainRegion
        {
            get;
        }

        [JsonPropertyName("new_abilities")]
        public NamedApiResource<Ability>[] NewAbilities
        {
            get;
        }
        [JsonPropertyName("new_moves")]
        public NamedApiResource<Move>[] NewMoves
        {
            get;
        }
        [JsonPropertyName("new_species")]
        public NamedApiResource<PokemonSpecies>[] NewSpeices
        {
            get;
        }
        [JsonPropertyName("new_types")]
        public NamedApiResource<PokemonType>[] NewTypes
        {
            get;
        }
        [JsonPropertyName("version_groups")]
        public NamedApiResource<VersionGroup>[] VersionGroups
        {
            get;
        }
    }

    public class Pokedex : NamedApiObject
    {
        [JsonPropertyName("is_main_series")]
        public bool IsMainSeries
        {
            get;
        }

        public Description[] Descriptions
        {
            get;
        }

        public ResourceName[] Names
        {
            get;
        }

        [JsonPropertyName("pokemon_entries")]
        public PokemonEntry[] Entries
        {
            get;
        }

        public NamedApiResource<Region> Region
        {
            get;
        }

        public NamedApiResource<VersionGroup>[] VersionGroups
        {
            get;
        }
    }

    public class GameVersion : NamedApiObject
    {
        public ResourceName[] Names
        {
            get;
        }

        [JsonPropertyName("version_group")]
        public NamedApiResource<VersionGroup> VersionGroup
        {
            get;
        }
    }

    public class VersionGroup : NamedApiObject
    {
        public int Order
        {
            get;
        }

        public NamedApiResource<Generation> Generation
        {
            get;
        }

        [JsonPropertyName("move_learn_methods")]
        public NamedApiResource<MoveLearnMethod>[] MoveLearnMethods
        {
            get;
        }

        public ResourceName[] Names
        {
            get;
        }

        [JsonPropertyName("pokedexes")]
        public NamedApiResource<Pokedex>[] Pokedices
        {
            get;
        }

        public NamedApiResource<Region>[] Regions
        {
            get;
        }

        public NamedApiResource<GameVersion>[] Versions
        {
            get;
        }
    }
}
