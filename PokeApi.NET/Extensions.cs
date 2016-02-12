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
    }
}
