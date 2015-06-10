using LitJson;
using System;

namespace PokeAPI.NET
{
    /// <summary>
    /// A learnable move. The resource URI contains data to the actual <see cref="Move" /> instance.
    /// </summary>
    public class LearnableMove : NameUriPair
    {
        /// <summary>
        /// Gets the learn type of the move.
        /// </summary>
        public string LearnType
        {
            get;
            internal set;
        }
        = String.Empty;
        /// <summary>
        /// Gets the level where the move is learned at.
        /// </summary>
        public int Level
        {
            get;
            internal set;
        }
        = -1;

        /// <summary>
        /// Creates a new instance of the <see cref="LearnableMove" /> class.
        /// </summary>
        /// <param name="name">The name of the move.</param>
        /// <param name="uri">The resource URI pointing to the move instance.</param>
        public LearnableMove(string name, string uri)
            : this(name, new Uri(uri))
        {

        }
        /// <summary>
        /// Creates a new instance of the <see cref="LearnableMove" /> class.
        /// </summary>
        /// <param name="name">The name of the move.</param>
        /// <param name="uri">The resource URI pointing to the move instance.</param>
        public LearnableMove(string name, Uri uri)
            : base(name, uri)
        {

        }

        internal static LearnableMove Create(JsonData json)
        {
            LearnableMove lm = new LearnableMove(json["name"].ToString().Replace('-', ' '), "http://www.pokeapi.co" + json["resource_uri"].ToString());

            lm.LearnType = json["learn_type"].ToString();
            int lv;
            if (json.Keys.Contains("level") && Int32.TryParse(json["level"].ToString(), out lv))
                lm.Level = lv;

            return lm;
        }
    }
}
