using System;
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
        }
        public NamedApiResource<GameVersion>[] Versions
        {
            get;
        }
    }
    public struct EncounterMethodRate
    {
        public NamedApiResource<EncounterMethod>[] EncounterMethod
        {
            get;
        }

        public EncounterVersionDetails[] VersionDetails
        {
            get;
        }
    }

    public struct PokemonEncounter
    {
        public NamedApiResource<Pokemon> Pokemon
        {
            get;
        }

        [JsonPropertyName("version_details")]
        public VersionEncounterDetail[] VersionDetails
        {
            get;
        }
    }

    public struct PalParkEncounterSpecies
    {
        [JsonPropertyName("base_score")]
        public int BaseScore
        {
            get;
        }
        public int Rate
        {
            get;
        }

        [JsonPropertyName("pokemon_species")]
        public NamedApiResource<PokemonSpecies> Species
        {
            get;
        }
    }

    public class Location : NamedApiObject
    {
        public NamedApiResource<Region> Region
        {
            get;
        }

        public ResourceName[] Names
        {
            get;
        }

        [JsonPropertyName("game_indices")]
        public GenerationGameIndex[] GameIndices
        {
            get;
        }

        public NamedApiResource<LocationArea>[] Areas
        {
            get;
        }
    }

    public class LocationArea : NamedApiObject
    {
        [JsonPropertyName("game_indices")]
        public int GameIndex
        {
            get;
        }

        [JsonPropertyName("encounter_method_rates")]
        public EncounterMethodRate[] EncounterMethodRates
        {
            get;
        }

        public NamedApiResource<Region> Region
        {
            get;
        }

        [JsonPropertyName("pokemon_encounters")]
        public PokemonEncounter[] Encounters
        {
            get;
        }
    }

    public class PalParkArea : NamedApiObject
    {
        public ResourceName[] Names
        {
            get;
        }

        [JsonPropertyName("pokemon_encounters")]
        public PalParkEncounterSpecies[] Encounters
        {
            get;
        }
    }

    public class Region : NamedApiObject
    {
        public NamedApiResource<Location>[] Locations
        {
            get;
        }
        [JsonPropertyName("main_generation")]
        public NamedApiResource<Generation> MainGeneration
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

        [JsonPropertyName("version_groups")]
        public NamedApiResource<VersionGroup>[] VersionGroups
        {
            get;
        }
    }
}
