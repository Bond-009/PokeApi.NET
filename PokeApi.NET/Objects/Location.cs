using System;
using System.Collections.Generic;
using System.Linq;

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

        public VersionEncounterDetail[] VersionDetails
        {
            get;
        }
    }

    public struct PalParkEncounterSpecies
    {
        public int BaseScore
        {
            get;
        }
        public int Rate
        {
            get;
        }
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
        public int GameIndex
        {
            get;
        }

        public EncounterMethodRate[] EncounterMethodRates
        {
            get;
        }

        public NamedApiResource<Region> Region
        {
            get;
        }

        public PokemonEncounter[] PokemonEncounters
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
        public NamedApiResource<Generation> MainGeneration
        {
            get;
        }

        public ResourceName[] Names
        {
            get;
        }

        public NamedApiResource<Pokedex>[] Pokedices
        {
            get;
        }

        public NamedApiResource<VersionGroup>[] VersionGroups
        {
            get;
        }
    }
}
