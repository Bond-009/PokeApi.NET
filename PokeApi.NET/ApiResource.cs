using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokeAPI
{
    public class ApiResource : IEquatable<ApiResource>
    {
        /// <summary>
        /// The resource name
        /// </summary>
        public string Name
        {
            get;
            protected set;
        }

        /// <summary>
        /// The resource Uri
        /// </summary>
        public Uri ResourceUri
        {
            get;
            protected set;
        }
        public int Id
        {
            get;
            protected set;
        }

        public ApiResource(string name, string resourceUri)
            : this(name, new Uri(resourceUri))
        {

        }
        public ApiResource(string name, Uri    resourceUri)
        {
            ResourceUri = resourceUri;
            Name        = name       ;

            var num = resourceUri.ToString().Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            Id = Int32.Parse(num[num.Length - 1]);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ApiResource))
                return false;

            var r = (ApiResource)obj;

            return Equals((ApiResource)obj);
        }
        public override int GetHashCode() => Name.GetHashCode() | ResourceUri.GetHashCode();
        public override string ToString() => "{ Name: " + Name + ", ResourceUri: " + ResourceUri + "}";

        public async Task<TApi> GetResource<TApi>()
            where TApi : ApiObject<TApi>
        {
            string[] split = ResourceUri.ToString().Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            string type = split[split.Length - 2].ToLower();
            string requested = split[split.Length - 1].ToLower();
            int i;
            int? reqInt = Int32.TryParse(requested, out i) ? (int?)i : null;

            switch (type)
            {
                case "pokemon":
                    if (typeof(TApi) != typeof(Pokemon))
                        throw new ArgumentException("Type mismatch.", nameof(TApi));

                    return (TApi)(object)(await (reqInt.HasValue
                        ? Pokemon.GetInstance(reqInt.Value)
                        : Pokemon.GetInstance(requested)));
                case "type":
                    if (typeof(TApi) != typeof(PokemonType))
                        throw new ArgumentException("Type mismatch.", nameof(TApi));

                    return (TApi)(object)(await (reqInt.HasValue
                        ? PokemonType.GetInstance(reqInt.Value)
                        : PokemonType.GetInstance(requested)));
                case "move":
                    if (typeof(TApi) != typeof(Move))
                        throw new ArgumentException("Type mismatch.", nameof(TApi));

                    return (TApi)(object)(await (reqInt.HasValue
                        ? Move.GetInstance(reqInt.Value)
                        : Move.GetInstance(requested)));
                case "ability":
                    if (typeof(TApi) != typeof(Ability))
                        throw new ArgumentException("Type mismatch.", nameof(TApi));

                    return (TApi)(object)(await (reqInt.HasValue
                        ? Ability.GetInstance(reqInt.Value)
                        : Ability.GetInstance(requested)));
                case "egg":
                    if (typeof(TApi) != typeof(EggGroup))
                        throw new ArgumentException("Type mismatch.", nameof(TApi));

                    return (TApi)(object)(await (reqInt.HasValue
                        ? EggGroup.GetInstance(reqInt.Value)
                        : EggGroup.GetInstance(requested)));
                case "description":
                    if (typeof(TApi) != typeof(Description))
                        throw new ArgumentException("Type mismatch.", nameof(TApi));

                    return (TApi)(object)(await Description.GetInstance(reqInt.Value));
                case "sprite":
                    if (typeof(TApi) != typeof(Sprite))
                        throw new ArgumentException("Type mismatch.", nameof(TApi));

                    return (TApi)(object)(await (reqInt.HasValue
                        ? Sprite.GetInstance(reqInt.Value)
                        : Sprite.GetInstance(requested)));
                case "game":
                    if (typeof(TApi) != typeof(Game))
                        throw new ArgumentException("Type mismatch.", nameof(TApi));

                    return (TApi)(object)(await (reqInt.HasValue
                        ? Game.GetInstance(reqInt.Value)
                        : Game.GetInstance(requested)));
            }

            return null;
        }

        public bool Equals(ApiResource r) => Name == r.Name && ResourceUri == r.ResourceUri;
    }
}
