using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LitJson;

namespace PokeAPI
{
    public class Sprite : ApiObject<Sprite>
    {
        static Cache<int, Sprite> cache = new Cache<int, Sprite>(async i => Maybe.Just(Create(await DataFetcher.GetSprite(i), new Sprite())));

        public static bool ShouldCacheData
        {
            get
            {
                return cache.IsActive;
            }
            set
            {
                cache.IsActive = value;
            }
        }

        public ApiResource Pokemon
        {
            get;
            private set;
        }

        public Uri ImageUri
        {
            get;
            private set;
        }
        public byte[] ImageData
        {
            get;
            private set;
        }

        private Sprite() { }

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

        protected override void Create(JsonData source)
        {
            Pokemon = ParseResource(source["pokemon"]);
            ImageUri = new Uri("http://www.pokeapi.co" + source["image"].ToString());
        }

        public async Task<Pokemon> RefPokemon() => await PokeAPI.Pokemon.GetInstance(Pokemon.Name);

        public static async Task<Sprite> GetInstance(string  name) => await GetInstance(PokeAPI.Pokemon.IDs[name.ToLower()] + 1);
        public static async Task<Sprite> GetInstance(Pokemon pkmn) => await GetInstance(pkmn.Id + 1);
        public static async Task<Sprite> GetInstance(int     id  ) => await cache.Get(id);
    }
}
