using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;

namespace PokeAPI.NET
{
    using PTIDT = Tuple<PokemonTypeID, PokemonTypeID>;

    /// <summary>
    /// Represents an instance of a Pokémon Type
    /// </summary>
    /// <remarks>Can be implicitely casted to a PokemonType enumeration (and vice versa).</remarks>
    public class PokemonType : PokeApiType
    {
        /// <summary>
        /// Wether it should cache types or not
        /// </summary>
        public static bool ShouldCacheData = true;
        /// <summary>
        /// Gets all cached types
        /// </summary>
        public static Dictionary<int, PokemonType> CachedTypes = new Dictionary<int, PokemonType>();

        #region public readonly static Dictionary<PTIDT, double> DamageMultipliers = new Dictionary<PTIDT, double>() { [...] };
        /// <summary>
        /// The damage multipliers map. The items in the tuple represent the move's type and the defending Pokémon's type, respectively.
        /// </summary>
        /// <remarks>Only works for single types.</remarks>
        public readonly static Dictionary<PTIDT, double> DamageMultipliers = new Dictionary<PTIDT, double>()
        {
            // http://bulbapedia.bulbagarden.net/wiki/Type_chart

            #region Normal
            { new PTIDT(PokemonTypeID.Normal, PokemonTypeID.Normal  ), 1d   },
            { new PTIDT(PokemonTypeID.Normal, PokemonTypeID.Fighting), 1d   },
            { new PTIDT(PokemonTypeID.Normal, PokemonTypeID.Flying  ), 1d   },
            { new PTIDT(PokemonTypeID.Normal, PokemonTypeID.Poison  ), 1d   },
            { new PTIDT(PokemonTypeID.Normal, PokemonTypeID.Ground  ), 1d   },
            { new PTIDT(PokemonTypeID.Normal, PokemonTypeID.Rock    ), 0.5d },
            { new PTIDT(PokemonTypeID.Normal, PokemonTypeID.Bug     ), 1d   },
            { new PTIDT(PokemonTypeID.Normal, PokemonTypeID.Ghost   ), 0d   },
            { new PTIDT(PokemonTypeID.Normal, PokemonTypeID.Steel   ), 0.5d },
            { new PTIDT(PokemonTypeID.Normal, PokemonTypeID.Fire    ), 1d   },
            { new PTIDT(PokemonTypeID.Normal, PokemonTypeID.Water   ), 1d   },
            { new PTIDT(PokemonTypeID.Normal, PokemonTypeID.Grass   ), 1d   },
            { new PTIDT(PokemonTypeID.Normal, PokemonTypeID.Electric), 1d   },
            { new PTIDT(PokemonTypeID.Normal, PokemonTypeID.Psychic ), 1d   },
            { new PTIDT(PokemonTypeID.Normal, PokemonTypeID.Ice     ), 1d   },
            { new PTIDT(PokemonTypeID.Normal, PokemonTypeID.Dragon  ), 1d   },
            { new PTIDT(PokemonTypeID.Normal, PokemonTypeID.Dark    ), 1d   },
            { new PTIDT(PokemonTypeID.Normal, PokemonTypeID.Fairy   ), 1d   },
            { new PTIDT(PokemonTypeID.Normal, PokemonTypeID.Unknown ), 1d   },
            #endregion

            #region Fighting
            { new PTIDT(PokemonTypeID.Fighting, PokemonTypeID.Normal  ), 2d   },
            { new PTIDT(PokemonTypeID.Fighting, PokemonTypeID.Fighting), 1d   },
            { new PTIDT(PokemonTypeID.Fighting, PokemonTypeID.Flying  ), 0.5d },
            { new PTIDT(PokemonTypeID.Fighting, PokemonTypeID.Poison  ), 0.5d },
            { new PTIDT(PokemonTypeID.Fighting, PokemonTypeID.Ground  ), 1d   },
            { new PTIDT(PokemonTypeID.Fighting, PokemonTypeID.Rock    ), 2d   },
            { new PTIDT(PokemonTypeID.Fighting, PokemonTypeID.Bug     ), 0.5d },
            { new PTIDT(PokemonTypeID.Fighting, PokemonTypeID.Ghost   ), 0d   },
            { new PTIDT(PokemonTypeID.Fighting, PokemonTypeID.Steel   ), 2d   },
            { new PTIDT(PokemonTypeID.Fighting, PokemonTypeID.Fire    ), 1d   },
            { new PTIDT(PokemonTypeID.Fighting, PokemonTypeID.Water   ), 1d   },
            { new PTIDT(PokemonTypeID.Fighting, PokemonTypeID.Grass   ), 1d   },
            { new PTIDT(PokemonTypeID.Fighting, PokemonTypeID.Electric), 1d   },
            { new PTIDT(PokemonTypeID.Fighting, PokemonTypeID.Psychic ), 0.5d },
            { new PTIDT(PokemonTypeID.Fighting, PokemonTypeID.Ice     ), 2d   },
            { new PTIDT(PokemonTypeID.Fighting, PokemonTypeID.Dragon  ), 1d   },
            { new PTIDT(PokemonTypeID.Fighting, PokemonTypeID.Dark    ), 2d   },
            { new PTIDT(PokemonTypeID.Fighting, PokemonTypeID.Fairy   ), 0.5d },
            { new PTIDT(PokemonTypeID.Fighting, PokemonTypeID.Unknown ), 1d   },
            #endregion

            #region Flying
            { new PTIDT(PokemonTypeID.Flying, PokemonTypeID.Normal  ), 1d   },
            { new PTIDT(PokemonTypeID.Flying, PokemonTypeID.Fighting), 2d   },
            { new PTIDT(PokemonTypeID.Flying, PokemonTypeID.Flying  ), 1d   },
            { new PTIDT(PokemonTypeID.Flying, PokemonTypeID.Poison  ), 1d   },
            { new PTIDT(PokemonTypeID.Flying, PokemonTypeID.Ground  ), 1d   },
            { new PTIDT(PokemonTypeID.Flying, PokemonTypeID.Rock    ), 0.5d },
            { new PTIDT(PokemonTypeID.Flying, PokemonTypeID.Bug     ), 2d   },
            { new PTIDT(PokemonTypeID.Flying, PokemonTypeID.Ghost   ), 1d   },
            { new PTIDT(PokemonTypeID.Flying, PokemonTypeID.Steel   ), 0.5d },
            { new PTIDT(PokemonTypeID.Flying, PokemonTypeID.Fire    ), 1d   },
            { new PTIDT(PokemonTypeID.Flying, PokemonTypeID.Water   ), 1d   },
            { new PTIDT(PokemonTypeID.Flying, PokemonTypeID.Grass   ), 2d   },
            { new PTIDT(PokemonTypeID.Flying, PokemonTypeID.Electric), 0.5d },
            { new PTIDT(PokemonTypeID.Flying, PokemonTypeID.Psychic ), 1d   },
            { new PTIDT(PokemonTypeID.Flying, PokemonTypeID.Ice     ), 1d   },
            { new PTIDT(PokemonTypeID.Flying, PokemonTypeID.Dragon  ), 1d   },
            { new PTIDT(PokemonTypeID.Flying, PokemonTypeID.Dark    ), 1d   },
            { new PTIDT(PokemonTypeID.Flying, PokemonTypeID.Fairy   ), 1d   },
            { new PTIDT(PokemonTypeID.Flying, PokemonTypeID.Unknown ), 1d   },
            #endregion

            #region Poison
            { new PTIDT(PokemonTypeID.Poison, PokemonTypeID.Normal  ), 1d   },
            { new PTIDT(PokemonTypeID.Poison, PokemonTypeID.Fighting), 2d   },
            { new PTIDT(PokemonTypeID.Poison, PokemonTypeID.Flying  ), 1d   },
            { new PTIDT(PokemonTypeID.Poison, PokemonTypeID.Poison  ), 0.5d },
            { new PTIDT(PokemonTypeID.Poison, PokemonTypeID.Ground  ), 0.5d },
            { new PTIDT(PokemonTypeID.Poison, PokemonTypeID.Rock    ), 0.5d },
            { new PTIDT(PokemonTypeID.Poison, PokemonTypeID.Bug     ), 1d   },
            { new PTIDT(PokemonTypeID.Poison, PokemonTypeID.Ghost   ), 0.5d },
            { new PTIDT(PokemonTypeID.Poison, PokemonTypeID.Steel   ), 0d   },
            { new PTIDT(PokemonTypeID.Poison, PokemonTypeID.Fire    ), 1d   },
            { new PTIDT(PokemonTypeID.Poison, PokemonTypeID.Water   ), 1d   },
            { new PTIDT(PokemonTypeID.Poison, PokemonTypeID.Grass   ), 2d   },
            { new PTIDT(PokemonTypeID.Poison, PokemonTypeID.Electric), 1d   },
            { new PTIDT(PokemonTypeID.Poison, PokemonTypeID.Psychic ), 1d   },
            { new PTIDT(PokemonTypeID.Poison, PokemonTypeID.Ice     ), 1d   },
            { new PTIDT(PokemonTypeID.Poison, PokemonTypeID.Dragon  ), 1d   },
            { new PTIDT(PokemonTypeID.Poison, PokemonTypeID.Dark    ), 1d   },
            { new PTIDT(PokemonTypeID.Poison, PokemonTypeID.Fairy   ), 2d   },
            { new PTIDT(PokemonTypeID.Poison, PokemonTypeID.Unknown ), 1d   },
            #endregion

            #region Ground
            { new PTIDT(PokemonTypeID.Ground, PokemonTypeID.Normal  ), 1d   },
            { new PTIDT(PokemonTypeID.Ground, PokemonTypeID.Fighting), 1d   },
            { new PTIDT(PokemonTypeID.Ground, PokemonTypeID.Flying  ), 0d   },
            { new PTIDT(PokemonTypeID.Ground, PokemonTypeID.Poison  ), 2d   },
            { new PTIDT(PokemonTypeID.Ground, PokemonTypeID.Ground  ), 1d   },
            { new PTIDT(PokemonTypeID.Ground, PokemonTypeID.Rock    ), 2d   },
            { new PTIDT(PokemonTypeID.Ground, PokemonTypeID.Bug     ), 0.5d },
            { new PTIDT(PokemonTypeID.Ground, PokemonTypeID.Ghost   ), 1d   },
            { new PTIDT(PokemonTypeID.Ground, PokemonTypeID.Steel   ), 2d   },
            { new PTIDT(PokemonTypeID.Ground, PokemonTypeID.Fire    ), 2d   },
            { new PTIDT(PokemonTypeID.Ground, PokemonTypeID.Water   ), 0.5d },
            { new PTIDT(PokemonTypeID.Ground, PokemonTypeID.Grass   ), 2d   },
            { new PTIDT(PokemonTypeID.Ground, PokemonTypeID.Electric), 2d   },
            { new PTIDT(PokemonTypeID.Ground, PokemonTypeID.Psychic ), 1d   },
            { new PTIDT(PokemonTypeID.Ground, PokemonTypeID.Ice     ), 1d   },
            { new PTIDT(PokemonTypeID.Ground, PokemonTypeID.Dragon  ), 1d   },
            { new PTIDT(PokemonTypeID.Ground, PokemonTypeID.Dark    ), 1d   },
            { new PTIDT(PokemonTypeID.Ground, PokemonTypeID.Fairy   ), 1d   },
            { new PTIDT(PokemonTypeID.Ground, PokemonTypeID.Unknown ), 1d   },
            #endregion

            #region Rock
            { new PTIDT(PokemonTypeID.Rock, PokemonTypeID.Normal  ), 1d   },
            { new PTIDT(PokemonTypeID.Rock, PokemonTypeID.Fighting), 0.5d },
            { new PTIDT(PokemonTypeID.Rock, PokemonTypeID.Flying  ), 2d   },
            { new PTIDT(PokemonTypeID.Rock, PokemonTypeID.Poison  ), 1d   },
            { new PTIDT(PokemonTypeID.Rock, PokemonTypeID.Ground  ), 0.5d },
            { new PTIDT(PokemonTypeID.Rock, PokemonTypeID.Rock    ), 1d   },
            { new PTIDT(PokemonTypeID.Rock, PokemonTypeID.Bug     ), 2d   },
            { new PTIDT(PokemonTypeID.Rock, PokemonTypeID.Ghost   ), 1d   },
            { new PTIDT(PokemonTypeID.Rock, PokemonTypeID.Steel   ), 0.5d },
            { new PTIDT(PokemonTypeID.Rock, PokemonTypeID.Fire    ), 2d   },
            { new PTIDT(PokemonTypeID.Rock, PokemonTypeID.Water   ), 1d   },
            { new PTIDT(PokemonTypeID.Rock, PokemonTypeID.Grass   ), 1d   },
            { new PTIDT(PokemonTypeID.Rock, PokemonTypeID.Electric), 1d   },
            { new PTIDT(PokemonTypeID.Rock, PokemonTypeID.Psychic ), 1d   },
            { new PTIDT(PokemonTypeID.Rock, PokemonTypeID.Ice     ), 2d   },
            { new PTIDT(PokemonTypeID.Rock, PokemonTypeID.Dragon  ), 1d   },
            { new PTIDT(PokemonTypeID.Rock, PokemonTypeID.Dark    ), 1d   },
            { new PTIDT(PokemonTypeID.Rock, PokemonTypeID.Fairy   ), 1d   },
            { new PTIDT(PokemonTypeID.Rock, PokemonTypeID.Unknown ), 1d   },
            #endregion

            #region Bug
            { new PTIDT(PokemonTypeID.Bug, PokemonTypeID.Normal  ), 1d   },
            { new PTIDT(PokemonTypeID.Bug, PokemonTypeID.Fighting), 0.5d },
            { new PTIDT(PokemonTypeID.Bug, PokemonTypeID.Flying  ), 0.5d },
            { new PTIDT(PokemonTypeID.Bug, PokemonTypeID.Poison  ), 0.5d },
            { new PTIDT(PokemonTypeID.Bug, PokemonTypeID.Ground  ), 1d   },
            { new PTIDT(PokemonTypeID.Bug, PokemonTypeID.Rock    ), 1d   },
            { new PTIDT(PokemonTypeID.Bug, PokemonTypeID.Bug     ), 1d   },
            { new PTIDT(PokemonTypeID.Bug, PokemonTypeID.Ghost   ), 0.5d },
            { new PTIDT(PokemonTypeID.Bug, PokemonTypeID.Steel   ), 0.5d },
            { new PTIDT(PokemonTypeID.Bug, PokemonTypeID.Fire    ), 0.5d },
            { new PTIDT(PokemonTypeID.Bug, PokemonTypeID.Water   ), 1d   },
            { new PTIDT(PokemonTypeID.Bug, PokemonTypeID.Grass   ), 1d   },
            { new PTIDT(PokemonTypeID.Bug, PokemonTypeID.Electric), 1d   },
            { new PTIDT(PokemonTypeID.Bug, PokemonTypeID.Psychic ), 2d   },
            { new PTIDT(PokemonTypeID.Bug, PokemonTypeID.Ice     ), 1d   },
            { new PTIDT(PokemonTypeID.Bug, PokemonTypeID.Dragon  ), 1d   },
            { new PTIDT(PokemonTypeID.Bug, PokemonTypeID.Dark    ), 2d   },
            { new PTIDT(PokemonTypeID.Bug, PokemonTypeID.Fairy   ), 0.5d },
            { new PTIDT(PokemonTypeID.Bug, PokemonTypeID.Unknown ), 1d   },
            #endregion

            #region Ghost
            { new PTIDT(PokemonTypeID.Ghost, PokemonTypeID.Normal  ), 0d   },
            { new PTIDT(PokemonTypeID.Ghost, PokemonTypeID.Fighting), 1d   },
            { new PTIDT(PokemonTypeID.Ghost, PokemonTypeID.Flying  ), 1d   },
            { new PTIDT(PokemonTypeID.Ghost, PokemonTypeID.Poison  ), 1d   },
            { new PTIDT(PokemonTypeID.Ghost, PokemonTypeID.Ground  ), 1d   },
            { new PTIDT(PokemonTypeID.Ghost, PokemonTypeID.Rock    ), 1d   },
            { new PTIDT(PokemonTypeID.Ghost, PokemonTypeID.Bug     ), 1d   },
            { new PTIDT(PokemonTypeID.Ghost, PokemonTypeID.Ghost   ), 2d   },
            { new PTIDT(PokemonTypeID.Ghost, PokemonTypeID.Steel   ), 0.5d },
            { new PTIDT(PokemonTypeID.Ghost, PokemonTypeID.Fire    ), 1d   },
            { new PTIDT(PokemonTypeID.Ghost, PokemonTypeID.Water   ), 1d   },
            { new PTIDT(PokemonTypeID.Ghost, PokemonTypeID.Grass   ), 1d   },
            { new PTIDT(PokemonTypeID.Ghost, PokemonTypeID.Electric), 1d   },
            { new PTIDT(PokemonTypeID.Ghost, PokemonTypeID.Psychic ), 2d   },
            { new PTIDT(PokemonTypeID.Ghost, PokemonTypeID.Ice     ), 1d   },
            { new PTIDT(PokemonTypeID.Ghost, PokemonTypeID.Dragon  ), 1d   },
            { new PTIDT(PokemonTypeID.Ghost, PokemonTypeID.Dark    ), 0.5d },
            { new PTIDT(PokemonTypeID.Ghost, PokemonTypeID.Fairy   ), 1d   },
            { new PTIDT(PokemonTypeID.Ghost, PokemonTypeID.Unknown ), 1d   },
            #endregion

            #region Steel
            { new PTIDT(PokemonTypeID.Steel, PokemonTypeID.Normal  ), 1d   },
            { new PTIDT(PokemonTypeID.Steel, PokemonTypeID.Fighting), 1d   },
            { new PTIDT(PokemonTypeID.Steel, PokemonTypeID.Flying  ), 1d   },
            { new PTIDT(PokemonTypeID.Steel, PokemonTypeID.Poison  ), 1d   },
            { new PTIDT(PokemonTypeID.Steel, PokemonTypeID.Ground  ), 1d   },
            { new PTIDT(PokemonTypeID.Steel, PokemonTypeID.Rock    ), 2d   },
            { new PTIDT(PokemonTypeID.Steel, PokemonTypeID.Bug     ), 1d   },
            { new PTIDT(PokemonTypeID.Steel, PokemonTypeID.Ghost   ), 1d   },
            { new PTIDT(PokemonTypeID.Steel, PokemonTypeID.Steel   ), 0.5d },
            { new PTIDT(PokemonTypeID.Steel, PokemonTypeID.Fire    ), 0.5d },
            { new PTIDT(PokemonTypeID.Steel, PokemonTypeID.Water   ), 0.5d },
            { new PTIDT(PokemonTypeID.Steel, PokemonTypeID.Grass   ), 1d   },
            { new PTIDT(PokemonTypeID.Steel, PokemonTypeID.Electric), 0.5d },
            { new PTIDT(PokemonTypeID.Steel, PokemonTypeID.Psychic ), 1d   },
            { new PTIDT(PokemonTypeID.Steel, PokemonTypeID.Ice     ), 2d   },
            { new PTIDT(PokemonTypeID.Steel, PokemonTypeID.Dragon  ), 1d   },
            { new PTIDT(PokemonTypeID.Steel, PokemonTypeID.Dark    ), 1d   },
            { new PTIDT(PokemonTypeID.Steel, PokemonTypeID.Fairy   ), 2d   },
            { new PTIDT(PokemonTypeID.Steel, PokemonTypeID.Unknown ), 1d   },
            #endregion

            #region Fire
            { new PTIDT(PokemonTypeID.Fire, PokemonTypeID.Normal  ), 1d   },
            { new PTIDT(PokemonTypeID.Fire, PokemonTypeID.Fighting), 1d   },
            { new PTIDT(PokemonTypeID.Fire, PokemonTypeID.Flying  ), 1d   },
            { new PTIDT(PokemonTypeID.Fire, PokemonTypeID.Poison  ), 1d   },
            { new PTIDT(PokemonTypeID.Fire, PokemonTypeID.Ground  ), 0.5d },
            { new PTIDT(PokemonTypeID.Fire, PokemonTypeID.Rock    ), 0.5d },
            { new PTIDT(PokemonTypeID.Fire, PokemonTypeID.Bug     ), 2d   },
            { new PTIDT(PokemonTypeID.Fire, PokemonTypeID.Ghost   ), 1d   },
            { new PTIDT(PokemonTypeID.Fire, PokemonTypeID.Steel   ), 2d   },
            { new PTIDT(PokemonTypeID.Fire, PokemonTypeID.Fire    ), 0.5d },
            { new PTIDT(PokemonTypeID.Fire, PokemonTypeID.Water   ), 0.5d },
            { new PTIDT(PokemonTypeID.Fire, PokemonTypeID.Grass   ), 2d   },
            { new PTIDT(PokemonTypeID.Fire, PokemonTypeID.Electric), 1d   },
            { new PTIDT(PokemonTypeID.Fire, PokemonTypeID.Psychic ), 1d   },
            { new PTIDT(PokemonTypeID.Fire, PokemonTypeID.Ice     ), 2d   },
            { new PTIDT(PokemonTypeID.Fire, PokemonTypeID.Dragon  ), 0.5d },
            { new PTIDT(PokemonTypeID.Fire, PokemonTypeID.Dark    ), 1d   },
            { new PTIDT(PokemonTypeID.Fire, PokemonTypeID.Fairy   ), 1d   },
            { new PTIDT(PokemonTypeID.Fire, PokemonTypeID.Unknown ), 1d   },
            #endregion

            #region Water
            { new PTIDT(PokemonTypeID.Water, PokemonTypeID.Normal  ), 1d   },
            { new PTIDT(PokemonTypeID.Water, PokemonTypeID.Fighting), 1d   },
            { new PTIDT(PokemonTypeID.Water, PokemonTypeID.Flying  ), 1d   },
            { new PTIDT(PokemonTypeID.Water, PokemonTypeID.Poison  ), 1d   },
            { new PTIDT(PokemonTypeID.Water, PokemonTypeID.Ground  ), 2d   },
            { new PTIDT(PokemonTypeID.Water, PokemonTypeID.Rock    ), 2d   },
            { new PTIDT(PokemonTypeID.Water, PokemonTypeID.Bug     ), 2d   },
            { new PTIDT(PokemonTypeID.Water, PokemonTypeID.Ghost   ), 1d   },
            { new PTIDT(PokemonTypeID.Water, PokemonTypeID.Steel   ), 1d   },
            { new PTIDT(PokemonTypeID.Water, PokemonTypeID.Fire    ), 2d   },
            { new PTIDT(PokemonTypeID.Water, PokemonTypeID.Water   ), 0.5d },
            { new PTIDT(PokemonTypeID.Water, PokemonTypeID.Grass   ), 0.5d },
            { new PTIDT(PokemonTypeID.Water, PokemonTypeID.Electric), 0.5d },
            { new PTIDT(PokemonTypeID.Water, PokemonTypeID.Psychic ), 1d   },
            { new PTIDT(PokemonTypeID.Water, PokemonTypeID.Ice     ), 1d   },
            { new PTIDT(PokemonTypeID.Water, PokemonTypeID.Dragon  ), 0.5d },
            { new PTIDT(PokemonTypeID.Water, PokemonTypeID.Dark    ), 1d   },
            { new PTIDT(PokemonTypeID.Water, PokemonTypeID.Fairy   ), 1d   },
            { new PTIDT(PokemonTypeID.Water, PokemonTypeID.Unknown ), 1d   },
            #endregion

            #region Grass
            { new PTIDT(PokemonTypeID.Grass, PokemonTypeID.Normal  ), 1d   },
            { new PTIDT(PokemonTypeID.Grass, PokemonTypeID.Fighting), 1d   },
            { new PTIDT(PokemonTypeID.Grass, PokemonTypeID.Flying  ), 0.5d },
            { new PTIDT(PokemonTypeID.Grass, PokemonTypeID.Poison  ), 0.5d },
            { new PTIDT(PokemonTypeID.Grass, PokemonTypeID.Ground  ), 2d   },
            { new PTIDT(PokemonTypeID.Grass, PokemonTypeID.Rock    ), 2d   },
            { new PTIDT(PokemonTypeID.Grass, PokemonTypeID.Bug     ), 0.5d },
            { new PTIDT(PokemonTypeID.Grass, PokemonTypeID.Ghost   ), 1d   },
            { new PTIDT(PokemonTypeID.Grass, PokemonTypeID.Steel   ), 0.5d },
            { new PTIDT(PokemonTypeID.Grass, PokemonTypeID.Fire    ), 0.5d },
            { new PTIDT(PokemonTypeID.Grass, PokemonTypeID.Water   ), 2d   },
            { new PTIDT(PokemonTypeID.Grass, PokemonTypeID.Grass   ), 0.5d },
            { new PTIDT(PokemonTypeID.Grass, PokemonTypeID.Electric), 1d   },
            { new PTIDT(PokemonTypeID.Grass, PokemonTypeID.Psychic ), 1d   },
            { new PTIDT(PokemonTypeID.Grass, PokemonTypeID.Ice     ), 1d   },
            { new PTIDT(PokemonTypeID.Grass, PokemonTypeID.Dragon  ), 0.5d },
            { new PTIDT(PokemonTypeID.Grass, PokemonTypeID.Dark    ), 1d   },
            { new PTIDT(PokemonTypeID.Grass, PokemonTypeID.Fairy   ), 1d   },
            { new PTIDT(PokemonTypeID.Grass, PokemonTypeID.Unknown ), 1d   },
            #endregion

            #region Electric
            { new PTIDT(PokemonTypeID.Electric, PokemonTypeID.Normal  ), 1d   },
            { new PTIDT(PokemonTypeID.Electric, PokemonTypeID.Fighting), 1d   },
            { new PTIDT(PokemonTypeID.Electric, PokemonTypeID.Flying  ), 2d   },
            { new PTIDT(PokemonTypeID.Electric, PokemonTypeID.Poison  ), 1d   },
            { new PTIDT(PokemonTypeID.Electric, PokemonTypeID.Ground  ), 0d   },
            { new PTIDT(PokemonTypeID.Electric, PokemonTypeID.Rock    ), 1d   },
            { new PTIDT(PokemonTypeID.Electric, PokemonTypeID.Bug     ), 1d   },
            { new PTIDT(PokemonTypeID.Electric, PokemonTypeID.Ghost   ), 1d   },
            { new PTIDT(PokemonTypeID.Electric, PokemonTypeID.Steel   ), 1d   },
            { new PTIDT(PokemonTypeID.Electric, PokemonTypeID.Fire    ), 1d   },
            { new PTIDT(PokemonTypeID.Electric, PokemonTypeID.Water   ), 2d   },
            { new PTIDT(PokemonTypeID.Electric, PokemonTypeID.Grass   ), 0.5d },
            { new PTIDT(PokemonTypeID.Electric, PokemonTypeID.Electric), 0.5d },
            { new PTIDT(PokemonTypeID.Electric, PokemonTypeID.Psychic ), 1d   },
            { new PTIDT(PokemonTypeID.Electric, PokemonTypeID.Ice     ), 1d   },
            { new PTIDT(PokemonTypeID.Electric, PokemonTypeID.Dragon  ), 0.5d },
            { new PTIDT(PokemonTypeID.Electric, PokemonTypeID.Dark    ), 1d   },
            { new PTIDT(PokemonTypeID.Electric, PokemonTypeID.Fairy   ), 1d   },
            { new PTIDT(PokemonTypeID.Electric, PokemonTypeID.Unknown ), 1d   },
            #endregion

            #region Psychic
            { new PTIDT(PokemonTypeID.Psychic, PokemonTypeID.Normal  ), 1d   },
            { new PTIDT(PokemonTypeID.Psychic, PokemonTypeID.Fighting), 2d   },
            { new PTIDT(PokemonTypeID.Psychic, PokemonTypeID.Flying  ), 1d   },
            { new PTIDT(PokemonTypeID.Psychic, PokemonTypeID.Poison  ), 2d   },
            { new PTIDT(PokemonTypeID.Psychic, PokemonTypeID.Ground  ), 1d   },
            { new PTIDT(PokemonTypeID.Psychic, PokemonTypeID.Rock    ), 1d   },
            { new PTIDT(PokemonTypeID.Psychic, PokemonTypeID.Bug     ), 0.5d },
            { new PTIDT(PokemonTypeID.Psychic, PokemonTypeID.Ghost   ), 1d   },
            { new PTIDT(PokemonTypeID.Psychic, PokemonTypeID.Steel   ), 0.5d },
            { new PTIDT(PokemonTypeID.Psychic, PokemonTypeID.Fire    ), 1d   },
            { new PTIDT(PokemonTypeID.Psychic, PokemonTypeID.Water   ), 1d   },
            { new PTIDT(PokemonTypeID.Psychic, PokemonTypeID.Grass   ), 1d   },
            { new PTIDT(PokemonTypeID.Psychic, PokemonTypeID.Electric), 1d   },
            { new PTIDT(PokemonTypeID.Psychic, PokemonTypeID.Psychic ), 0.5d },
            { new PTIDT(PokemonTypeID.Psychic, PokemonTypeID.Ice     ), 1d   },
            { new PTIDT(PokemonTypeID.Psychic, PokemonTypeID.Dragon  ), 1d   },
            { new PTIDT(PokemonTypeID.Psychic, PokemonTypeID.Dark    ), 0d   },
            { new PTIDT(PokemonTypeID.Psychic, PokemonTypeID.Fairy   ), 1d   },
            { new PTIDT(PokemonTypeID.Psychic, PokemonTypeID.Unknown ), 1d   },
            #endregion

            #region Ice
            { new PTIDT(PokemonTypeID.Ice, PokemonTypeID.Normal  ), 1d   },
            { new PTIDT(PokemonTypeID.Ice, PokemonTypeID.Fighting), 1d   },
            { new PTIDT(PokemonTypeID.Ice, PokemonTypeID.Flying  ), 2d   },
            { new PTIDT(PokemonTypeID.Ice, PokemonTypeID.Poison  ), 1d   },
            { new PTIDT(PokemonTypeID.Ice, PokemonTypeID.Ground  ), 2d   },
            { new PTIDT(PokemonTypeID.Ice, PokemonTypeID.Rock    ), 1d   },
            { new PTIDT(PokemonTypeID.Ice, PokemonTypeID.Bug     ), 1d   },
            { new PTIDT(PokemonTypeID.Ice, PokemonTypeID.Ghost   ), 1d   },
            { new PTIDT(PokemonTypeID.Ice, PokemonTypeID.Steel   ), 0.5d },
            { new PTIDT(PokemonTypeID.Ice, PokemonTypeID.Fire    ), 0.5d },
            { new PTIDT(PokemonTypeID.Ice, PokemonTypeID.Water   ), 0.5d },
            { new PTIDT(PokemonTypeID.Ice, PokemonTypeID.Grass   ), 2d   },
            { new PTIDT(PokemonTypeID.Ice, PokemonTypeID.Electric), 1d   },
            { new PTIDT(PokemonTypeID.Ice, PokemonTypeID.Psychic ), 1d   },
            { new PTIDT(PokemonTypeID.Ice, PokemonTypeID.Ice     ), 0.5d },
            { new PTIDT(PokemonTypeID.Ice, PokemonTypeID.Dragon  ), 2d   },
            { new PTIDT(PokemonTypeID.Ice, PokemonTypeID.Dark    ), 1d   },
            { new PTIDT(PokemonTypeID.Ice, PokemonTypeID.Fairy   ), 1d   },
            { new PTIDT(PokemonTypeID.Ice, PokemonTypeID.Unknown ), 1d   },
            #endregion

            #region Dragon
            { new PTIDT(PokemonTypeID.Dragon, PokemonTypeID.Normal  ), 1d   },
            { new PTIDT(PokemonTypeID.Dragon, PokemonTypeID.Fighting), 1d   },
            { new PTIDT(PokemonTypeID.Dragon, PokemonTypeID.Flying  ), 1d   },
            { new PTIDT(PokemonTypeID.Dragon, PokemonTypeID.Poison  ), 1d   },
            { new PTIDT(PokemonTypeID.Dragon, PokemonTypeID.Ground  ), 1d   },
            { new PTIDT(PokemonTypeID.Dragon, PokemonTypeID.Rock    ), 1d   },
            { new PTIDT(PokemonTypeID.Dragon, PokemonTypeID.Bug     ), 1d   },
            { new PTIDT(PokemonTypeID.Dragon, PokemonTypeID.Ghost   ), 1d   },
            { new PTIDT(PokemonTypeID.Dragon, PokemonTypeID.Steel   ), 0.5d },
            { new PTIDT(PokemonTypeID.Dragon, PokemonTypeID.Fire    ), 1d   },
            { new PTIDT(PokemonTypeID.Dragon, PokemonTypeID.Water   ), 1d   },
            { new PTIDT(PokemonTypeID.Dragon, PokemonTypeID.Grass   ), 1d   },
            { new PTIDT(PokemonTypeID.Dragon, PokemonTypeID.Electric), 1d   },
            { new PTIDT(PokemonTypeID.Dragon, PokemonTypeID.Psychic ), 1d   },
            { new PTIDT(PokemonTypeID.Dragon, PokemonTypeID.Ice     ), 1d   },
            { new PTIDT(PokemonTypeID.Dragon, PokemonTypeID.Dragon  ), 2d   },
            { new PTIDT(PokemonTypeID.Dragon, PokemonTypeID.Dark    ), 1d   },
            { new PTIDT(PokemonTypeID.Dragon, PokemonTypeID.Fairy   ), 0d   },
            { new PTIDT(PokemonTypeID.Dragon, PokemonTypeID.Unknown ), 1d   },
            #endregion

            #region Dark
            { new PTIDT(PokemonTypeID.Dark, PokemonTypeID.Normal  ), 1d   },
            { new PTIDT(PokemonTypeID.Dark, PokemonTypeID.Fighting), 0.5d },
            { new PTIDT(PokemonTypeID.Dark, PokemonTypeID.Flying  ), 1d   },
            { new PTIDT(PokemonTypeID.Dark, PokemonTypeID.Poison  ), 1d   },
            { new PTIDT(PokemonTypeID.Dark, PokemonTypeID.Ground  ), 1d   },
            { new PTIDT(PokemonTypeID.Dark, PokemonTypeID.Rock    ), 1d   },
            { new PTIDT(PokemonTypeID.Dark, PokemonTypeID.Bug     ), 1d   },
            { new PTIDT(PokemonTypeID.Dark, PokemonTypeID.Ghost   ), 2d   },
            { new PTIDT(PokemonTypeID.Dark, PokemonTypeID.Steel   ), 1d   },
            { new PTIDT(PokemonTypeID.Dark, PokemonTypeID.Fire    ), 1d   },
            { new PTIDT(PokemonTypeID.Dark, PokemonTypeID.Water   ), 1d   },
            { new PTIDT(PokemonTypeID.Dark, PokemonTypeID.Grass   ), 1d   },
            { new PTIDT(PokemonTypeID.Dark, PokemonTypeID.Electric), 1d   },
            { new PTIDT(PokemonTypeID.Dark, PokemonTypeID.Psychic ), 2d   },
            { new PTIDT(PokemonTypeID.Dark, PokemonTypeID.Ice     ), 1d   },
            { new PTIDT(PokemonTypeID.Dark, PokemonTypeID.Dragon  ), 1d   },
            { new PTIDT(PokemonTypeID.Dark, PokemonTypeID.Dark    ), 0.5d },
            { new PTIDT(PokemonTypeID.Dark, PokemonTypeID.Fairy   ), 0.5d },
            { new PTIDT(PokemonTypeID.Dark, PokemonTypeID.Unknown ), 1d   },
            #endregion

            #region Fairy
            { new PTIDT(PokemonTypeID.Fairy, PokemonTypeID.Normal  ), 1d   },
            { new PTIDT(PokemonTypeID.Fairy, PokemonTypeID.Fighting), 2d   },
            { new PTIDT(PokemonTypeID.Fairy, PokemonTypeID.Flying  ), 1d   },
            { new PTIDT(PokemonTypeID.Fairy, PokemonTypeID.Poison  ), 0.5d },
            { new PTIDT(PokemonTypeID.Fairy, PokemonTypeID.Ground  ), 1d   },
            { new PTIDT(PokemonTypeID.Fairy, PokemonTypeID.Rock    ), 1d   },
            { new PTIDT(PokemonTypeID.Fairy, PokemonTypeID.Bug     ), 1d   },
            { new PTIDT(PokemonTypeID.Fairy, PokemonTypeID.Ghost   ), 1d   },
            { new PTIDT(PokemonTypeID.Fairy, PokemonTypeID.Steel   ), 0.5d },
            { new PTIDT(PokemonTypeID.Fairy, PokemonTypeID.Fire    ), 0.5d },
            { new PTIDT(PokemonTypeID.Fairy, PokemonTypeID.Water   ), 1d   },
            { new PTIDT(PokemonTypeID.Fairy, PokemonTypeID.Grass   ), 1d   },
            { new PTIDT(PokemonTypeID.Fairy, PokemonTypeID.Electric), 1d   },
            { new PTIDT(PokemonTypeID.Fairy, PokemonTypeID.Psychic ), 1d   },
            { new PTIDT(PokemonTypeID.Fairy, PokemonTypeID.Ice     ), 1d   },
            { new PTIDT(PokemonTypeID.Fairy, PokemonTypeID.Dragon  ), 2d   },
            { new PTIDT(PokemonTypeID.Fairy, PokemonTypeID.Dark    ), 2d   },
            { new PTIDT(PokemonTypeID.Fairy, PokemonTypeID.Fairy   ), 1d   },
            { new PTIDT(PokemonTypeID.Fairy, PokemonTypeID.Unknown ), 1d   },
            #endregion

            #region Unknown
            { new PTIDT(PokemonTypeID.Unknown, PokemonTypeID.Normal  ), 1d   },
            { new PTIDT(PokemonTypeID.Unknown, PokemonTypeID.Fighting), 1d   },
            { new PTIDT(PokemonTypeID.Unknown, PokemonTypeID.Flying  ), 1d   },
            { new PTIDT(PokemonTypeID.Unknown, PokemonTypeID.Poison  ), 1d   },
            { new PTIDT(PokemonTypeID.Unknown, PokemonTypeID.Ground  ), 1d   },
            { new PTIDT(PokemonTypeID.Unknown, PokemonTypeID.Rock    ), 1d   },
            { new PTIDT(PokemonTypeID.Unknown, PokemonTypeID.Bug     ), 1d   },
            { new PTIDT(PokemonTypeID.Unknown, PokemonTypeID.Ghost   ), 1d   },
            { new PTIDT(PokemonTypeID.Unknown, PokemonTypeID.Steel   ), 1d   },
            { new PTIDT(PokemonTypeID.Unknown, PokemonTypeID.Fire    ), 1d   },
            { new PTIDT(PokemonTypeID.Unknown, PokemonTypeID.Water   ), 1d   },
            { new PTIDT(PokemonTypeID.Unknown, PokemonTypeID.Grass   ), 1d   },
            { new PTIDT(PokemonTypeID.Unknown, PokemonTypeID.Electric), 1d   },
            { new PTIDT(PokemonTypeID.Unknown, PokemonTypeID.Psychic ), 1d   },
            { new PTIDT(PokemonTypeID.Unknown, PokemonTypeID.Ice     ), 1d   },
            { new PTIDT(PokemonTypeID.Unknown, PokemonTypeID.Dragon  ), 1d   },
            { new PTIDT(PokemonTypeID.Unknown, PokemonTypeID.Dark    ), 1d   },
            { new PTIDT(PokemonTypeID.Unknown, PokemonTypeID.Fairy   ), 1d   },
            { new PTIDT(PokemonTypeID.Unknown, PokemonTypeID.Unknown ), 1d   }
            #endregion
        };
        #endregion

