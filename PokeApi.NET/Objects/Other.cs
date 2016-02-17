using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;

namespace PokeAPI
{
    public class Ability : NamedApiObject
    {
        [JsonPropertyName("is_main_series")]
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

        [JsonPropertyName("flavor_text_entries")]
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
        [JsonPropertyName("gene_modulo")]
        public int GeneModulo
        {
            get;
        }

        [JsonPropertyName("possible_values")]
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

        [JsonPropertyName("pokemon_species")]
        public NamedApiResource<PokemonSpecies> Species
        {
            get;
        }
    }

    public class Gender : NamedApiObject
    {
        [JsonPropertyName("pokemon_species_details")]
        public PokemonSpeciesGender[] SpeciesDetails
        {
            get;
        }

        [JsonPropertyName("required_for_evolution")]
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

        [JsonPropertyName("pokemon_species")]
        public NamedApiResource<PokemonSpecies>[] Species
        {
            get;
        }
    }

    public class Nature : NamedApiObject
    {
        [JsonPropertyName("decreased_stat")]
        public NamedApiResource<Stat> DecreasedStat
        {
            get;
        }
        [JsonPropertyName("increased_stat")]
        public NamedApiResource<Stat> IncreasedStat
        {
            get;
        }

        [JsonPropertyName("hates_flavor")]
        public NamedApiResource<BerryFlavor> HatesFlavor
        {
            get;
        }
        [JsonPropertyName("likes_flavor")]
        public NamedApiResource<BerryFlavor> LikesFlavor
        {
            get;
        }

        [JsonPropertyName("pokeathlon_stat_changes")]
        public NatureStatChange[] PokeathlonStatChanges
        {
            get;
        }

        [JsonPropertyName("move_battle_style_preferences")]
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

        [JsonPropertyName("affecting_natures")]
        public NaturePokeathlonStatAffectSets AffectingNatures
        {
            get;
        }
    }

}
