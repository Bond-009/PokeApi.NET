using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LitJson;

namespace PokeAPI
{
    public class Evolution : ApiResource
    {
        public object MethodPrecision
        {
            get;
        }
        public string Method
        {
            get;
        }
        public string EvolveTo
        {
            get;
        }

        public Evolution(JsonData data)
            : base(String.Empty, new Uri("http://www.pokeapi.co/" + data["resource_uri"]))
        {
            if (data.Keys.Contains("level"))
                MethodPrecision = (int)data["level"];
            if (data.Keys.Contains("item"))
                MethodPrecision = data["item"].ToString();

            Method = data["method"].ToString().Replace('_', ' ');
            EvolveTo = data["to"].ToString();
        }

        public async Task<Pokemon> ToPokemon() => await Pokemon.GetInstance(EvolveTo);
    }
}
