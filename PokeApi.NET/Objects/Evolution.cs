using System;
using System.Collections.Generic;
using System.Linq;

namespace PokeAPI
{
    public struct EvolutionDetail
    {
        public NamedApiResource<Item> Item
        {
            get;
        }

        public NamedApiResource<EvolutionTrigger> Trigger
        {
            get;
        }

        public NamedApiResource<Gender> Gender
        {
            get;
        }

        public NamedApiResource<Item> HeldItem
        {
            get;
        }

        public NamedApiResource<Move> KnownMove
        {
            get;
        }

        public NamedApiResource<PokemonType> KnownMoveType
        {
            get;
        }

        public NamedApiResource<Location> Location
        {
            get;
        }

        public int MinLevel
        {
            get;
        }
        public int MinHappiness
        {
            get;
        }
        public int MinBeauty
        {
            get;
        }
        public int MinAffection
        {
            get;
        }
        public bool NeedsOverworldRain
        {
            get;
        }

        public NamedApiResource<PokemonSpecies> PartySpecies
        {
            get;
        }

        public NamedApiResource<PokemonType> PartyType
        {
            get;
        }

        public int RelativePhysicalStats
        {
            get;
        }

        public TimeOfDay TimeOfDay
        {
            get;
        }

        public NamedApiResource<PokemonSpecies> TradeSpecies
        {
            get;
        }

        public bool TurnUpsideDown
        {
            get;
        }
    }

    public struct ChainLink
    {
        public bool IsBaby
        {
            get;
        }

        public NamedApiResource<PokemonSpecies> Species
        {
            get;
        }

        public EvolutionDetail Details
        {
            get;
        }

        public ChainLink[] EvolvesTo
        {
            get;
        }
    }

    public class EvolutionChain : ApiObject
    {
        public NamedApiResource<Item> BabyTriggerItem
        {
            get;
        }

        public ChainLink Chain
        {
            get;
        }
    }

    public class EvolutionTrigger : NamedApiObject
    {
        public ResourceName[] Names
        {
            get;
        }

        NamedApiResource<PokemonSpecies>[] Species
        {
            get;
        }
    }
}