        /// <summary>
        /// The types this PokemonType instance is ineffective against
        /// </summary>
        public List<NameUriPair> Ineffective
        {
            get;
            private set;
        } = new List<NameUriPair>();
        /// <summary>
        /// The types this PokemonType instance has no effect against
        /// </summary>
        public List<NameUriPair> NoEffect
        {
            get;
            private set;
        } = new List<NameUriPair>();
        /// <summary>
        /// The types this PokemonType instance is resistant to
        /// </summary>
        public List<NameUriPair> Resistance
        {
            get;
            private set;
        } = new List<NameUriPair>();
        /// <summary>
        /// The types this PokemonType instance is super effective against
        /// </summary>
        public List<NameUriPair> SuperEffective
        {
            get;
            private set;
        } = new List<NameUriPair>();
        /// <summary>
        /// The types this PokemonType instance is weak to
        /// </summary>
        public List<NameUriPair> Weakness
        {
            get;
            private set;
        } = new List<NameUriPair>();

        /// <summary>
        /// Gets an entry of the Ineffective list as a PokemonType
        /// </summary>
        /// <param name="index">The index of the entry</param>
        /// <returns>The entry of the Ineffective list as a PokemonType</returns>
        public PokemonType RefIneffective(int index)
        {
            return GetInstance(Ineffective[index].Name);
        }
        /// <summary>
        /// Gets an entry of the NoEffect list as a PokemonType
        /// </summary>
        /// <param name="index">The index of the entry</param>
        /// <returns>The entry of the NoEffect list as a PokemonType</returns>
        public PokemonType RefNoEffect(int index)
        {
            return GetInstance(NoEffect[index].Name);
        }
        /// <summary>
        /// Gets an entry of the Resistance list as a PokemonType
        /// </summary>
        /// <param name="index">The index of the entry</param>
        /// <returns>The entry of the Resistance list as a PokemonType</returns>
        public PokemonType RefResistance(int index)
        {
            return GetInstance(Resistance[index].Name);
        }
        /// <summary>
        /// Gets an entry of the SuperEffective list as a PokemonType
        /// </summary>
        /// <param name="index">The index of the entry</param>
        /// <returns>The entry of the SuperEffective list as a PokemonType</returns>
        public PokemonType RefSuperEffective(int index)
        {
            return GetInstance(SuperEffective[index].Name);
        }
        /// <summary>
        /// Gets an entry of the Weakness list as a PokemonType
        /// </summary>
        /// <param name="index">The index of the entry</param>
        /// <returns>The entry of the Weakness list as a PokemonType</returns>
        public PokemonType RefWeakness(int index)
        {
            return GetInstance(Weakness[index].Name);
        }

