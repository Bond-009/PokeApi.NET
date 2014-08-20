using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using LitJson;

namespace PokeAPI.NET
{
    /// <summary>
    /// Represents the sprite of a Pokémon
    /// </summary>
    public class Sprite : PokeApiType
    {
        /// <summary>
        /// Wether it should cache sprites or not
        /// </summary>
        public static bool ShouldCacheData = true;
        /// <summary>
        /// Gets all cached sprites
        /// </summary>
        public static Dictionary<int, Sprite> CachedSprites = new Dictionary<int, Sprite>();

        bool creating = true, loadImg = false;
        byte[] imgData = null;

        /// <summary>
        /// Creates a new instance of the Sprite class
        /// </summary>
        /// <param name="loadImageAfterCreation">Wether it should load the images directly after creation or not</param>
        public Sprite(bool loadImageAfterCreation = false)
        {
            LoadImageAfterCreation = loadImageAfterCreation;
        }

        /// <summary>
        /// Wether the ImageData array should be loaded after the object is created or not.
        /// Can only be set before the PokeApiType is created.
        /// </summary>
        public bool LoadImageAfterCreation
        {
            get
            {
                return loadImg;
            }
            set
            {
                if (!creating)
                    throw new InvalidOperationException("LoadImageAftercreation should only be called before the Sprite is initialized");

                creating = value;
            }
        }

        /// <summary>
        /// The Pokemon this Sprite instance is for
        /// </summary>
        public NameUriPair Pokemon
        {
            get;
            private set;
        }
        /// <summary>
        /// The Pokemon property as a Pokemon
        /// </summary>
        public Pokemon RefPokemon
        {
            get
            {
                return NET.Pokemon.GetInstance(Pokemon.Name);
            }
        }
        /// <summary>
        /// The Uri of the image file
        /// </summary>
        public Uri ImageUri
        {
            get;
            private set;
        }

        /// <summary>
        /// The image as a byte array.
        /// Only exists if LoadImageData is called.
        /// </summary>
        public byte[] ImageData
        {
            get
            {
                if (imgData == null)
                    LoadImageData();

                return imgData;
            }
        }
        /// <summary>
        /// The image as a System.Drawing.Bitmap. (Windows Forms/GDI+)
        /// Only exists if LoadImageData is called.
        /// </summary>
        public Bitmap ImageAsBitmap
        {
            get
            {
                Bitmap b;

                using (MemoryStream ms = new MemoryStream(ImageData))
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    b = new Bitmap(ms);
                    ms.Close();
                }

                return b;
            }
        }
        /// <summary>
        /// The image as a System.Windows.Media.Imaging.BitmapImage. (WPF/DirectX)
        /// Only exists if LoadImageData is called.
        /// </summary>
        public BitmapImage ImageAsBitmapImage
        {
            get
            {
                BitmapImage b;

                using (MemoryStream ms = new MemoryStream(ImageData))
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    b = new BitmapImage();
                    b.BeginInit();
                    b.StreamSource = ms;
                    b.CacheOption = BitmapCacheOption.OnLoad;
                    b.EndInit();
                    ms.Close();
                }

                return b;
            }
        }

        /// <summary>
        /// Loads the image data.
        /// Automatically called when LoadImageAfterCreation is true before Create was called.
        /// </summary>
        public void LoadImageData()
        {
            if (imgData == null)
                imgData = DataFetcher.client.DownloadData(ImageUri);
        }

        /// <summary>
        /// Creates a new instance from a JSON object
        /// </summary>
        /// <param name="source">The JSON object where to create the new instance from</param>
        protected override void Create(JsonData source)
        {
            Pokemon = ParseNameUriPair(source["pokemon"]);
            ImageUri = new Uri("http://www.pokeapi.co" + source["image"].ToString());

            if (LoadImageAfterCreation)
                LoadImageData();

            creating = false;
        }

        /// <summary>
        /// Creates an instance of a Sprite with the given name
        /// </summary>
        /// <param name="name">The name of the Sprite to instantiate</param>
        /// <param name="loadImageAfterCreation">Wether the image data should be loaded directly after the Sprite has been created or not</param>
        /// <returns>The created instance of the Sprite</returns>
        public static Sprite GetInstance(string  name, bool loadImageAfterCreation = false)
        {
            return GetInstance(NET.Pokemon.IDs[name.ToLower()] + 1, loadImageAfterCreation);
        }
        /// <summary>
        /// Creates an instance of a Sprite with the given ID
        /// </summary>
        /// <param name="id">The id of the Sprite to instantiate</param>
        /// <param name="loadImageAfterCreation">Wether the image data should be loaded directly after the Sprite has been created or not</param>
        /// <returns>The created instance of the Sprite</returns>
        public static Sprite GetInstance(int     id  , bool loadImageAfterCreation = false)
        {
            if (CachedSprites.ContainsKey(id))
                return CachedSprites[id];

            Sprite p = new Sprite(loadImageAfterCreation);
            Create(DataFetcher.GetSprite(id), p);

            if (ShouldCacheData)
                CachedSprites.Add(id, p);

            return p;
        }
        /// <summary>
        /// Creates an instance of a Sprite of the given Pokémon.
        /// </summary>
        /// <param name="pkmn">The Pokémon to get the Sprite from.</param>
        /// <param name="loadImageAfterCreation">Wether the image data should be loaded directly after the Sprite has been created or not.</param>
        /// <returns>The created instance of the Sprite.</returns>
        public static Sprite GetInstance(Pokemon pkmn, bool loadImageAfterCreation = false)
        {
            return GetInstance(pkmn.ID + 1, loadImageAfterCreation);
        }
    }
}
