using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;

namespace PokeAPI
{
    public struct ContestComboDetail
    {
        [JsonPropertyName("use_before")]
        public NamedApiResource<Move> UseBefore
        {
            get;
        }
        [JsonPropertyName("use_after")]
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

        [JsonPropertyName("min_hits")]
        public int? MinHits
        {
            get;
        }
        [JsonPropertyName("max_hits")]
        public int? MaxHits
        {
            get;
        }
        [JsonPropertyName("min_turns")]
        public int? MinTurns
        {
            get;
        }
        [JsonPropertyName("max_turns")]
        public int? MaxTurns
        {
            get;
        }

        [JsonPropertyName("drain")]
        public int DrainRecoil
        {
            get;
        }
        public int Healing
        {
            get;
        }

        [JsonPropertyName("crit_rate")]
        public int CritRate
        {
            get;
        }
        [JsonPropertyName("ailment_chance")]
        public float AilmentChance
        {
            get;
        }
        [JsonPropertyName("flinch_chance")]
        public float FlinchChance
        {
            get;
        }
        [JsonPropertyName("stat_chance")]
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
        [JsonPropertyName("effect_chance")]
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

        [JsonPropertyName("effect_entries")]
        public VerboseEffect[] Effects
        {
            get;
        }

        public PokemonType Type
        {
            get;
        }

        [JsonPropertyName("version_group")]
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
        [JsonPropertyName("effect_chance")]
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

        [JsonPropertyName("contest_combos")]
        public ContestComboSet[] ComboSets
        {
            get;
        }

        [JsonPropertyName("contest_type")]
        public NamedApiResource<ContestType> ContestType
        {
            get;
        }

        [JsonPropertyName("contest_effect")]
        public ApiResource<ContestEffect> ContestEffect
        {
            get;
        }

        [JsonPropertyName("damage_class")]
        public NamedApiResource<MoveDamageClass> DamageClass
        {
            get;
        }

        [JsonPropertyName("effect_entries")]
        public VerboseEffect[] Effects
        {
            get;
        }

        [JsonPropertyName("effect_changes")]
        public AbilityEffectChange[] EffectChanges
        {
            get;
        }

        public NamedApiResource<Generation> Generation
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

        [JsonPropertyName("past_values")]
        public PastMoveStatValue[] PastValues
        {
            get;
        }

        [JsonPropertyName("stat_changed")]
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

        [JsonPropertyName("version_groups")]
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
