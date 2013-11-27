using System;

namespace PokeAPI.NET
{
    public static class PokeExtensions
    {
        static bool IsPowerOfTwo(int x)
        {
            return x != 0 && (x & (x - 1)) == 0;
        }

        /// <summary>
        /// Converts a PokemonType to it's ID
        /// </summary>
        /// <param name="type">The PokemonType to convert</param>
        /// <returns>The converted PokemonType</returns>
        public static int ID(this PokemonType type)
        {
            if (!IsPowerOfTwo((int)type)) // multiple types
                return 0;

            int id = 1;
            for (int i = 1; i <= 131072; i *= 2) // lazy again
            {
                if (((int)type & i) == i)
                    return id;

                id++;
            }

            return 0;
        }
    }
}
