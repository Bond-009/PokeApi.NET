using System;
using System.Collections.Generic;
using System.Linq;

namespace PokeAPI
{
    public struct AbilityEffectChange
    {
        public Effect[] Effects
        {
            get;
        }

        public NamedApiResource<VersionGroup> VersionGroup
        {
            get;
        }
    }
    public struct AbilityPokemon
    {
        public bool IsHidden
        {
            get;
        }
        public int Slot
        {
            get;
        }

        public NamedApiResource<Pokemon> Pokemon
        {
            get;
        }
    }

    public struct PokemonSpeciesGender
    {
        /// <summary>
        /// null -> genderless
        /// </summary>
        public float? FamaleToMaleRate
        {
            get;
        }

        public NamedApiResource<PokemonSpecies> Species
        {
            get;
        }
    }

    public struct GrowthRateExperienceLevel
    {
        public int Level
        {
            get;
        }
        public int Experience
        {
            get;
        }
    }

    public struct NatureStatChange
    {
        public int Change
        {
            get;
        }

        public NamedApiResource<PokeathlonStat> Stat
        {
            get;
        }
    }
    public struct MoveBattleStylePreference
    {
        public float LowHPPreference
        {
            get;
        }
        public float HighHPPrefernece
        {
            get;
        }

        public NamedApiResource<MoveBattleStyle> BattleStyle
        {
            get;
        }
    }

    public struct NaturePokeathlonStatAffect
    {
        public int MaxChange
        {
            get;
        }

        public NamedApiResource<Nature> Nature
        {
            get;
        }
    }
    public struct NaturePokeathlonStatAffectSets
    {
        public NaturePokeathlonStatAffect[] Increase
        {
            get;
        }
        public NaturePokeathlonStatAffect[] Decrease
        {
            get;
        }
    }

    public struct PokemonAbility
    {
        public bool IsHidden
        {
            get;
        }
        public int Slot
        {
            get;
        }

        public NamedApiResource<Ability> Ability
        {
            get;
        }
    }

    public struct PokemonTypeMap
    {
        public int Slot
        {
            get;
        }

        public NamedApiResource<PokemonType> Type
        {
            get;
        }
    }

    public struct LocationAreaEncounter
    {
        public ApiResource<LocationArea> LocationArea
        {
            get;
        }

        public VersionEncounterDetail[] VersionDetails
        {
            get;
        }
    }

    public struct AwesomeName
    {
        public string Text
        {
            get;
        }

        public NamedApiResource<Language>[] Language
        {
            get;
        }
    }

    public struct Genus
    {
        public string Name;

        public NamedApiResource<Language> Language
        {
            get;
        }
    }
    public struct PokemonSpeciesDexEntry
    {
        public int EntryNumber
        {
            get;
        }

        public NamedApiResource<Pokedex> Pokedex
        {
            get;
        }
    }
    public class PalParkEncounterArea
    {
        public int BaseScore
        {
            get;
        }
        public int Rate
        {
            get;
        }

        public NamedApiResource<PalParkArea> Area
        {
            get;
        }
    }

    //TODO: make generic?
    public struct MoveStatAffect
    {
        public int MaxChange
        {
            get;
        }

        public NamedApiResource<Move> Move
        {
            get;
        }
    }
    //TODO: make generic
    public struct MoveStatAffectSets
    {
        public MoveStatAffect[] Increase
        {
            get;
        }
        public MoveStatAffect[] Decrease
        {
            get;
        }
    }
    public struct NatureStatAffect
    {
        public int MaxChange
        {
            get;
        }

        public NamedApiResource<Nature> Nature
        {
            get;
        }
    }
    public struct NatureStatAffectSets
    {
        public NatureStatAffect[] Increase
        {
            get;
        }
        public NatureStatAffect[] Decrease
        {
            get;
        }
    }

    public struct TypePokemon
    {
        public int Slot
        {
            get;
        }

