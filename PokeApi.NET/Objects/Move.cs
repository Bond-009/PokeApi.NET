using System;
using System.Collections.Generic;
using System.Linq;

namespace PokeAPI
{
    public struct ContestComboDetail
    {
        public NamedApiResource<Move> UseBefore
        {
            get;
        }
        public NamedApiResource<Move> UseAfter
        {
            get;
        }
    }
    public struct ContestComboSet
    {
        public ContestComboDetail[] Normal
        {
            get;
        }
        public ContestComboDetail[] Super
        {
            get;
        }
    }

    public struct MoveMetadata
    {
        public NamedApiResource<MoveAilment> Ailment
        {
            get;
        }
        public NamedApiResource<MoveCategory> Category
        {
            get;
        }

        public int? MinHits
        {
            get;
        }
        public int? MaxHits
        {
            get;
        }
        public int? MinTurns
        {
            get;
        }
        public int? MaxTurns
        {
            get;
        }

        public int DrainRecoil
        {
            get;
        }
        public int Healing
        {
            get;
        }

        public int CritRage
        {
            get;
        }
        public float AilmentChance
        {
            get;
        }
        public float FlinchChance
        {
            get;
        }
        public int StatChance
        {
            get;
        }
    }
    public struct MoveStatChange
    {
        public int Change
        {
            get;
        }

        public NamedApiResource<Stat> Stat
        {
            get;
        }
    }

    public struct PastMoveStatValue
    {
        public float Accuracy
        {
            get;
        }
        public float EffectChance
        {
            get;
        }

        public int Power
        {
            get;
        }
        public int PP
        {
            get;
        }

        public VerboseEffect[] Effects
        {
            get;
        }

        public PokemonType Type
        {
            get;
        }

        public NamedApiResource<VersionGroup> VersionGroup
        {
            get;
        }
    }

    public class Move : NamedApiObject
    {
        public float Accuracy
        {
            get;
        }
        public float EffectChance
        {
            get;
        }

        public int PP
        {
            get;
        }
        public int Priority
        {
            get;
        }
        public int Power
        {
            get;
        }

        public ContestComboSet[] ComboSets
        {
            get;
        }

        public NamedApiResource<ContestType> ContestType
        {
            get;
        }

        public ApiResource<ContestEffect> ContestEffect
        {
            get;
        }

        public NamedApiResource<MoveDamageClass> DamageClass
        {
            get;
        }

        public VerboseEffect[] Effects
        {
            get;
        }

        public AbilityEffectChange[] EffectChanges
        {
            get;
        }

        public NamedApiResource<Generation> generation
        {
            get;
        }

        public MoveMetadata Meta
        {
            get;
        }

        public ResourceName[] Names
        {
            get;
        }

        public PastMoveStatValue[] PastValues
        {
            get;
        }

        public MoveStatChange[] StatChanges
        {
            get;
        }

        public MoveTarget Target
        {
            get;
        }

        public PokemonType Type
        {
            get;
        }
    }

    public class MoveAilment : NamedApiObject
    {
        public NamedApiResource<Move>[] Moves
        {
            get;
        }

        public ResourceName[] Names
        {
            get;
        }
    }

    public class MoveBattleStyle : NamedApiObject
    {
        public ResourceName[] Names
        {
            get;
        }
    }

    public class MoveCategory : NamedApiObject
    {
        public NamedApiResource<Move>[] Moves
        {
            get;
        }

        public Description[] Descriptions
        {
            get;
        }
    }

    public class MoveDamageClass : NamedApiObject
    {
        public Description[] Descriptions
        {
            get;
        }

        public NamedApiResource<Move>[] Moves
        {
            get;
        }

        public ResourceName[] Names
        {
            get;
        }
    }

    public class MoveLearnMethod : NamedApiObject
    {
        public Description[] Descriptions
        {
            get;
        }

        public ResourceName[] Names
        {
            get;
        }

        public NamedApiResource<VersionGroup>[] VersionGroups
        {
            get;
        }
    }

    public class MoveTarget : NamedApiObject
    {
        public Description[] Descriptions
        {
            get;
        }

        public NamedApiResource<Move>[] Moves
        {
            get;
        }

        public ResourceName[] Names
        {
            get;
        }
    }
}