        /// <summary>
        /// Gets the type IDs of the PokemonType.
        /// </summary>
        public List<PokemonTypeID> TypeIDs
        {
            get
            {
                return ((PokemonTypeFlags)this).AnalyzeIDs();
            }
        }

        /// <summary>
        /// Creates a new instance from a JSON object
        /// </summary>
        /// <param name="source">The JSON object where to create the new instance from</param>
        protected override void Create(JsonData source)
        {
            foreach (JsonData data in source["no_effect"])
                NoEffect.Add(ParseNameUriPair(data));
            foreach (JsonData data in source["ineffective"])
                Ineffective.Add(ParseNameUriPair(data));
            foreach (JsonData data in source["resistance"])
                Resistance.Add(ParseNameUriPair(data));
            foreach (JsonData data in source["super_effective"])
                SuperEffective.Add(ParseNameUriPair(data));
            foreach (JsonData data in source["weakness"])
                Weakness.Add(ParseNameUriPair(data));
        }

        /// <summary>
        /// Creates an instance of a PokemonType with the given name.
        /// </summary>
        /// <param name="name">The name of the PokemonType to instantiate</param>
        /// <returns>The created instance of the PokemonType</returns>
        /// <remarks>Only works on a single-type PokemonType.</remarks>
        public static PokemonType GetInstance(string name)
        {
            if (name.Trim() == "???")
                name = "Unknown";

            if (Enum.TryParse(name.Trim(), true, out PokemonTypeID id))
                return GetInstance((int)id);

            return null;
        }
        /// <summary>
        /// Creates an instance of a PokemonType with the given PokemonTypeID
        /// </summary>
        /// <param name="type">The type of the PokemonType to instantiate</param>
        /// <returns>The created instance of the PokemonType</returns>
        public static PokemonType GetInstance(PokemonTypeID type)
        {
            return GetInstance((int)type);
        }
        /// <summary>
        /// Creates an instance of a PokemonType with the given ID
        /// </summary>
        /// <param name="id">The id of the PokemonType to instantiate</param>
        /// <returns>The created instance of the PokemonType</returns>
        public static PokemonType GetInstance(int id)
        {
            if (CachedTypes.ContainsKey(id))
                return CachedTypes[id];

            PokemonType p = new PokemonType();
            if (id == 0)
            {
                p.Created = p.LastModified = DateTime.Now;

                p.ID = 0;
                p.Name = "???";

                p.Ineffective    = new List<NameUriPair>();
                p.NoEffect       = new List<NameUriPair>();
                p.Resistance     = new List<NameUriPair>();
                p.SuperEffective = new List<NameUriPair>();
                p.Weakness       = new List<NameUriPair>();

                p.ResourceUri = new Uri("http://www.pokeapi.co/");
            }
            else
                Create(DataFetcher.GetType(id), p);

            if (ShouldCacheData)
                CachedTypes.Add(id, p);

            return p;
        }

