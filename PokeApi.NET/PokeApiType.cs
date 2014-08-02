using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LitJson;

namespace PokeAPI.NET
{
    /// <summary>
    /// The base of all PokeApi.NET API types.
    /// </summary>
    // how it's displayed in the debugger
    [DebuggerDisplay("ID:{ID}, Name:{Name}, ResourceUri:{ResourceUri}, Created:{Created}, Modified:{LastModified}")]
    public abstract class PokeApiType
    {
        /// <summary>
        /// The name of the PokeApiType instance
        /// </summary>
        public string Name
        {
            get;
            protected set;
        }
        /// <summary>
        /// The resource URI of the PokeApiType instance
        /// </summary>
        public Uri ResourceUri
        {
            get;
            protected set;
        }
        /// <summary>
        /// The ID of the PokeApiType instance
        /// </summary>
        public int ID
        {
            get;
            protected set;
        }
        /// <summary>
        /// The creation date of the PokeApiType instance
        /// </summary>
        public DateTime Created
        {
            get;
            protected set;
        }
        /// <summary>
        /// The last time the PokeApiType instance was modified
        /// </summary>
        public DateTime LastModified
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the name/uri pair of this PokeApiType instance.
        /// </summary>
        public NameUriPair NameUriPair
        {
            get
            {
                return new NameUriPair(Name, ResourceUri);
            }
        }

        /// <summary>
        /// Creates a PokeApiType from a JSON source
        /// </summary>
        /// <param name="source">The JSON source</param>
        /// <param name="ret">An object that inherits from PokeApiType</param>
        /// <returns>ret with the data from the JSON source</returns>
        public static void Create(JsonData source, PokeApiType ret)
        {
            if (source.Keys.Contains("error_message"))
                throw new PokemonParseException(source["error_message"].ToString());

            if (!ret.OverrideDefaultParsing())
            {
                ret.Name = source["name"].ToString().Replace('-', ' ');
                ret.ResourceUri = new Uri("http://www.pokeapi.co" + source["resource_uri"].ToString());
                if (source.Keys.Contains("id")) // pokemon uses national_id, and the dex can't have an ID
                    ret.ID = (int)source["id"];
                ret.Created = DateTime.Parse((string)source["created"]);
                ret.LastModified = DateTime.Parse((string)source["modified"]);
                //ret.Created = ParseDateString(source["created"].ToString());
                //ret.LastModified = ParseDateString(source["modified"].ToString());
            }

            ret.Create(source);
        }

        /// <summary>
        /// Parses a date string from a JSON source to a DateTime
        /// </summary>
        /// <param name="source">The JSON source to parse</param>
        /// <returns>The parsed JSON source as a DateTime</returns>
        [Obsolete("It turns out it's built-in in .NET")]
        protected static DateTime ParseDateString(string source)
        {
            string[] dateAndTime = source.Split('T');

            // source is eg. 2013-11-02T12:08:58.787000
            return new DateTime
                (Convert.ToInt32(dateAndTime[0].Split('-')[0]), // year
                Convert.ToInt32(dateAndTime[0].Split('-')[1]), // month
                Convert.ToInt32(dateAndTime[0].Split('-')[2]), // day

                Convert.ToInt32(dateAndTime[1].Split(':')[0]), // hour (24-h)
                Convert.ToInt32(dateAndTime[1].Split(':')[1]), // minute
                Convert.ToInt32(dateAndTime[1].Split(':')[2].Split('.')[0]), // second
                Convert.ToInt32(dateAndTime[1].Split(':')[2].Split('.')[1]) / 1000); // millisecond * 1000
        }
        /// <summary>
        /// Parses a JsonData object to a NameUriPair
        /// </summary>
        /// <param name="data">The JsonData object to parse</param>
        /// <returns>The parsed JsonData object as a NameUriPair</returns>
        protected static NameUriPair ParseNameUriPair(JsonData data)
        {
            return new NameUriPair(data["name"].ToString().Replace('-', ' '), new Uri("http://pokeapi.co" + data["resource_uri"].ToString()));
        }

        /// <summary>
        /// Creates a new instance from a JSON object
        /// </summary>
        /// <param name="source">The JSON object where to create the new instance from</param>
        protected abstract void Create(JsonData source);
        /// <summary>
        /// Wether to override default parsing (creation, name, id, ...) or not
        /// </summary>
        /// <returns>true if default parsing should be overridden, false otherwise.</returns>
        protected virtual bool OverrideDefaultParsing()
        {
            return false;
        }

        /// <summary>
        /// Gets the ID of the PokeApiType from a resource Uri
        /// </summary>
        /// <param name="resourceUri">The resource Uri to get the ID from</param>
        /// <returns>The ID of the PokeApiType from the resource Uri</returns>
        public static int ResourceUriToID(Uri resourceUri)
        {
            return Convert.ToInt32(resourceUri.AbsolutePath[resourceUri.AbsolutePath.Length - 2].ToString());
        }
        /// <summary>
        /// Gets the ID of the PokeApiType from a resource Uri
        /// </summary>
        /// <param name="resourceUri">The resource Uri to get the ID from</param>
        /// <returns>The ID of the PokeApiType from the resource Uri</returns>
        public static int ResourceUriToID(string resourceUri)
        {
            return ResourceUriToID(new Uri(resourceUri));
        }

        /// <summary>
        /// Checks equality to another object
        /// </summary>
        /// <param name="obj">The other object to compare to</param>
        /// <returns>true if the objects are considered equal, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            return obj is PokeApiType && (PokeApiType)obj == this;
        }
        /// <summary>
        /// Gets the hash code for the current instance
        /// </summary>
        /// <returns>The hash code for the current instance</returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode() + ResourceUri.GetHashCode() + ID.GetHashCode() + Created.GetHashCode() + LastModified.GetHashCode();
        }
        /// <summary>
        /// Converts the current instance to a string
        /// </summary>
        /// <returns>The current instance as a string</returns>
        public override string ToString()
        {
            return "{" + ID + ": " + Name + "}";
        }

        /// <summary>
        /// Implicitely casts a PokeApiType to a NameUriPair.
        /// </summary>
        /// <param name="pat">The PokeApiType to cast.</param>
        public static implicit operator NameUriPair(PokeApiType pat)
        {
            return pat.NameUriPair;
        }
    }
}
