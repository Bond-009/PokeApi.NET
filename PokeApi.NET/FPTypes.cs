using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PokeAPI
{
    public enum Unit : byte { Value }

    public static class Maybe
    {
        internal const MethodImplOptions AggressiveInlining = (MethodImplOptions)0x100; // AggressiveInlining, will only happen when .NET 4.5+ is installed.

        [MethodImpl(AggressiveInlining)]
        public static Maybe<T> Just<T>(T value) => new Maybe<T>(value);
    }
    public struct Maybe<T> // Nullable is only for structs, need support for any T
    {
        public readonly static Maybe<T> Nothing = new Maybe<T>();

        T v;

        public bool HasValue
        {
            get;
        }
        public T Value
        {
            get
            {
                if (!HasValue)
                    throw new InvalidOperationException();

                return v;
            }
        }

        public Maybe(T value)
        {
            HasValue = true;
            v = value;
        }
    }

    public static class ValueTuple
    {
        [MethodImpl(Maybe.AggressiveInlining)]
        public static ValueTuple<T1, T2> Create<T1, T2>(T1 a, T2 b) => new ValueTuple<T1, T2>(a, b);
    }
    public struct ValueTuple<T1, T2> : IEquatable<ValueTuple<T1, T2>>
    {
        public T1 Item1
        {
            get;
        }
        public T2 Item2
        {
            get;
        }

        public ValueTuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            if (obj is ValueTuple<T1, T2>)
                return Equals((ValueTuple<T1, T2>)obj);

            return false;
        }
        public override int GetHashCode() => (ReferenceEquals(Item1, null) ? 0 : Item1.GetHashCode()) ^ (ReferenceEquals(Item2, null) ? 0 : Item2.GetHashCode());
        public override string ToString() => $"{{{Item1}, {Item2}}}";

        public bool Equals(ValueTuple<T1, T2> vt)
        {
            bool eq = true;

            eq &= ReferenceEquals(Item1, null) ? ReferenceEquals(vt.Item1, null) : Item1.Equals(vt.Item1);
            eq &= ReferenceEquals(Item2, null) ? ReferenceEquals(vt.Item2, null) : Item2.Equals(vt.Item2);

            return eq;
        }

        public static bool operator ==(ValueTuple<T1, T2> a, ValueTuple<T1, T2> b) =>  a.Equals(b);
        public static bool operator !=(ValueTuple<T1, T2> a, ValueTuple<T1, T2> b) => !a.Equals(b);

        public static explicit operator      Tuple<T1, T2>(ValueTuple<T1, T2> vt) =>      Tuple.Create(vt.Item1, vt.Item2);
        public static explicit operator ValueTuple<T1, T2>(     Tuple<T1, T2>  t) => ValueTuple.Create( t.Item1,  t.Item2);
    }
}
