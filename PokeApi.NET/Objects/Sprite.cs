using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LitJson;

namespace PokeAPI
{
    /// <summary>
    /// Represents the sprite of a <see cref="PokeAPI.Pokemon" />.
    /// </summary>
    public class Sprite : ApiObject<Sprite>
    {
        readonly static string
            PKMN = "pokemon",
            IMG  = "image";

        static readonly Cache<int, Sprite> cache = new Cache<int, Sprite>(async i => Maybe.Just(Create(await DataFetcher.GetSprite(i), new Sprite())));

        /// <summary>
        /// Gets the <see cref="Sprite" /> instance cache.
        /// </summary>
        public static CacheGetter<int, Sprite> Cache { get; } = new CacheGetter<int, Sprite>(cache);

        /// <summary>
        /// Gets the Pokemon that is shown in the <see cref="Sprite" />.
        /// </summary>
        public ApiResource Pokemon
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the <see cref="Uri" /> pointing to the actual sprite image.
        /// </summary>
        public Uri ImageUri
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the image represented by a <see cref="byte" /> array (A PNG file).
        /// </summary>
        /// <remarks>Remains null until <see cref="LoadImageData" /> is called.</remarks>
        public byte[] ImageData
        {
            get;
            private set;
        }

        Sprite()
        {
        }

        /// <summary>
        /// Loads the image and puts its content in <see cref="ImageData" /> asynchronously.
        /// </summary>
        public async void LoadImageData()
        {
            if (ImageData == null)
            {
                var s = await DataFetcher.client.GetStreamAsync(ImageUri);

                if (s is MemoryStream)
                    ImageData = ((MemoryStream)s).ToArray();
                else
                    using (var ms = new MemoryStream((int)(s.Length & ((1L << 32) - 1L))))
                    {
                        s.CopyTo(ms);

                        ImageData = ms.ToArray();
                    }
            }
        }

        /// <summary>
        /// Does parsing stuff in the derived class.
        /// </summary>
        /// <param name="source">The JSON data to parse.</param>
        protected override void Create(JsonData source)
        {
            Pokemon = ParseResource(source[PKMN]);
            ImageUri = new Uri(BASE_URI + source[IMG].ToString());
        }

        /// <summary>
        /// Gets the <see cref="PokeAPI.Pokemon" /> instance represented by <see cref="Pokemon" /> asynchronously.
        /// </summary>
        /// <returns>A task containing the <see cref="PokeAPI.Pokemon" />.</returns>
        public async Task<Pokemon> RefPokemon() => await PokeAPI.Pokemon.GetInstanceAsync(Pokemon.Id);

        /// <summary>
        /// Gets a <see cref="Sprite" /> instance from its name asynchronously.
        /// </summary>
        /// <param name="name">The name of the Pokemon who's <see cref="Sprite" /> should be returned.</param>
        /// <returns>A task containing the <see cref="Sprite" /> instance.</returns>
        public static async Task<Sprite> GetInstanceAsync(string  name) => await GetInstanceAsync(PokeAPI.Pokemon.Ids[name.ToLowerInvariant()] + 1);
        /// <summary>
        /// Gets a <see cref="Sprite" /> instance from a <see cref="PokeAPI.Pokemon" /> instance asynchronously.
        /// </summary>
        /// <param name="pkmn">The <see cref="PokeAPI.Pokemon" /> who's <see cref="Sprite" /> should be returned.</param>
        /// <returns>A task containing the <see cref="Sprite" /> instance.</returns>
        public static async Task<Sprite> GetInstanceAsync(Pokemon pkmn) => await GetInstanceAsync(pkmn.Id + 1);
        /// <summary>
        /// Gets a <see cref="Sprite" /> instance from its id asynchronously.
        /// </summary>
        /// <param name="id">The id of the <see cref="Sprite" />.</param>
        /// <returns>A task containing the <see cref="Sprite" /> instance.</returns>
        public static async Task<Sprite> GetInstanceAsync(int     id  ) => await cache.Get(id);
    }
}