        /// <summary>
        /// Gets the damage multiplier of the given attacking and defending types.
        /// </summary>
        /// <param name="attacking">The attacking type.</param>
        /// <param name="defending">The defending type. Can be multiple types.</param>
        /// <returns>The damage multiplier of the given attacking and defending types.</returns>
        public static double GetDamageMultiplier(PokemonTypeID attacking, PokemonTypeFlags defending)
        {
            List<PokemonTypeID> analyzed = defending.AnalyzeIDs();

            double ret = 1d;

            for (int i = 0; i < analyzed.Count; i++)
                ret *= DamageMultipliers[new PTIDT(attacking, analyzed[i])];

            return ret;
        }

        /// <summary>
        /// Combines multiple PokemonTypes into one PokemonTypeID.
        /// </summary>
        /// <param name="types">The PokemonTypes to combine.</param>
        /// <returns>The combined PokemonTypes as a PokemonTypeID.</returns>
        public static PokemonTypeFlags Combine(params PokemonType[] types)
        {
            PokemonTypeFlags ret = 0;

            for (int i = 0; i < types.Length; i++)
                ret |= types[i];

            return ret;
        }
        /// <summary>
        /// Combines multiple PokemonTypes into one PokemonTypeID.
        /// </summary>
        /// <param name="types">The PokemonTypes to combine.</param>
        /// <returns>The combined PokemonTypes as a PokemonTypeID.</returns>
        public static PokemonTypeFlags Combine(IEnumerable<PokemonType> types)
        {
            return Combine(types.ToArray());
        }

        /// <summary>
        /// Converts a PokemonType into a PokemonType
        /// </summary>
        /// <param name="type">The PokemonType to convert from</param>
        public static implicit operator PokemonTypeFlags(PokemonType type)
        {
            Enum.TryParse(type.Name, true, out PokemonTypeFlags ret);
            return ret;
        }
        /// <summary>
        /// Converts a PokemonType into a PokemonType
        /// </summary>
        /// <param name="type">The PokemonType to convert from</param>
        public static explicit operator PokemonType(PokemonTypeFlags type)
        {
            return GetInstance(type.ID());
        }
        /// <summary>
        /// Converts a PokemonType into a PokemonType
        /// </summary>
        /// <param name="type">The PokemonType to convert from</param>
        public static implicit operator PokemonTypeID(PokemonType type)
        {
            if (type.TypeIDs.Count != 1)
                return PokemonTypeID.Unknown;

            Enum.TryParse(type.Name, true, out PokemonTypeID ret);
            return ret;
        }
        /// <summary>
        /// Converts a PokemonType into a PokemonType
        /// </summary>
        /// <param name="type">The PokemonType to convert from</param>
        public static explicit operator PokemonType(PokemonTypeID type)
        {
            return GetInstance(type);
        }
    }
}
