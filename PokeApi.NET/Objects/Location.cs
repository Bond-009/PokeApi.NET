﻿using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;

namespace PokeAPI
{
    public struct EncounterVersionDetails
    {
        public int Rate
        {
            get;
            internal set;
        }
        public NamedApiResource<GameVersion>[] Versions
        {
            get;
            internal set;
        }
    }
    public struct EncounterMethodRate
    {
        public NamedApiResource<EncounterMethod>[] EncounterMethod
        {
            get;
            internal set;
        }

        public EncounterVersionDetails[] VersionDetails
        {
            get;
            internal set;
        }
    }

    public struct PokemonEncounter
    {
        public NamedApiResource<Pokemon> Pokemon
        {
            get;
            internal set;
        }

        [JsonPropertyName("version_details")]
        public VersionEncounterDetail[] VersionDetails
        {
            get;
            internal set;
        }
    }

    public struct PalParkEncounterSpecies
    {
        [JsonPropertyName("base_score")]
        public int BaseScore
        {
            get;
            internal set;
        }
        public int Rate
        {
            get;
            internal set;
        }

        [JsonPropertyName("pokemon_species")]
        public NamedApiResource<PokemonSpecies> Species
        {
            get;
            internal set;
        }
    }

    public class Location : NamedApiObject
    {
        public NamedApiResource<Region> Region
        {
            get;
            internal set;
        }

        public ResourceName[] Names
        {
            get;
            internal set;
        }

        [JsonPropertyName("game_indices")]
        public GenerationGameIndex[] GameIndices
        {
            get;
            internal set;
        }

        public NamedApiResource<LocationArea>[] Areas
        {
            get;
            internal set;
        }
    }

    public class LocationArea : NamedApiObject
    {
        [JsonPropertyName("game_indices")]
        public int GameIndex
        {
            get;
            internal set;
        }

        [JsonPropertyName("encounter_method_rates")]
        public EncounterMethodRate[] EncounterMethodRates
        {
            get;
            internal set;
        }

        public NamedApiResource<Region> Region
        {
            get;
            internal set;
        }

        [JsonPropertyName("pokemon_encounters")]
        public PokemonEncounter[] Encounters
        {
            get;
            internal set;
        }
    }

    public class PalParkArea : NamedApiObject
    {
        public ResourceName[] Names
        {
            get;
            internal set;
        }

        [JsonPropertyName("pokemon_encounters")]
        public PalParkEncounterSpecies[] Encounters
        {
            get;
            internal set;
        }
    }

    public class Region : NamedApiObject
    {
        public NamedApiResource<Location>[] Locations
        {
            get;
            internal set;
        }
        [JsonPropertyName("main_generation")]
        public NamedApiResource<Generation> MainGeneration
        {
            get;
            internal set;
        }

        public ResourceName[] Names
        {
            get;
            internal set;
        }

        [JsonPropertyName("pokedexes")]
        public NamedApiResource<Pokedex>[] Pokedices
        {
            get;
            internal set;
        }

        [JsonPropertyName("version_groups")]
        public NamedApiResource<VersionGroup>[] VersionGroups
        {
            get;
            internal set;
        }
    }
}
