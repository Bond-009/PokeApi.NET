using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LitJson;

namespace PokeAPI
{
    /// <summary>
    /// Extension methods used in the PokeAPI library.
    /// </summary>
    public static class PokeExtensions
    {
        /// <summary>
        /// Gets whether a number is a power of two or not.
        /// </summary>
        /// <param name="x">The number to check.</param>
        /// <returns>true if the number is a power of two, false otherwise.</returns>
        [MethodImpl((MethodImplOptions)256 /* AggressiveInlining, will only happen when .NET 4.5+ is installed. */)]
        internal static bool IsPowerOfTwo(int x) => x != 0 && (x & (x - 1)) == 0;

        /// <summary>
        /// Gets the <see cref="TypeId" /> of the given <see cref="TypeFlags" />.
        /// </summary>
        /// <param name="type">The <see cref="TypeFlags" /> to convert.</param>
        /// <returns>The converted <see cref="TypeFlags" /> as a <see cref="TypeId" />.</returns>
        /// <exception cref="ArgumentException"><paramref name="type" /> is a composed type, i.e. two TypeFlags instances binary-or'd.</exception>
        public static TypeId Id(this TypeFlags type)
        {
            if (!IsPowerOfTwo((int)type)) // multiple types
                throw new ArgumentException("The type must not be a composed type.", nameof(type));

            return (TypeId /* apparently, float -> enum is possible */)Math.Log((int)type, 2d);
        }
        /// <summary>
        /// Gets the list of <see cref="TypeId" />s the <see cref="TypeFlags" /> represents.
        /// </summary>
        /// <param name="type">The <see cref="TypeFlags" /> to convert.</param>
        /// <returns>A list containing all the <see cref="TypeId" />s represented by <paramref name="type" />. A list containing <see cref="TypeId.Unknown" /> is returned when <paramref name="type" /> equals <see cref="TypeFlags.Unknown" />.</returns>
        public static List<TypeId> AnalyzeIds(this TypeFlags type)
        {
            List<TypeId> ret = new List<TypeId>();

            TypeId id = TypeId.Normal;
            for (TypeFlags i = TypeFlags.Normal; i <= TypeFlags.Fairy; i = (TypeFlags)((int)i << 1), id++)
                if ((type & i) != 0)
                    ret.Add(id);

            if (ret.Count == 0) // type == TypeFlags.Unknown
                ret.Add(TypeId.Unknown);

            return ret;
        }
        /// <summary>
        /// Converts the given <see cref="TypeId" /> to its <see cref="TypeFlags" /> representation.
        /// </summary>
        /// <param name="id">The <see cref="TypeId" /> to convert.</param>
        /// <returns><paramref name="id" /> as a <see cref="TypeFlags" />.</returns>
        [MethodImpl((MethodImplOptions)256 /* AggressiveInlining, will only happen when .NET 4.5+ is installed. */)]
        public static TypeFlags Flags(this TypeId id) => (TypeFlags)(1 << ((int)id - 1));

        internal static int  AsInt    (this JsonData j, string key)
        {
            if (j[key].GetJsonType() == JsonType.String)
                return Int32.Parse((string)j[key]);

            if (j[key].GetJsonType() != JsonType.Int   )
                throw new FormatException();

            return (int)j[key];
        }
        internal static int? AsNullInt(this JsonData j, string key)
        {
            if (j[key].GetJsonType() == JsonType.String)
            {
                int i;
                if (Int32.TryParse((string)j[key], out i))
                    return i;

                return null;
            }
            if (j[key].GetJsonType() != JsonType.Int)
                return null;

            return (int)j[key];
        }

        /// <summary>
        /// Maps a function over a nongeneric <see cref="IEnumerable" />.
        /// </summary>
        /// <typeparam name="TIn">The type of objects contained by <paramref name="coll" />.</typeparam>
        /// <typeparam name="TOut">The return type of <paramref name="fn" />.</typeparam>
        /// <param name="coll">The collection to map <paramref name="fn" /> over.</param>
        /// <param name="fn">The function to map over <paramref name="coll" />.</param>
        /// <returns>A new (generic) <see cref="IEnumerable{TOut}" />, containing the return values of <paramref name="fn" />.</returns>
        public static IEnumerable<TOut> Map<TIn, TOut>(this IEnumerable coll, Func<TIn, TOut> fn)
        {
            foreach (TIn t in coll)
                yield return fn(t);

            yield break;
        }
    }
}
