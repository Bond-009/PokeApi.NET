using System;
using System.Collections.Generic;
using System.Linq;

namespace PokeAPI
{
    public struct PokemonEntry
    {
        public int EntryNumber
        {
            get;
        }
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

        public NamedApiResource<Region> MainRegion
        {
            get;
        }

        public NamedApiResource<Ability>[] NewAbilities
        {
            get;
        }
        public NamedApiResource<Move>[] NewMoves
        {
            get;
        }
        public NamedApiResource<PokemonSpecies>[] NewSpeices
        {
            get;
        }
        public NamedApiResource<PokemonType>[] NewTypes
        {
            get;
        }
        public NamedApiResource<VersionGroup>[] VersionGroups
        {
            get;
        }
    }

    public class Pokedex : NamedApiObject
    {
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

        public NamedApiResource<MoveLearnMethod>[] MoveLearnMethods
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
