using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;

namespace PokeAPI
{
    public struct EvYield
    {
        readonly static string SPACE = " ";

        public int Amount
        {
            get;
        }
        public string Category
        {
            get;
        }

        public EvYield(int amt, string cat)
        {
            this.Amount = amt;
            this.Category = cat;
        }

        public override string ToString() => Amount + SPACE + Category;

        public static EvYield Parse(JsonData j, string key = "ev_yield")
        {
            JsonData v = j[key];

            if (v.GetJsonType() == JsonType.String)
            {
                string val = (string)v;

                string[] split = val.Split(' ');
                int amt;

                if (split.Length > 1)
                {
                    string cat = split[1];

                    for (int i = 2; i < split.Length; i++)
                        cat += " " + split[i];

                    if (Int32.TryParse(split[0], out amt))
                        return new EvYield(amt, cat);
                    else
                        return new EvYield(0, cat);
                }
                else if (split.Length > 0 && Int32.TryParse(split[0], out amt))
                    return new EvYield(amt, String.Empty);
                else
                    return new EvYield(  0, String.Empty);
            }

            return new EvYield((int)v, String.Empty);
        }
    }
}
