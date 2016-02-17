using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;

namespace PokeAPI
{
    public class Pokemon : NamedApiObject
    {
        [JsonPropertyName("base_experience")]
        public int BaseExperience
        {
            get;
        }

        [JsonPropertyName("is_default")]
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

        [JsonPropertyName("game_indices")]
        public VersionGameIndex[] GameIndices
        {
            get;
        }

        [JsonPropertyName("held_items")]
        public NamedApiResource<Item>[] HeldItems
        {
            get;
        }

        [JsonPropertyName("location_area_encounters")]
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

        [JsonPropertyName("pokemon_species")]
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
        [JsonPropertyName("form_order")]
        public int FormOrder
        {
            get;
        }

        [JsonPropertyName("is_default")]
        public bool IsDefault
        {
            get;
        }
        [JsonPropertyName("is_battle_only")]
        public bool IsBattleOnly
        {
            get;
        }
        [JsonPropertyName("is_mega")]
        public bool IsMegaEvolution
        {
            get;
        }

        [JsonPropertyName("form_name")]
        public string FormName
        {
            get;
        }

        public NamedApiResource<Pokemon> Pokemon
        {
            get;
        }

        [JsonPropertyName("version_group")]
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

        [JsonPropertyName("pokemon_species")]
        public NamedApiResource<PokemonSpecies>[] Species
        {
            get;
        }
    }

    public class PokemonShape : NamedApiObject
    {
        [JsonPropertyName("awesome_names")]
        public AwesomeName[] AwesomeNames
        {
            get;
        }

        public ResourceName[] Names
        {
            get;
        }

        [JsonPropertyName("pokemon_species")]
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

        [JsonPropertyName("gender_rate"), JsonConverter(typeof(PokemonSpeciesGender.GenderConverter))]
        public float? FemaleToMaleRate
        {
            get;
        }
        [JsonPropertyName("capture_rate")]
        public float CaptureRate
        {
            get;
        }

        [JsonPropertyName("base_happiness")]
        public int BaseHappiness
        {
            get;
        }

        [JsonPropertyName("is_baby")]
        public bool IsBaby
        {
            get;
        }

        [JsonPropertyName("hatch_counter")]
        public int HatchCounter
        {
            get;
        }

        [JsonPropertyName("has_gender_differences")]
        public bool HasGenderDifferences
        {
            get;
        }

        [JsonPropertyName("forms_switchable")]
        public bool FormsAreSwitchable
        {
            get;
        }

        [JsonPropertyName("growth_rate")]
        public NamedApiResource<GrowhtRate> GrowthRate
        {
            get;
        }

        [JsonPropertyName("pokedex_numbers")]
        public PokemonSpeciesDexEntry[] PokedexNumbers
        {
            get;
        }

        [JsonPropertyName("egg_groups")]
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
        [JsonPropertyName("evolve_from_species")]
        public NamedApiResource<PokemonSpecies> EvolveFromSpecies
        {
            get;
        }
        [JsonPropertyName("evolution_chain")]
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

        [JsonPropertyName("pal_park_encounters")]
        public PalParkEncounterArea[] PalParkEncounters
        {
            get;
        }

        [JsonPropertyName("form_descriptions")]
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
        [JsonPropertyName("damage_relations")]
        public TypeRelations DamageRelations
        {
            get;
        }

        [JsonPropertyName("game_indices")]
        public GenerationGameIndex[] GameIndices
        {
            get;
        }

        public NamedApiResource<Generation> Generation
        {
            get;
        }

        [JsonPropertyName("move_damage_class")]
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
        [JsonPropertyName("game_index")]
        public int GameIndex
        {
            get;
        }

        [JsonPropertyName("is_battle_only")]
        public bool IsBattleOnly
        {
            get;
        }

        [JsonPropertyName("affecting_moves")]
        public StatAffectSets<Move  > AffectingMoves
        {
            get;
        }
        [JsonPropertyName("affecting_natures")]
        public StatAffectSets<Nature> AffectingNatures
        {
            get;
        }

        public ApiResource<Characteristic>[] Characteristics
        {
            get;
        }

        [JsonPropertyName("move_damage_class")]
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
