using LitJson;
using System;

namespace PokeAPI
{
    public class LearnableMove : ApiResource
    {
        readonly static string
            NAME = "name",
            RESU = "resource_uri",
            L_TP = "learn_type",
            LVL  = "level";

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
            var lm = new LearnableMove(json[NAME].ToString().Replace('-', ' '), BASE_URI + json[RESU].ToString())
            {
                LearnType = json[L_TP].ToString()
            };

            int lv = -1;

            if (json.Keys.Contains(LVL))
                Int32.TryParse(json[LVL].ToString(), out lv);

            lm.Level = lv;

            return lm;
        }
    }
}
