using System;

namespace PokeAPI.NET
{
    /// <summary>
    /// A name/uri pair
    /// </summary>
    public struct NameUriPair(string name, Uri resourceUri)
    {
        /// <summary>
        /// The name of the name/uri pair
        /// </summary>
        public string Name
        {
            get;
            set;
        } = name;
        /// <summary>
        /// The uri of the name/uri pair
        /// </summary>
        public Uri ResourceUri
        {
            get;
            set;
        } = resourceUri;

        /// <summary>
        /// Creates a new instance of the NameUriPair class
        /// </summary>
        /// <param name="name">The name of the name/uri pair</param>
        /// <param name="resourceUri">The uri of the name/uri pair</param>
        public NameUriPair(string name, string resourceUri)
            : this(name, new Uri(resourceUri))
        {

        }

        /// <summary>
        /// Checks equality to another object
        /// </summary>
        /// <param name="obj">The other object to compare to</param>
        /// <returns>true if the objects are considered equal, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            return obj is NameUriPair && (NameUriPair)obj == this;
        }
        /// <summary>
        /// Gets the hash code for the current instance
        /// </summary>
        /// <returns>The hash code for the current instance</returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode() | ResourceUri.GetHashCode();
        }
        /// <summary>
        /// Converts the current instance to a string
        /// </summary>
        /// <returns>The current instance as a string</returns>
        public override string ToString()
        {
            return "{ Name: " + Name + ", ResourceUri: " + ResourceUri + "}";
        }

        /// <summary>
        /// Gets the resource where ResourceUri points to, as a PokeApiType.
        /// </summary>
        /// <returns>The resource where ResourceUri points to.</returns>
        public PokeApiType GetResource()
        {
            string[] split = ResourceUri.ToString().Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            string type = split[split.Length - 2].ToLower();
            string requested = split[split.Length - 1].ToLower();
            int? reqInt = Int32.TryParse(requested, out int i) ? (int?)i : null;

            switch (type)
            {
                case "pokemon":
                    return reqInt.HasValue
                        ? Pokemon.GetInstance(reqInt.Value)
                        : Pokemon.GetInstance(requested);
                case "type":
                    return reqInt.HasValue
                        ? PokemonType.GetInstance(reqInt.Value)
                        : PokemonType.GetInstance(requested);
                case "move":
                    return reqInt.HasValue
                        ? Move.GetInstance(reqInt.Value)
                        : Move.GetInstance(requested);
                case "ability":
                    return reqInt.HasValue
                        ? Ability.GetInstance(reqInt.Value)
                        : Ability.GetInstance(requested);
                case "egg":
                    return reqInt.HasValue
                        ? EggGroup.GetInstance(reqInt.Value)
                        : EggGroup.GetInstance(requested);
                case "description":
                    return Description.GetInstance(reqInt.Value);
                case "sprite":
                    return reqInt.HasValue
                        ? Sprite.GetInstance(reqInt.Value)
                        : Sprite.GetInstance(requested);
                case "game":
                    return reqInt.HasValue
                        ? Game.GetInstance(reqInt.Value)
                        : Game.GetInstance(requested);
            }

            return null;
        }
        /// <summary>
        /// Gets the resource where ResourceUri points to, as a <typeparam name="TPokeApiType" />.
        /// </summary>
        /// <returns>The resource where ResourceUri points to.</returns>
        public TPokeApiType GetResource<TPokeApiType>()
            where TPokeApiType : PokeApiType
        {
            return (TPokeApiType)GetResource();
        }

        /// <summary>
        /// Checks wether two name/uri pairs are considered equal or not
        /// </summary>
        /// <param name="a">The first name/uri pair</param>
        /// <param name="b">The second name/uri pair</param>
        /// <returns>true if the name/uri pairs are considered equal, false otherwise.</returns>
        public static bool operator ==(NameUriPair a, NameUriPair b)
        {
            return a.Name == b.Name && a.ResourceUri == b.ResourceUri;
        }
        /// <summary>
        /// Checks wether two name/uri pairs are considered inequal or not
        /// </summary>
        /// <param name="a">The first name/uri pair</param>
        /// <param name="b">The second name/uri pair</param>
        /// <returns>true if the name/uri pairs are considered inequal, false otherwise.</returns>
        public static bool operator !=(NameUriPair a, NameUriPair b)
        {
            return a.Name != b.Name || a.ResourceUri != b.ResourceUri;
        }

        /// <summary>
        /// Casts an instance of the NameUriPair class to a PokeApiType.
        /// </summary>
        /// <param name="nup">The NameUriPair to cast.</param>
        public static implicit operator PokeApiType(NameUriPair nup)
        {
            return nup.GetResource();
        }
    }
}
