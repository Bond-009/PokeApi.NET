using LitJson;
using System;

namespace PokeAPI
{
    public class LearnableMove : ApiResource
    {
        public string LearnType
        {
            get;
            internal set;
        }
        public int Level
        {
            get;
            internal set;
        }

        public LearnableMove(string name, string uri)
            : this(name, new Uri(uri))
        {

        }
        public LearnableMove(string name, Uri uri)
            : base(name, uri)
        {

        }

        internal static LearnableMove Parse(JsonData json)
        {
            var lm = new LearnableMove(json["name"].ToString().Replace('-', ' '), "http://www.pokeapi.co" + json["resource_uri"].ToString())
            {
                LearnType = json["learn_type"].ToString()
            };

            int lv = -1;

            if (json.Keys.Contains("level"))
                Int32.TryParse(json["level"].ToString(), out lv);

            lm.Level = lv;

            return lm;
        }
    }
}