        public NamedApiResource<Pokemon> Pokemon
        {
            get;
        }
    }
    public struct TypeRelations
    {
        public NamedApiResource<PokemonType> NoDamageTo
        {
            get;
        }
        public NamedApiResource<PokemonType> HalfDamageTo
        {
            get;
        }
        public NamedApiResource<PokemonType> DoubleDamageTo
        {
            get;
        }

        public NamedApiResource<PokemonType> NoDamageFrom
        {
            get;
        }
        public NamedApiResource<PokemonType> HalfDamageFrom
        {
            get;
        }
        public NamedApiResource<PokemonType> DoubleDamageFrom
        {
            get;
        }
    }

    // ---

    public class Ability : NamedApiObject
    {
        public bool IsMainSeries
        {
            get;
        }

        public NamedApiResource<Generation> Generation
        {
            get;
        }

        public ResourceName[] Names
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

        public VersionGroupFlavorText[] FlavorTexts
        {
            get;
        }

        public AbilityPokemon[] Pokemon
        {
            get;
        }
    }

    public class Characteristic : ApiObject
    {
        public int GeneModulo
        {
            get;
        }

        public int[] PossibleValues
        {
            get;
        }

        public Description[] Descriptions
        {
            get;
        }
    }

    public class EggGroup : NamedApiObject
    {
        public ResourceName[] Names
        {
            get;
        }

        public NamedApiResource<PokemonSpecies> Species
        {
            get;
        }
    }

    public class Gender : NamedApiObject
    {
        public PokemonSpeciesGender[] SpeciesDetails
        {
            get;
        }

        public NamedApiResource<PokemonSpecies> RequiredForEvolution
        {
            get;
        }
    }

    public class GrowhtRate : NamedApiObject
    {
        /// <summary>
        /// LaTeX-style (maths mode)
        /// </summary>
        public string Formula
        {
            get;
        }

        public Description[] Descriptions
        {
            get;
        }

        public GrowthRateExperienceLevel[] Levels
        {
            get;
        }

        public NamedApiResource<PokemonSpecies>[] Species
        {
            get;
        }
    }

    public class Nature : NamedApiObject
    {
        public NamedApiResource<Stat> DecreasedStat
        {
            get;
        }

        public NamedApiResource<Stat> IncreasedStat
        {
            get;
        }

        public NamedApiResource<BerryFlavor> HatesFlavor
        {
            get;
        }

        public NamedApiResource<BerryFlavor> LikesFlavor
        {
            get;
        }

        public NatureStatChange[] PokeathlonStatChanges
        {
            get;
        }

        public MoveBattleStylePreference[] BattleStylePreferences
        {
            get;
        }

        public ResourceName[] Names
        {
            get;
        }
    }

    public class PokeathlonStat : NamedApiObject
    {
        public ResourceName[] Names
        {
            get;
        }

        public NaturePokeathlonStatAffectSets AffectingNatures
        {
            get;
        }
    }

    public class Pokemon : NamedApiObject
    {
        public int BaseExperience
        {
            get;
        }

        public bool IsDefault
        {
            get;
        }

        public int Height
        {
            get;
        }
        public int Mass
        {
            get;
        }

        public int Order
        {
            get;
        }

        public PokemonAbility[] Abilities
        {
            get;
        }

        public NamedApiResource<PokemonForm>[] Forms
        {
            get;
        }

        public VersionGameIndex[] GameIndices
        {
            get;
        }

        public NamedApiResource<Item>[] HeldItems
        {
            get;
        }

        public LocationAreaEncounter[] LocationAreaEncounters
        {
            get;
        }

        public NamedApiResource<Move>[] Moves
        {
            get;
        }

        public NamedApiResource<PokemonSpecies> Species
        {
            get;
        }

        public NamedApiResource<Stat> Stats
        {
            get;
        }

        public PokemonTypeMap[] Types
        {
            get;
        }
    }

