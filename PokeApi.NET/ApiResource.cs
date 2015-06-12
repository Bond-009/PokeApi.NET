using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokeAPI
{
    /// <summary>
    /// Represents a resource from the http://pokeapi.co/ website.
    /// </summary>
    public class ApiResource : IEquatable<ApiResource>
    {
        readonly static string
            NAME = "{ Name: ",
            RESU = ", ResourceUri: ",
            CLBR = "}",

            TYPE_ERR = "Type mismatch.";
        protected internal readonly static string BASE_URI = "http://pokeapi.co";

        readonly static char[] SplitChars = { '/' };

        /// <summary>
        /// Gets the resource name.
        /// </summary>
        public string Name
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the resource <see cref="Uri" />.
        /// </summary>
        public Uri ResourceUri
        {
            get;
            protected set;
        }
        /// <summary>
        /// Gets the id of the resource.
        /// </summary>
        /// <remarks>When not set in a deriving class, this will be the id specified in <see cref="ResourceUri" />.</remarks>
        public int Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ApiResource" /> class.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        /// <param name="resourceUri">the uri of the resource as a <see cref="string"/>.</param>
        public ApiResource(string name, string resourceUri)
            : this(name, new Uri(resourceUri))
        {

        }
        /// <summary>
        /// Creates a new instance of the <see cref="ApiResource" /> class.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        /// <param name="resourceUri">the uri of the resource as a <see cref="Uri" />.</param>
        public ApiResource(string name, Uri    resourceUri)
        {
            ResourceUri = resourceUri;
            Name        = name       ;

            var num = resourceUri.ToString().Split(SplitChars, StringSplitOptions.RemoveEmptyEntries);
            Id = Int32.Parse(num[num.Length - 1]);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" /> is equal to the current <see cref="object" />.
        /// </summary>
        /// <param name="obj">The object to compare with the current <see cref="object" />.</param>
        /// <returns>true if the specified <see cref="object" /> is equal to the current <see cref="object" />; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ApiResource))
                return false;

            var r = (ApiResource)obj;

            return Equals((ApiResource)obj);
        }
        /// <summary>
        /// Serves as a hash function.
        /// </summary>
        /// <returns>A hash code for the current <see cref="object" />.</returns>
        public override int GetHashCode() => Name.GetHashCode() | ResourceUri.GetHashCode();
        /// <summary>
        /// Returns a string that represents the current <see cref="object" />.
        /// </summary>
        /// <returns>A string that represents the current <see cref="object" />.</returns>
        public override string ToString() => NAME + Name + RESU + ResourceUri + CLBR;

        /// <summary>
        /// Gets the <see cref="ApiObject{TApi}" /> represented by this <see cref="ApiResource" /> asynchronously.
        /// </summary>
        /// <typeparam name="TApi">the type of <see cref="ApiObject{TApi}" /> to return.</typeparam>
        /// <returns>The <see cref="ApiObject{TApi}" /> represented by thos <see cref="ApiResource" />.</returns>
        /// <remarks>If this is an object that derives from <see cref="ApiResource" />, no request is sent and the object itself is returned.</remarks>
        /// <exception cref="ArgumentException"><typeparamref name="TApi" /> is not the equivalent of the type represented by this <see cref="ApiResource" />.</exception>
        public async Task<TApi> GetResource<TApi>()
            where TApi : ApiObject<TApi>
        {
            if (GetType() != typeof(ApiResource) && GetType() == typeof(TApi))
                return (TApi)this;

            string[] split = ResourceUri.ToString().Split(SplitChars, StringSplitOptions.RemoveEmptyEntries);
            string type = split[split.Length - 2].ToLowerInvariant();
            string requested = split[split.Length - 1].ToLowerInvariant();
            int i;
            int? reqInt = Int32.TryParse(requested, out i) ? (int?)i : null;

            switch (type)
            {
                case "pokemon":
                    if (typeof(TApi) != typeof(Pokemon))
                        throw new ArgumentException(TYPE_ERR, nameof(TApi));

                    return (TApi)(object)(await (reqInt.HasValue
                        ? Pokemon.GetInstance(reqInt.Value)
                        : Pokemon.GetInstance(requested)));
                case "type":
                    if (typeof(TApi) != typeof(PokemonType))
                        throw new ArgumentException(TYPE_ERR, nameof(TApi));

                    return (TApi)(object)(await (reqInt.HasValue
                        ? PokemonType.GetInstance(reqInt.Value)
                        : PokemonType.GetInstance(requested)));
                case "move":
                    if (typeof(TApi) != typeof(Move))
                        throw new ArgumentException(TYPE_ERR, nameof(TApi));

                    return (TApi)(object)(await (reqInt.HasValue
                        ? Move.GetInstance(reqInt.Value)
                        : Move.GetInstance(requested)));
                case "ability":
                    if (typeof(TApi) != typeof(Ability))
                        throw new ArgumentException(TYPE_ERR, nameof(TApi));

                    return (TApi)(object)(await (reqInt.HasValue
                        ? Ability.GetInstance(reqInt.Value)
                        : Ability.GetInstance(requested)));
                case "egg":
                    if (typeof(TApi) != typeof(EggGroup))
                        throw new ArgumentException(TYPE_ERR, nameof(TApi));

                    return (TApi)(object)(await (reqInt.HasValue
                        ? EggGroup.GetInstance(reqInt.Value)
                        : EggGroup.GetInstance(requested)));
                case "description":
                    if (typeof(TApi) != typeof(Description))
                        throw new ArgumentException(TYPE_ERR, nameof(TApi));

                    return (TApi)(object)(await Description.GetInstance(reqInt.Value));
                case "sprite":
                    if (typeof(TApi) != typeof(Sprite))
                        throw new ArgumentException(TYPE_ERR, nameof(TApi));

                    return (TApi)(object)(await (reqInt.HasValue
                        ? Sprite.GetInstance(reqInt.Value)
                        : Sprite.GetInstance(requested)));
                case "game":
                    if (typeof(TApi) != typeof(Game))
                        throw new ArgumentException(TYPE_ERR, nameof(TApi));

                    return (TApi)(object)(await (reqInt.HasValue
                        ? Game.GetInstance(reqInt.Value)
                        : Game.GetInstance(requested)));
            }

            return null;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="r">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
        public bool Equals(ApiResource r) => Name == r.Name && ResourceUri == r.ResourceUri;
    }
}
