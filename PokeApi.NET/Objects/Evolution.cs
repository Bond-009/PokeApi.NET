using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LitJson;

namespace PokeAPI
{
    public class Evolution : ApiResource
    {
        readonly static string
            RESU = "resource_uri",
            LVL  = "level",
            ITEM = "item",
            MTD  = "method",
            TO   = "to";

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
            : base(String.Empty, new Uri(BASE_URI + data[RESU]))
        {
            if (data.Keys.Contains(LVL))
                MethodPrecision = (int)data[LVL];
            if (data.Keys.Contains(ITEM))
                MethodPrecision = data[ITEM].ToString();

            Method = data[MTD].ToString().Replace('_', ' ');
            EvolveTo = data[TO].ToString();
        }

        public async Task<Pokemon> ToPokemon() => await Pokemon.GetInstanceAsync(EvolveTo);
    }
}
