using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;

namespace PokeAPI.NET
{
    /// <summary>
    /// Effort value yield.
    /// </summary>
    public struct EvYield
    {
        /// <summary>
        /// The amount of EV yielded.
        /// </summary>
        public int Amount
        {
            get;
            private set;
        }
        /// <summary>
        /// The category.
        /// </summary>
        public string Category
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a new instance of the NameUriPair class
        /// </summary>
        /// <param name="amt">The default amout</param>
        /// <param name="cat">The default category</param>
        public EvYield(int amt, string cat)
        {
            this.Amount = amt;
            this.Category = cat;

        }

        /// <summary>
        ///  Returns the current instance represented through a string.
        /// </summary>
        /// <returns>A System.String that represents the current instance.</returns>
        public override string ToString()
        {
            return Amount + " " + Category;
        }

        /// <summary>
        /// Parses an EvYield object from a JsonData instance.
        /// </summary>
        /// <param name="j">The JsonData object.</param>
        /// <param name="key">The key of the JsonData that contains the EvYield string representation.</param>
        /// <returns>The EvYield parsed from the JsonData instance.</returns>
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
                    return new EvYield(0, String.Empty);
            }

            return new EvYield((int)v, String.Empty);
        }
    }
}
