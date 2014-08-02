using System;
using System.Collections.Generic;
using LitJson;

namespace PokeAPI.NET
{
    /// <summary>
    /// Extension methods used in the PokeAPI.NET library
    /// </summary>
    public static class PokeExtensions
    {
        internal static bool IsPowerOfTwo(int x)
        {
            return x != 0 && (x & (x - 1)) == 0;
        }

        /// <summary>
        /// Converts a PokemonType to it's ID
        /// </summary>
        /// <param name="type">The PokemonType to convert</param>
        /// <returns>The converted PokemonType</returns>
        public static PokemonTypeID ID(this PokemonTypeFlags type)
        {
            if (!IsPowerOfTwo((int)type)) // multiple types
                return 0;

            PokemonTypeID id = PokemonTypeID.Normal;
            for (int i = 1; i <= (int)PokemonTypeFlags.Fairy; i *= 2, id++) // lazy again
                if (((int)type & i) != 0)
                    return id;

            return 0;
        }
        /// <summary>
        /// Analyzes a Type into the separated enum values.
        /// </summary>
        /// <param name="type">The Type flags to analyze.</param>
        /// <returns>A list containing the enum values.</returns>
        public static List<PokemonTypeID> AnalyzeIDs(this PokemonTypeFlags type)
        {
            List<PokemonTypeID> ret = new List<PokemonTypeID>();

            for (PokemonTypeFlags i = PokemonTypeFlags.Bug; i <= PokemonTypeFlags.Water; i = (PokemonTypeFlags)((int)i * 2))
                if ((type & i) != 0)
                    ret.Add((type & i).ID());

            if (ret.Count == 0)
                ret.Add(PokemonTypeID.Unknown);

            return ret;
        }

        /// <summary>
        /// Gets the JsonData value as an int.
        /// </summary>
        /// <param name="j"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int AsInt(this JsonData j, string key)
        {
            if (j[key].GetJsonType() == JsonType.String)
                return Int32.Parse((string)j[key]);

            return (int)j[key];
        }
        /// <summary>
        /// Gets the JsonData value as a nullable int.
        /// </summary>
        /// <param name="j"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int? AsNullInt(this JsonData j, string key)
        {
            if (j[key].GetJsonType() == JsonType.String)
                return String.IsNullOrEmpty((string)j[key]) ? null : (int?)Int32.Parse((string)j[key]);

            return (int)j[key];
        }

        /// <summary>
        /// Fills a collection with properties (or casts) from another collection.
        /// </summary>
        /// <typeparam name="T1">The type of the elements in the initial collection.</typeparam>
        /// <typeparam name="T2">The type of the elements in the new collection.</typeparam>
        /// <param name="ienum">The initial collection.</param>
        /// <param name="func">A Func that returns the property.</param>
        /// <returns>The collection with the properties/casts from the initial collection.</returns>
        public static IEnumerable<T2> FillNew<T1, T2>(this IEnumerable<T1> ienum, Func<T1, T2> func)
        {
            // examples:
            // var ints    = objColl.FillNew(obj => obj.intValue  );
            // var strings = objColl.FillNew(obj => obj.ToString());

            List<T2> ret = new List<T2>();

            foreach (T1 t in ienum)
                ret.Add(func(t));

            return ret;
        }
    }
}
