using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;

namespace PokeAPI
{
    public struct AbilityEffectChange
    {
        [JsonPropertyName("effect_entries")]
        public Effect[] Effects
        {
            get;
        }

        [JsonPropertyName("version_group")]
        public NamedApiResource<VersionGroup> VersionGroup
        {
            get;
        }
    }
    public struct AbilityPokemon
    {
        [JsonPropertyName("is_hidden")]
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
        internal class GenderConverter : IJsonConverter
        {
            public bool Deserialize(JsonData j, out object value)
            {
                if (j.JsonType != JsonType.Int)
                {
                    value = null;
                    return false;
                }

                var i = (int)j;

                value = i == -1 ? null : (float?)i / 0.128f;

                return true;
            }
        }

        /// <summary>
        /// The chance of this <see cref="Pokemon" /> being female; or null for genderless.
        /// </summary>
        [JsonPropertyName("rate"), JsonConverter(typeof(GenderConverter))]
        public float? FamaleToMaleRate
        {
            get;
        }

        [JsonPropertyName("pokemon_species")]
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
        [JsonPropertyName("low_hp_preference")]
        public float LowHPPreference
        {
            get;
        }
        [JsonPropertyName("high_hp_preference")]
        public float HighHPPrefernece
        {
            get;
        }

        [JsonPropertyName("move_battle_style")]
        public NamedApiResource<MoveBattleStyle> BattleStyle
        {
            get;
        }
    }

    public struct NaturePokeathlonStatAffect
    {
        [JsonPropertyName("max_change")]
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
        [JsonPropertyName("is_hidden")]
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
        [JsonPropertyName("location_area")]
        public ApiResource<LocationArea> LocationArea
        {
            get;
        }

        [JsonPropertyName("version_details")]
        public VersionEncounterDetail[] VersionDetails
        {
            get;
        }
    }

    public struct AwesomeName
    {
        [JsonPropertyName("awesome_name")]
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
        [JsonPropertyName("genus")]
        public string Name
        {
            get;
        }

        public NamedApiResource<Language> Language
        {
            get;
        }
    }
    public struct PokemonSpeciesDexEntry
    {
        [JsonPropertyName("entry_number")]
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
        [JsonPropertyName("base_score")]
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

    public struct StatAffect<T>
        where T : NamedApiObject
    {
        [JsonPropertyName("max_change")]
        public int MaxChange
        {
            get;
        }

        [JsonPropertyName("move"), JsonPropertyName("nature")]
        public NamedApiResource<T> Resource
        {
            get;
        }
    }
    public struct StatAffectSets<T>
        where T : NamedApiObject
    {
        public StatAffect<T>[] Increase
        {
            get;
        }
        public StatAffect<T>[] Decrease
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
        [JsonPropertyName("no_damage_to")]
        public NamedApiResource<PokemonType> NoDamageTo
        {
            get;
        }
        [JsonPropertyName("half_damage_to")]
        public NamedApiResource<PokemonType> HalfDamageTo
        {
            get;
        }
        [JsonPropertyName("double_damage_to")]
        public NamedApiResource<PokemonType> DoubleDamageTo
        {
            get;
        }

        [JsonPropertyName("no_damage_from")]
        public NamedApiResource<PokemonType> NoDamageFrom
        {
            get;
        }
        [JsonPropertyName("half_damage_from")]
        public NamedApiResource<PokemonType> HalfDamageFrom
        {
            get;
        }
        [JsonPropertyName("double_damage_from")]
        public NamedApiResource<PokemonType> DoubleDamageFrom
        {
            get;
        }
    }
}
