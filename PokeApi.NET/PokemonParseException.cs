using System;

namespace PokeAPI.NET
{
    /// <summary>
    /// An exception thrown when parsing a PokéApi JSON object
    /// </summary>
    public class PokemonParseException : Exception
    {
        const string DEFAULT_MESSAGE = "An error occured when parsing Pokémon data.";

        /// <summary>
        /// Creates a new instance of the PokemonParseException class
        /// </summary>
        public PokemonParseException() : this(DEFAULT_MESSAGE, null) { }
        /// <summary>
        /// Creates a new instance of the PokemonParseException class
        /// </summary>
        /// <param name="message">The message of the Exception</param>
        public PokemonParseException(string message) : this(message, null) { }
        /// <summary>
        /// Creates a new instance of the PokemonParseException class
        /// </summary>
        /// <param name="innerException">The exception that was the cause of this exception</param>
        public PokemonParseException(Exception innerException) : this(DEFAULT_MESSAGE, innerException) { }
        /// <summary>
        /// Creates a new instance of the PokemonParseException class
        /// </summary>
        /// <param name="message">The message of the Exception</param>
        /// <param name="innerException">The exception that was the cause of this exception</param>
        public PokemonParseException(string message, Exception innerException) : base(message, innerException) { }
    }
}
