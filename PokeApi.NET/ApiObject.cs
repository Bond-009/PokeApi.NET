using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LitJson;

namespace PokeAPI
{
    /// <summary>
    /// The base of all PokeApi.NET API types.
    /// </summary>
    // how it's displayed in the debugger
    [DebuggerDisplay("ID:{ID}, Name:{Name}, ResourceUri:{ResourceUri}, Created:{Created}, Modified:{LastModified}")]
    public abstract class ApiObject<TApi> : ApiResource
        where TApi : ApiObject<TApi>
    {
        public DateTime Created
        {
            get;
            protected set;
        }
        public DateTime LastModified
        {
            get;
            protected set;
        }

        protected virtual bool OverrideDefaultParsing => false;

        protected ApiObject()
            : base(null, (Uri)null)
        {

        }

        public static TApi Create(JsonData source, TApi ret)
        {
            if (source.Keys.Contains("error_message"))
                throw new PokemonParseException(source["error_message"].ToString());

            if (!ret.OverrideDefaultParsing)
            {
                ret.Name = source["name"].ToString().Replace('-', ' ');
                ret.ResourceUri = new Uri("http://www.pokeapi.co" + source["resource_uri"].ToString());

                if (source.Keys.Contains("id")) // pokemon uses national_id, and the dex can't have an ID
                    ret.Id = (int)source["id"];

                ret.Created = DateTime.Parse((string)source["created"]);
                ret.LastModified = DateTime.Parse((string)source["modified"]);
            }

            ret.Create(source);

            return ret;
        }

        protected static ApiResource ParseResource(JsonData data) => new ApiResource(data["name"].ToString().Replace('-', ' '), new Uri("http://pokeapi.co" + data["resource_uri"].ToString()));

        protected abstract void Create(JsonData source);

        public static int ResourceUriToId(Uri resourceUri) => Convert.ToInt32(resourceUri.AbsolutePath[resourceUri.AbsolutePath.Length - 2].ToString());
        public static int ResourceUriToId(string resourceUri) => ResourceUriToId(new Uri(resourceUri));

        public override int GetHashCode() => Name.GetHashCode() + ResourceUri.GetHashCode() + Id.GetHashCode() + Created.GetHashCode() + LastModified.GetHashCode();
        public override string ToString() => "{#" + Id + ": " + Name + "}";
    }
}
