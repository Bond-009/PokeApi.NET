using System;

namespace PokeAPI.NET
{
    public class PokemonParseException : Exception
    {
        const string DEFAULT_MESSAGE = "An error occured when parsing Pokémon data.";

        public PokemonParseException() : this(DEFAULT_MESSAGE, null) { }
        public PokemonParseException(string message) : this(message, null) { }
        public PokemonParseException(Exception innerException) : this(DEFAULT_MESSAGE, innerException) { }
        public PokemonParseException(string message, Exception innerException) : base(message, innerException) { }
    }
}
