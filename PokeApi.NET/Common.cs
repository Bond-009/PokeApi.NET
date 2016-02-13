using System;
using System.Collections.Generic;
using System.Linq;

namespace PokeAPI
{
    public enum TimeOfDay
    {
        Day,
        Night
    }

    public struct Description
    {
        /// <summary>
        /// The localized description for an <see cref="ApiResource{T}" /> in a specific langauge.
        /// </summary>
        public string Text
        {
            get;
        }

        /// <summary>
        /// The language this description is in.
        /// </summary>
        public NamedApiResource<Language> Language
        {
            get;
        }
    }

    public struct Effect
    {
        /// <summary>
        /// The localized text for an <see cref="ApiResource{T}" /> in a specific language.
        /// </summary>
        public string Text
        {
            get;
        }

        /// <summary>
        /// The language this effect is in.
        /// </summary>
        public NamedApiResource<Language> Language
        {
            get;
        }
    }

    public struct Encounter
    {
        /// <summary>
        /// The lowest level the pokémon could be encountered at.
        /// </summary>
        public int MinLevel
        {
            get;
        }
        /// <summary>
        /// The highest level the pokémon could be encountered at.
        /// </summary>
        public int MaxLevel
        {
            get;
        }

        /// <summary>
        /// A list os condition values that muse be in effect for this encounter to occur.
        /// </summary>
        public NamedApiResource<EncounterConditionValue>[] ConditionValues
        {
            get;
        }

        /// <summary>
        /// The chance, ranging from 0 to 1, that this encounter will occur.
        /// </summary>
        public float Chance
        {
            get;
        }

        /// <summary>
        /// The method by which this encounter happens.
        /// </summary>
        public NamedApiResource<EncounterMethod> Method
        {
            get;
        }
    }

    public struct FlavorText
    {
        /// <summary>
        /// The localized name for an <see cref="ApiResource{T}" /> in a specific langauge.
        /// </summary>
        public string Text
        {
            get;
        }

        /// <summary>
        /// The language this flavor text is in.
        /// </summary>
        public NamedApiResource<Language> Language
        {
            get;
        }
    }

    public struct GenerationGameIndex
    {
        /// <summary>
        /// The internal ID of an <see cref="ApiResource{T}" /> within game data.
        /// </summary>
        public int GameIndex
        {
            get;
        }

        /// <summary>
        /// The generation relevant to this game index.
        /// </summary>
        public NamedApiResource<Generation> Generation
        {
            get;
        }
    }

    public struct ResourceName
    {
        /// <summary>
        /// The localized name for an <see cref="ApiResource{T}" /> in a specific langauge.
        /// </summary>
        public string Name
        {
            get;
        }

        /// <summary>
        /// The language this name is in.
        /// </summary>
        public NamedApiResource<Language> Language
        {
            get;
        }
    }

    public struct VerboseEffect
    {
        /// <summary>
        /// The localized effect text for an <see cref="ApiResource{T}" /> in a specific language.
        /// </summary>
        public string Effect
        {
            get;
        }
        /// <summary>
        /// The localized effect text in brief.
        /// </summary>
        public string ShortEffect
        {
            get;
        }

        /// <summary>
        /// The language this effect is in.
        /// </summary>
        public NamedApiResource<Language> Language
        {
            get;
        }
    }

    public struct VersionEncounterDetail
    {
        /// <summary>
        /// the game version this encounter happens in.
        /// </summary>
        public NamedApiResource<GameVersion> Version
        {
            get;
        }

        /// <summary>
        /// the total chance, ranging from 0 to 1, of all encounter potential.
        /// </summary>
        public float MaxChance
        {
            get;
        }

        /// <summary>
        /// A list of special encounters and their specifics.
        /// </summary>
        public Encounter[] EncounterDetails
        {
            get;
        }
    }

    public struct VersionGameIndex
    {
        /// <summary>
        /// The internal ID of an <see cref="ApiResource{T}" /> within game data.
        /// </summary>
        public int GameIndex
        {
            get;
        }

        /// <summary>
        /// The version relevant to this game index.
        /// </summary>
        public NamedApiResource<GameVersion> Version
        {
            get;
        }
    }

    public struct VersionGroupFlavorText
    {
        /// <summary>
        /// The localized name for an <see cref="ApiResource{T}" /> in a specific language.
        /// </summary>
        public string Text
        {
            get;
        }

        /// <summary>
        /// The language this name is in.
        /// </summary>
        public NamedApiResource<Language> Language
        {
            get;
        }

        /// <summary>
        /// The version group which uses this flavor text.
        /// </summary>
        public NamedApiResource<VersionGroup> VersionGroup
        {
            get;
        }
    }
}
