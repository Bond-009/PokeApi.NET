using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LitJson;

namespace PokeAPI
{
    /// <summary>
    /// The base of all PokeAPI API objects.
    /// </summary>
    /// <typeparam name="T">The type deriving from <see cref="ApiObject{T}" />.</typeparam>
    [DebuggerDisplay("ID:{ID}, Name:{Name}, ResourceUri:{ResourceUri}, Created:{Created}, Modified:{LastModified}")]
    public abstract class ApiObject<T> : ApiResource
        where T : ApiObject<T>
    {
#pragma warning disable 3005
        protected internal readonly static string
            NAME = "name",
            RESU = "resource_uri",
            EMSG = "error_message",
            ID   = "id",
            CR_D = "created",
            MO_D = "modified";
#pragma warning restore 3005
        readonly static string
            OBR = "{#",
            CLN = ": ",
            CBR = "}";

        /// <summary>
        /// Gets the date when the <see cref="ApiObject{TApi}" /> was created.
        /// </summary>
        public DateTime Created
        {
            get;
            protected set;
        }
        /// <summary>
        /// Gets the <see cref="ApiObject{TApi}" />'s last modified date.
        /// </summary>
        public DateTime LastModified
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets whether the type deriving from <see cref="ApiObject{TApi}" /> uses the default parser for some fields or not. Default is false.
        /// </summary>
        protected virtual bool OverrideDefaultParsing => false;

        /// <summary>
        /// Creates a new instance of the <see cref="ApiObject{TApi}" /> class.
        /// </summary>
        protected ApiObject()
            : base(null, (Uri)null)
        {

        }

        /// <summary>
        /// Parses the <typeparamref name="T" /> from the given JSON data and returns it.
        /// </summary>
        /// <param name="source">The JSON data to parse.</param>
        /// <param name="ret">The <typeparamref name="T" /> that should be modified and returned.</param>
        /// <returns>The modified <typeparamref name="T" />, completed by the JSON data.</returns>
        public static T Create(JsonData source, T ret)
        {
            if (source.Keys.Contains(EMSG))
                throw new PokemonParseException(source[EMSG].ToString());

            try
            {
                if (!ret.OverrideDefaultParsing)
                {
                    ret.Name = source[NAME].ToString().Replace('-', ' ');
                    ret.ResourceUri = new Uri(BASE_URI + source[RESU].ToString());

                    if (source.Keys.Contains(ID)) // pokemon uses national_id, and the dex can't have an ID
                        ret.Id = (int)source[ID];

                    ret.Created      = DateTime.Parse((string)source[CR_D]);
                    ret.LastModified = DateTime.Parse((string)source[MO_D]);
                }

                ret.Create(source);

                return ret;
            }
            catch (Exception e)
            {
                throw new PokemonParseException(e);
            }
        }

        /// <summary>
        /// Parses JSON data as a bare <see cref="ApiResource" /> instance.
        /// </summary>
        /// <param name="data">The JSON data to parse.</param>
        /// <returns>The parsed JSON data as a <see cref="ApiResource" />.</returns>
        protected static ApiResource ParseResource(JsonData data) => new ApiResource(data[NAME].ToString().Replace('-', ' '), new Uri(BASE_URI + data[RESU].ToString()));

        /// <summary>
        /// Does parsing stuff in the derived class.
        /// </summary>
        /// <param name="source">The JSON data to parse.</param>
        protected abstract void Create(JsonData source);

        /// <summary>
        /// Gets the id of a PokeApi resource <see cref="Uri" />.
        /// </summary>
        /// <param name="resourceUri">The <see cref="Uri" /> to get the id from.</param>
        /// <returns>The id of the resource <see cref="Uri" />.</returns>
        public static int ResourceUriToId(Uri resourceUri) => Convert.ToInt32(resourceUri.AbsolutePath[resourceUri.AbsolutePath.Length - 2].ToString());
        /// <summary>
        /// Gets the id of a PokeApi resource <see cref="string" />.
        /// </summary>
        /// <param name="resourceUri">The <see cref="string" /> to get the id from.</param>
        /// <returns>The id of the resource <see cref="string" />.</returns>
        public static int ResourceUriToId(string resourceUri) => ResourceUriToId(new Uri(resourceUri));

        /// <summary>
        /// Serves as a hash function.
        /// </summary>
        /// <returns>A hash code for the current <see cref="object" />.</returns>
        public override int GetHashCode() => Name.GetHashCode() + ResourceUri.GetHashCode() + Id.GetHashCode() + Created.GetHashCode() + LastModified.GetHashCode();
        /// <summary>
        /// Returns a string that represents the current <see cref="object" />.
        /// </summary>
        /// <returns>A string that represents the current <see cref="object" />.</returns>
        public override string ToString() => OBR + Id + CLN + Name + CBR;
    }
}