    public class PokemonColour : NamedApiObject
    {
        public ResourceName[] Names
        {
            get;
        }

        public NamedApiResource<PokemonSpecies>[] Species
        {
            get;
        }
    }

    public class PokemonForm : NamedApiObject
    {
        public int Order
        {
            get;
        }
        public int FormOrder
        {
            get;
        }

        public bool IsDefault
        {
            get;
        }
        public bool IsBattleOnly
        {
            get;
        }
        public bool IsMegaEvolution
        {
            get;
        }

        public string FormName
        {
            get;
        }

        public NamedApiResource<Pokemon> Pokemon
        {
            get;
        }

        public NamedApiResource<VersionGroup> VersionGroup
        {
            get;
        }
    }

    public class PokemonHabitat : NamedApiObject
    {
        public ResourceName[] Names
        {
            get;
        }

        public NamedApiResource<PokemonSpecies>[] Species
        {
            get;
        }
    }

    public class PokemonShape : NamedApiObject
    {
        public AwesomeName[] AwesomeNames
        {
            get;
        }

        public ResourceName[] Names
        {
            get;
        }

        public NamedApiResource<PokemonSpecies>[] Species
        {
            get;
        }
    }

    public class PokemonSpecies : NamedApiObject
    {
        public int Order
        {
            get;
        }

        public float? FemaleToMaleRate
        {
            get;
        }
        public float CaptureRate
        {
            get;
        }

        public int BaseHappiness
        {
            get;
        }

        public bool IsBaby
        {
            get;
        }

        public int HatchCounter
        {
            get;
        }

        public bool HasGenderDifferences
        {
            get;
        }

        public bool FormsAreSwitchable
        {
            get;
        }

        public NamedApiResource<GrowhtRate> GrowthRate
        {
            get;
        }

        public PokemonSpeciesDexEntry[] PokedexNumbers
        {
            get;
        }

        public NamedApiResource<EggGroup>[] EggGroups
        {
            get;
        }

        public NamedApiResource<PokemonColour> Colours
        {
            get;
        }
        public NamedApiResource<PokemonShape> Shape
        {
            get;
        }
        public NamedApiResource<PokemonSpecies> EvolveFromSpecies
        {
            get;
        }
        public ApiResource<EvolutionChain> EvolutionChain
        {
            get;
        }
        public NamedApiResource<PokemonHabitat> Habitat
        {
            get;
        }
        public NamedApiResource<Generation> Generation
        {
            get;
        }

        public ResourceName[] Names
        {
            get;
        }

        public PalParkEncounterArea[] PalParkEncounters
        {
            get;
        }

        public Description[] Descriptions
        {
            get;
        }

        public Genus[] Genera
        {
            get;
        }

        public NamedApiResource<Pokemon> Varieties
        {
            get;
        }
    }

    public class PokemonType : NamedApiObject
    {
        public TypeRelations DamageRelations
        {
            get;
        }

        public GenerationGameIndex[] GameIndices
        {
            get;
        }

        public NamedApiResource<Generation> Generation
        {
            get;
        }

        public NamedApiResource<MoveDamageClass> MoveDamageClass
        {
            get;
        }

        public ResourceName[] Names
        {
            get;
        }

        public TypePokemon[] Pokemon
        {
            get;
        }

        public NamedApiResource<Move>[] Moves
        {
            get;
        }
    }

    public class Stat : NamedApiObject
    {
        public int GameIndex
        {
            get;
        }

        public bool IsBattleOnly
        {
            get;
        }

        public MoveStatAffectSets AffectingMoves
        {
            get;
        }
        public NatureStatAffectSets AffectingNatures
        {
            get;
        }

        public ApiResource<Characteristic>[] Characteristics
        {
            get;
        }

        public NamedApiResource<MoveDamageClass> MoveDamageClass
        {
            get;
        }

        public ResourceName[] Names
        {
            get;
        }
    }
}
