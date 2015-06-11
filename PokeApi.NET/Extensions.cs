using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;

namespace PokeAPI
{
    /// <summary>
    /// Extension methods used in the PokeAPI library
    /// </summary>
    public static class PokeExtensions
    {
        internal static bool IsPowerOfTwo(int x) => x != 0 && (x & (x - 1)) == 0;

        public static TypeID ID(this TypeFlags type)
        {
            if (!IsPowerOfTwo((int)type)) // multiple types
                return 0;

            return (TypeID /* apparently, this is possible */)Math.Log((int)type, 2d);
        }
        public static List<TypeID> AnalyzeIDs(this TypeFlags type)
        {
            List<TypeID> ret = new List<TypeID>();

            TypeID id = TypeID.Normal;
            for (TypeFlags i = TypeFlags.Normal; i <= TypeFlags.Fairy; i = (TypeFlags)((int)i << 1), id++)
                if ((type & i) != 0)
                    ret.Add(id);

            if (ret.Count == 0)
                ret.Add(TypeID.Unknown);

            return ret;
        }
        public static TypeFlags Flags(this TypeID id) => (TypeFlags)(int)Math.Pow(2, (int)id - 1);

        public static int AsInt(this JsonData j, string key)
        {
            if (j[key].GetJsonType() == JsonType.String)
                return Int32.Parse((string)j[key]);

            return (int)j[key];
        }
        public static int? AsNullInt(this JsonData j, string key)
        {
            if (j[key].GetJsonType() == JsonType.String)
                return String.IsNullOrEmpty((string)j[key]) ? null : (int?)Int32.Parse((string)j[key]);

            return (int)j[key];
        }

        public static IEnumerable<TOut> Map<TIn, TOut>(this IEnumerable coll, Func<TIn, TOut> fn)
        {
            foreach (TIn t in coll)
                yield return fn(t);

            yield break;
        }
    }
}
