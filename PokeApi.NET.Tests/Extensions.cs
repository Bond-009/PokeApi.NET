using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace PokeAPI.Tests
{
    public static class Extensions
    {
        static IEnumerable<PropertyInfo> FilterProps<T>(this object obj) => typeof(T).GetProperties()
            .Where(p => p.PropertyType == typeof(T)
                || p.PropertyType.IsSubclassOf(typeof(T))
                || (typeof(T).IsInterface && p.PropertyType.GetInterfaces().Any(t => t == typeof(T))));

        public static bool AnyClassPropertyEmpty <T>(this object obj,                        Predicate<T> check = null) where T : class =>
            obj.FilterProps<T>().Select(p => (T)p.GetValue(obj)).Any(v => v == null        || check == null || check(v));
        public static bool AnyStructPropertyEmpty<T>(this object obj, T defVal = default(T), Predicate<T> check = null) where T : struct =>
            obj.FilterProps<T>().Select(p => (T)p.GetValue(obj)).Any(v => v.Equals(defVal) || check == null || check(v));

        public static bool AnyStringPropertyEmpty     (this object obj) => AnyClassPropertyEmpty<string>(obj, String.IsNullOrEmpty     );
        public static bool AnyStringPropertyWhiteSpace(this object obj) => AnyClassPropertyEmpty<string>(obj, String.IsNullOrWhiteSpace);

        /// <summary>
        /// Generates the slug. http://predicatet.blogspot.com.es/2009/04/improved-c-slug-generator-or-how-to.html
        /// </summary>
        /// <param name="phrase">The phrase.</param>
        /// <returns></returns>
        public static string GenerateSlug(this string phrase)
        {
            string str = phrase.RemoveAccent().ToLower();

            str = Regex.Replace(str, @"[^a-z0-9\s-]", ""); // invalid chars
            str = Regex.Replace(str, @"\s+", " ").Trim(); // convert multiple spaces into one space
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim(); // cut and trim it
            str = Regex.Replace(str, @"\s", "-"); // hyphens

            return str;
        }

        public static string RemoveAccent(this string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }
}
