using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using LitJson;

namespace PokeAPI
{
    /// <summary>
    /// Represents an instance of a Pokemon Move.
    /// </summary>
    public partial class Move : ApiObject<Move>
    {
        readonly static string
            POW  = "power",
            PP_C = "pp",
            DESC = "description",
            ACC  = "accuracy",
            CAT  = "category";

        static readonly Cache<int, Move> cache = new Cache<int, Move>(async i => Maybe.Just(Create(await DataFetcher.GetMove(i), new Move())));

        /// <summary>
        /// Gets the <see cref="Move" /> instance cache.
        /// </summary>
        public static CacheGetter<int, Move> Cache { get; } = new CacheGetter<int, Move>(cache);

        /// <summary>
        /// Gets the description of the <see cref="Move" />.
        /// </summary>
        public string Description
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the power of the <see cref="Move" />.
        /// </summary>
        public int Power
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the amount of power points the <see cref="Move" /> has.
        /// </summary>
        public int PP
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the accuracy of the <see cref="Move" /> as a percentage.
        /// </summary>
        public double Accurracy
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the category of the <see cref="Move" />.
        /// </summary>
        public MoveCategory Category
        {
            get;
            private set;
        }

        Move()
        {
        }

        /// <summary>
        /// Does parsing stuff in the derived class.
        /// </summary>
        /// <param name="source">The JSON data to parse.</param>
        protected override void Create(JsonData source)
        {
            Power = source.AsInt(POW );
            PP    = source.AsInt(PP_C);

            Description = source[DESC].ToString();
            Accurracy = Double.Parse(source[ACC].ToString(), CultureInfo.InvariantCulture) * 0.01d;

            MoveCategory cat;
            Enum.TryParse(source[CAT].ToString(), true, out cat);

            Category = cat;
        }

        /// <summary>
        /// Gets a <see cref="Move" /> instance from its name asynchronously.
        /// </summary>
        /// <param name="name">The name of the <see cref="Move" /> that should be returned.</param>
        /// <returns>A task containing the <see cref="Move" /> instance.</returns>
        public static async Task<Move> GetInstanceAsync(string name) => await GetInstanceAsync(Ids[name]);
        /// <summary>
        /// Gets a <see cref="Move" /> instance from its id asynchronously.
        /// </summary>
        /// <param name="id">The id of the <see cref="Move" />.</param>
        /// <returns>A task containing the <see cref="Move" /> instance.</returns>
        public static async Task<Move> GetInstanceAsync(int    id  ) => await cache.Get(id);
    }
}
