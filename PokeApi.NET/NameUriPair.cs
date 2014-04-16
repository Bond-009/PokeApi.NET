using System;

namespace PokeAPI.NET
{
    /// <summary>
    /// A name/uri pair
    /// </summary>
    public struct NameUriPair
    {
        /// <summary>
        /// The name of the name/uri pair
        /// </summary>
        public string Name;
        /// <summary>
        /// The uri of the name/uri pair
        /// </summary>
        public Uri ResourceUri;

        /// <summary>
        /// Creates a new instance of the NameUriPair class
        /// </summary>
        /// <param name="name">The name of the name/uri pair</param>
        /// <param name="resourceUri">The uri of the name/uri pair</param>
        public NameUriPair(string name, Uri resourceUri)
        {
            Name = name;
            ResourceUri = resourceUri;
        }
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
    }
}
