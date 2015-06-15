using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PokeAPI.Tests
{
    public static class Extensions
    {
        static IEnumerable<PropertyInfo> FilterProps<T>(this object obj) => typeof(T).GetProperties()
            .Where(p => p.PropertyType == typeof(T)
                || p.PropertyType.IsSubclassOf(typeof(T))
                || (typeof(T).IsInterface && p.PropertyType.GetInterfaces().Any(t => t == typeof(T))));

        public static bool AnyClassPropertyEmpty<T>(this object obj, Predicate<T> check = null) where T : class =>
            obj.FilterProps<T>().Select(p => (T)p.GetValue(obj)).Any(v => v == null || (check == null ? true : check(v)));
        public static bool AnyStructPropertyEmpty<T>(this object obj, T defVal = default(T), Predicate<T> check = null) where T : struct =>
            obj.FilterProps<T>().Select(p => (T)p.GetValue(obj)).Any(v => v.Equals(defVal) || (check == null ? true : check(v)));

        public static bool AnyStringPropertyEmpty     (this object obj) => AnyClassPropertyEmpty<string>(obj, s => String.IsNullOrEmpty     (s));
        public static bool AnyStringPropertyWhiteSpace(this object obj) => AnyClassPropertyEmpty<string>(obj, s => String.IsNullOrWhiteSpace(s));
    }
}
