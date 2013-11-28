using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Forms;
using LitJson;

namespace PokeAPI.NET
{
    /// <summary>
    /// The base of all PokeApi types.
    /// </summary>
    // how it's displayed in the debugger
    [DebuggerDisplay("{ID:{ID}, Name:{Name}, ResourceUri:{ResourceUri}, Created:{Created}, Modified:{Modified}")]
    public abstract class PokeApiType
    {
        protected string name;
        protected Uri uri;
        protected int id;
        protected DateTime created, modified;

        /// <summary>
        /// The name of the PokeApiType instance
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }
        /// <summary>
        /// The resource URI of the PokeApiType instance
        /// </summary>
        public Uri ResourceUri
        {
            get
            {
                return uri;
            }
        }
        /// <summary>
        /// The ID of the PokeApiType instance
        /// </summary>
        public int ID
        {
            get
            {
                return id;
            }
        }
        /// <summary>
        /// The creation date of the PokeApiType instance
        /// </summary>
        public DateTime Created
        {
            get
            {
                return created;
            }
        }
        /// <summary>
        /// The last time the PokeApiType instance was modified
        /// </summary>
        public DateTime LastModified
        {
            get
            {
                return created;
            }
        }

        /// <summary>
        /// Creates a PokeApiType from a JSON source
        /// </summary>
        /// <param name="source">The JSON source</param>
        /// <param name="ret">An object that inherits from PokeApiType</param>
        /// <returns>ret with the data from the JSON source</returns>
        public static PokeApiType Create(JsonData source, PokeApiType ret)
        {
            Create(source, ref ret);

            return ret;
        }
        /// <summary>
        /// Creates a PokeApiType from a JSON source
        /// </summary>
        /// <param name="source">The JSON source</param>
        /// <param name="ret">An object that inherits from PokeApiType</param>
        public static void Create(JsonData source, ref PokeApiType ret)
        {
            if (source.Keys.Contains("error_message"))
                throw new PokemonParseException(source["error_message"].ToString());

            if (!ret.OverrideDefaultParsing())
            {
                ret.name = source["name"].ToString().Replace('-', ' ');
                ret.uri = new Uri("http://www.pokeapi.co/" + source["resource_uri"].ToString());
                if (source.Keys.Contains("id")) // pokemon uses national_id, and the dex can't have an ID
                    ret.id = (int)source["id"];
                ret.created = ParseDateString(source["created"].ToString());
                ret.modified = ParseDateString(source["created"].ToString());
            }

            ret.Create(source);
        }

        /// <summary>
        /// Parses a date string from a JSON source to a DateTime
        /// </summary>
        /// <param name="source">The JSON source to parse</param>
        /// <returns>The parsed JSON source as a DateTime</returns>
        protected static DateTime ParseDateString(string source)
        {
            string[] dateAndTime = source.Split('T');

            // source is eg. 2013-11-02T12:08:58.787000
            return new DateTime
                (Convert.ToInt32(dateAndTime[0].Split('-')[0]), // year
                Convert.ToInt32(dateAndTime[0].Split('-')[1]), // month
                Convert.ToInt32(dateAndTime[0].Split('-')[2]), // day

                Convert.ToInt32(dateAndTime[1].Split(':')[0]), // hour (24-h)
                Convert.ToInt32(dateAndTime[1].Split(':')[1]), // minute
                Convert.ToInt32(dateAndTime[1].Split(':')[2].Split('.')[0]), // second
                Convert.ToInt32(dateAndTime[1].Split(':')[2].Split('.')[1]) / 1000); // millisecond * 1000
        }
        /// <summary>
        /// Parses a JsonData object to a NameUriPair
        /// </summary>
        /// <param name="data">The JsonData object to parse</param>
        /// <returns>The parsed JsonData object as a NameUriPair</returns>
        protected static NameUriPair ParseNameUriPair(JsonData data)
        {
            return new NameUriPair(data["name"].ToString().Replace('-', ' '), new Uri("http://pokeapi.co" + data["resource_uri"].ToString()));
        }

        protected abstract void Create(JsonData source);
        protected virtual bool OverrideDefaultParsing()
        {
            return false;
        }

        public static int ResourceUriToID(Uri resourceUri)
        {
            return Convert.ToInt32(resourceUri.AbsolutePath[resourceUri.AbsolutePath.Length - 2].ToString());
        }
        public static int ResourceUriToID(string resourceUri)
        {
            return ResourceUriToID(new Uri(resourceUri));
        }

        public override bool Equals(object obj)
        {
            return obj is PokeApiType && (PokeApiType)obj == this;
        }
        public override int GetHashCode()
        {
            return name.GetHashCode() + uri.GetHashCode() + id.GetHashCode() + created.GetHashCode() + modified.GetHashCode();
        }
        public override string ToString()
        {
            return "{" + id + ", " + name + "}";
        }
    }

    /// <summary>
    /// Represents an instance of a Pokédex
    /// </summary>
    public class Pokedex : PokeApiType
    {
        public static bool ShouldCacheData = true;
        public static Pokedex CachedPokedex = null;

        Dictionary<int, NameUriPair> pokemon = new Dictionary<int, NameUriPair>();

        /// <summary>
        /// A big list of Pokemon as NameUriPairs within this Pokedex instance
        /// </summary>
        public Dictionary<int, NameUriPair> PokemonList
        {
            get
            {
                return pokemon;
            }
        }

        /// <summary>
        /// Gets an entry of the PokemonList as a Pokemon
        /// </summary>
        /// <param name="index">The index of the entry</param>
        /// <returns>The entry of the PokemonList as a Pokemon</returns>
        public Pokemon RefPokemon(int index)
        {
            return Pokemon.GetInstance(PokemonList[id].Name);
        }

        protected override void Create(JsonData source)
        {
            foreach (JsonData data in source["pokemon"])
            {
                string[] num = data["resource_uri"].ToString().Split(new char[1] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                pokemon.Add(Convert.ToInt32(num[num.Length - 1]), ParseNameUriPair(data));
            }
        }

        /// <summary>
        /// Creates an instance of a Pokedex
        /// </summary>
        /// <returns>The created Pokedex instance</returns>
        public static Pokedex GetInstance()
        {
            if (CachedPokedex != null)
                return CachedPokedex;

            Pokedex p = new Pokedex();
            p = (Pokedex)PokeApiType.Create(DataFetcher.GetPokedex(), p);

            if (ShouldCacheData)
                CachedPokedex = p;

            return p;
        }
    }
    /// <summary>
    /// Represents an instance of a Pokémon
    /// </summary>
    public class Pokemon : PokeApiType
    {
        public static bool ShouldCacheData = true;
        public static Dictionary<int, Pokemon> CachedPokemon = new Dictionary<int, Pokemon>();

        PokemonType t = (PokemonType)(-1);

        #region public readonly static Dictionary<string, int> IDs = new Dictionary<string, int> { [...] };
        public readonly static Dictionary<string, int> IDs = new Dictionary<string, int>
        { 
            {"Bulbasaur", 1}
            , {"Ivysaur", 2}
            , {"Venusaur", 3}
            , {"Charmander", 4}
            , {"Charmeleon", 5}
            , {"Charizard", 6}
            , {"Squirtle", 7}
            , {"Wartortle", 8}
            , {"Blastoise", 9}
            , {"Caterpie", 10}
            , {"Metapod", 11}
            , {"Butterfree", 12}
            , {"Weedle", 13}
            , {"Kakuna", 14}
            , {"Beedrill", 15}
            , {"Pidgey", 16}
            , {"Pidgeotto", 17}
            , {"Pidgeot", 18}
            , {"Rattata", 19}
            , {"Raticate", 20}
            , {"Spearow", 21}
            , {"Fearow", 22}
            , {"Ekans", 23}
            , {"Arbok", 24}
            , {"Pikachu", 25}
            , {"Raichu", 26}
            , {"Sandshrew", 27}
            , {"Sandslash", 28}
            , {"Nidoran F", 29}
            , {"Nidorina", 30}
            , {"Nidoqueen", 31}
            , {"Nidoran M", 32}
            , {"Nidorino", 33}
            , {"Nidoking", 34}
            , {"Clefairy", 35}
            , {"Clefable", 36}
            , {"Vulpix", 37}
            , {"Ninetales", 38}
            , {"Jigglypuff", 39}
            , {"Wigglytuff", 40}
            , {"Zubat", 41}
            , {"Golbat", 42}
            , {"Oddish", 43}
            , {"Gloom", 44}
            , {"Vileplume", 45}
            , {"Paras", 46}
            , {"Parasect", 47}
            , {"Venonat", 48}
            , {"Venomoth", 49}
            , {"Diglett", 50}
            , {"Dugtrio", 51}
            , {"Meowth", 52}
            , {"Persian", 53}
            , {"Psyduck", 54}
            , {"Golduck", 55}
            , {"Mankey", 56}
            , {"Primeape", 57}
            , {"Growlithe", 58}
            , {"Arcanine", 59}
            , {"Poliwag", 60}
            , {"Poliwhirl", 61}
            , {"Poliwrath", 62}
            , {"Abra", 63}
            , {"Kadabra", 64}
            , {"Alakazam", 65}
            , {"Machop", 66}
            , {"Machoke", 67}
            , {"Machamp", 68}
            , {"Bellsprout", 69}
            , {"Weepinbell", 70}
            , {"Victreebel", 71}
            , {"Tentacool", 72}
            , {"Tentacruel", 73}
            , {"Geodude", 74}
            , {"Graveler", 75}
            , {"Golem", 76}
            , {"Ponyta", 77}
            , {"Rapidash", 78}
            , {"Slowpoke", 79}
            , {"Slowbro", 80}
            , {"Magnemite", 81}
            , {"Magneton", 82}
            , {"Farfetchd", 83}
            , {"Doduo", 84}
            , {"Dodrio", 85}
            , {"Seel", 86}
            , {"Dewgong", 87}
            , {"Grimer", 88}
            , {"Muk", 89}
            , {"Shellder", 90}
            , {"Cloyster", 91}
            , {"Gastly", 92}
            , {"Haunter", 93}
            , {"Gengar", 94}
            , {"Onix", 95}
            , {"Drowzee", 96}
            , {"Hypno", 97}
            , {"Krabby", 98}
            , {"Kingler", 99}
            , {"Voltorb", 100}
            , {"Electrode", 101}
            , {"Exeggcute", 102}
            , {"Exeggutor", 103}
            , {"Cubone", 104}
            , {"Marowak", 105}
            , {"Hitmonlee", 106}
            , {"Hitmonchan", 107}
            , {"Lickitung", 108}
            , {"Koffing", 109}
            , {"Weezing", 110}
            , {"Rhyhorn", 111}
            , {"Rhydon", 112}
            , {"Chansey", 113}
            , {"Tangela", 114}
            , {"Kangaskhan", 115}
            , {"Horsea", 116}
            , {"Seadra", 117}
            , {"Goldeen", 118}
            , {"Seaking", 119}
            , {"Staryu", 120}
            , {"Starmie", 121}
            , {"Mr. Mime", 122}
            , {"Scyther", 123}
            , {"Jynx", 124}
            , {"Electabuzz", 125}
            , {"Magmar", 126}
            , {"Pinsir", 127}
            , {"Tauros", 128}
            , {"Magikarp", 129}
            , {"Gyarados", 130}
            , {"Lapras", 131}
            , {"Ditto", 132}
            , {"Eevee", 133}
            , {"Vaporeon", 134}
            , {"Jolteon", 135}
            , {"Flareon", 136}
            , {"Porygon", 137}
            , {"Omanyte", 138}
            , {"Omastar", 139}
            , {"Kabuto", 140}
            , {"Kabutops", 141}
            , {"Aerodactyl", 142}
            , {"Snorlax", 143}
            , {"Articuno", 144}
            , {"Zapdos", 145}
            , {"Moltres", 146}
            , {"Dratini", 147}
            , {"Dragonair", 148}
            , {"Dragonite", 149}
            , {"Mewtwo", 150}
            , {"Mew", 151}
            , {"Chikorita", 152}
            , {"Bayleef", 153}
            , {"Meganium", 154}
            , {"Cyndaquil", 155}
            , {"Quilava", 156}
            , {"Typhlosion", 157}
            , {"Totodile", 158}
            , {"Croconaw", 159}
            , {"Feraligatr", 160}
            , {"Sentret", 161}
            , {"Furret", 162}
            , {"Hoothoot", 163}
            , {"Noctowl", 164}
            , {"Ledyba", 165}
            , {"Ledian", 166}
            , {"Spinarak", 167}
            , {"Ariados", 168}
            , {"Crobat", 169}
            , {"Chinchou", 170}
            , {"Lanturn", 171}
            , {"Pichu", 172}
            , {"Cleffa", 173}
            , {"Igglybuff", 174}
            , {"Togepi", 175}
            , {"Togetic", 176}
            , {"Natu", 177}
            , {"Xatu", 178}
            , {"Mareep", 179}
            , {"Flaaffy", 180}
            , {"Ampharos", 181}
            , {"Bellossom", 182}
            , {"Marill", 183}
            , {"Azumarill", 184}
            , {"Sudowoodo", 185}
            , {"Politoed", 186}
            , {"Hoppip", 187}
            , {"Skiploom", 188}
            , {"Jumpluff", 189}
            , {"Aipom", 190}
            , {"Sunkern", 191}
            , {"Sunflora", 192}
            , {"Yanma", 193}
            , {"Wooper", 194}
            , {"Quagsire", 195}
            , {"Espeon", 196}
            , {"Umbreon", 197}
            , {"Murkrow", 198}
            , {"Slowking", 199}
            , {"Misdreavus", 200}
            , {"Unown", 201}
            , {"Wobbuffet", 202}
            , {"Girafarig", 203}
            , {"Pineco", 204}
            , {"Forretress", 205}
            , {"Dunsparce", 206}
            , {"Gligar", 207}
            , {"Steelix", 208}
            , {"Snubbull", 209}
            , {"Granbull", 210}
            , {"Qwilfish", 211}
            , {"Scizor", 212}
            , {"Shuckle", 213}
            , {"Heracross", 214}
            , {"Sneasel", 215}
            , {"Teddiursa", 216}
            , {"Ursaring", 217}
            , {"Slugma", 218}
            , {"Magcargo", 219}
            , {"Swinub", 220}
            , {"Piloswine", 221}
            , {"Corsola", 222}
            , {"Remoraid", 223}
            , {"Octillery", 224}
            , {"Delibird", 225}
            , {"Mantine", 226}
            , {"Skarmory", 227}
            , {"Houndour", 228}
            , {"Houndoom", 229}
            , {"Kingdra", 230}
            , {"Phanpy", 231}
            , {"Donphan", 232}
            , {"Porygon2", 233}
            , {"Stantler", 234}
            , {"Smeargle", 235}
            , {"Tyrogue", 236}
            , {"Hitmontop", 237}
            , {"Smoochum", 238}
            , {"Elekid", 239}
            , {"Magby", 240}
            , {"Miltank", 241}
            , {"Blissey", 242}
            , {"Raikou", 243}
            , {"Entei", 244}
            , {"Suicune", 245}
            , {"Larvitar", 246}
            , {"Pupitar", 247}
            , {"Tyranitar", 248}
            , {"Lugia", 249}
            , {"Ho-oh", 250}
            , {"Celebi", 251}
            , {"Treecko", 252}
            , {"Grovyle", 253}
            , {"Sceptile", 254}
            , {"Torchic", 255}
            , {"Combusken", 256}
            , {"Blaziken", 257}
            , {"Mudkip", 258}
            , {"Marshtomp", 259}
            , {"Swampert", 260}
            , {"Poochyena", 261}
            , {"Mightyena", 262}
            , {"Zigzagoon", 263}
            , {"Linoone", 264}
            , {"Wurmple", 265}
            , {"Silcoon", 266}
            , {"Beautifly", 267}
            , {"Cascoon", 268}
            , {"Dustox", 269}
            , {"Lotad", 270}
            , {"Lombre", 271}
            , {"Ludicolo", 272}
            , {"Seedot", 273}
            , {"Nuzleaf", 274}
            , {"Shiftry", 275}
            , {"Taillow", 276}
            , {"Swellow", 277}
            , {"Wingull", 278}
            , {"Pelipper", 279}
            , {"Ralts", 280}
            , {"Kirlia", 281}
            , {"Gardevoir", 282}
            , {"Surskit", 283}
            , {"Masquerain", 284}
            , {"Shroomish", 285}
            , {"Breloom", 286}
            , {"Slakoth", 287}
            , {"Vigoroth", 288}
            , {"Slaking", 289}
            , {"Nincada", 290}
            , {"Ninjask", 291}
            , {"Shedinja", 292}
            , {"Whismur", 293}
            , {"Loudred", 294}
            , {"Exploud", 295}
            , {"Makuhita", 296}
            , {"Hariyama", 297}
            , {"Azurill", 298}
            , {"Nosepass", 299}
            , {"Skitty", 300}
            , {"Delcatty", 301}
            , {"Sableye", 302}
            , {"Mawile", 303}
            , {"Aron", 304}
            , {"Lairon", 305}
            , {"Aggron", 306}
            , {"Meditite", 307}
            , {"Medicham", 308}
            , {"Electrike", 309}
            , {"Manectric", 310}
            , {"Plusle", 311}
            , {"Minun", 312}
            , {"Volbeat", 313}
            , {"Illumise", 314}
            , {"Roselia", 315}
            , {"Gulpin", 316}
            , {"Swalot", 317}
            , {"Carvanha", 318}
            , {"Sharpedo", 319}
            , {"Wailmer", 320}
            , {"Wailord", 321}
            , {"Numel", 322}
            , {"Camerupt", 323}
            , {"Torkoal", 324}
            , {"Spoink", 325}
            , {"Grumpig", 326}
            , {"Spinda", 327}
            , {"Trapinch", 328}
            , {"Vibrava", 329}
            , {"Flygon", 330}
            , {"Cacnea", 331}
            , {"Cacturne", 332}
            , {"Swablu", 333}
            , {"Altaria", 334}
            , {"Zangoose", 335}
            , {"Seviper", 336}
            , {"Lunatone", 337}
            , {"Solrock", 338}
            , {"Barboach", 339}
            , {"Whiscash", 340}
            , {"Corphish", 341}
            , {"Crawdaunt", 342}
            , {"Baltoy", 343}
            , {"Claydol", 344}
            , {"Lileep", 345}
            , {"Cradily", 346}
            , {"Anorith", 347}
            , {"Armaldo", 348}
            , {"Feebas", 349}
            , {"Milotic", 350}
            , {"Castform", 351}
            , {"Kecleon", 352}
            , {"Shuppet", 353}
            , {"Banette", 354}
            , {"Duskull", 355}
            , {"Dusclops", 356}
            , {"Tropius", 357}
            , {"Chimecho", 358}
            , {"Absol", 359}
            , {"Wynaut", 360}
            , {"Snorunt", 361}
            , {"Glalie", 362}
            , {"Spheal", 363}
            , {"Sealeo", 364}
            , {"Walrein", 365}
            , {"Clamperl", 366}
            , {"Huntail", 367}
            , {"Gorebyss", 368}
            , {"Relicanth", 369}
            , {"Luvdisc", 370}
            , {"Bagon", 371}
            , {"Shelgon", 372}
            , {"Salamence", 373}
            , {"Beldum", 374}
            , {"Metang", 375}
            , {"Metagross", 376}
            , {"Regirock", 377}
            , {"Regice", 378}
            , {"Registeel", 379}
            , {"Latias", 380}
            , {"Latios", 381}
            , {"Kyogre", 382}
            , {"Groudon", 383}
            , {"Rayquaza", 384}
            , {"Jirachi", 385}
            , {"Deoxys", 386}
            , {"Turtwig", 387}
            , {"Grotle", 388}
            , {"Torterra", 389}
            , {"Chimchar", 390}
            , {"Monferno", 391}
            , {"Infernape", 392}
            , {"Piplup", 393}
            , {"Prinplup", 394}
            , {"Empoleon", 395}
            , {"Starly", 396}
            , {"Staravia", 397}
            , {"Staraptor", 398}
            , {"Bidoof", 399}
            , {"Bibarel", 400}
            , {"Kricketot", 401}
            , {"Kricketune", 402}
            , {"Shinx", 403}
            , {"Luxio", 404}
            , {"Luxray", 405}
            , {"Budew", 406}
            , {"Roserade", 407}
            , {"Cranidos", 408}
            , {"Rampardos", 409}
            , {"Shieldon", 410}
            , {"Bastiodon", 411}
            , {"Burmy", 412}
            , {"Wormadam", 413}
            , {"Mothim", 414}
            , {"Combee", 415}
            , {"Vespiquen", 416}
            , {"Pachirisu", 417}
            , {"Buizel", 418}
            , {"Floatzel", 419}
            , {"Cherubi", 420}
            , {"Cherrim", 421}
            , {"Shellos", 422}
            , {"Gastrodon", 423}
            , {"Ambipom", 424}
            , {"Drifloon", 425}
            , {"Drifblim", 426}
            , {"Buneary", 427}
            , {"Lopunny", 428}
            , {"Mismagius", 429}
            , {"Honchkrow", 430}
            , {"Glameow", 431}
            , {"Purugly", 432}
            , {"Chingling", 433}
            , {"Stunky", 434}
            , {"Skuntank", 435}
            , {"Bronzor", 436}
            , {"Bronzong", 437}
            , {"Bonsly", 438}
            , {"Mime Jr.", 439}
            , {"Happiny", 440}
            , {"Chatot", 441}
            , {"Spiritomb", 442}
            , {"Gible", 443}
            , {"Gabite", 444}
            , {"Garchomp", 445}
            , {"Munchlax", 446}
            , {"Riolu", 447}
            , {"Lucario", 448}
            , {"Hippopotas", 449}
            , {"Hippowdon", 450}
            , {"Skorupi", 451}
            , {"Drapion", 452}
            , {"Croagunk", 453}
            , {"Toxicroak", 454}
            , {"Carnivine", 455}
            , {"Finneon", 456}
            , {"Lumineon", 457}
            , {"Mantyke", 458}
            , {"Snover", 459}
            , {"Abomasnow", 460}
            , {"Weavile", 461}
            , {"Magnezone", 462}
            , {"Lickilicky", 463}
            , {"Rhyperior", 464}
            , {"Tangrowth", 465}
            , {"Electivire", 466}
            , {"Magmortar", 467}
            , {"Togekiss", 468}
            , {"Yanmega", 469}
            , {"Leafeon", 470}
            , {"Glaceon", 471}
            , {"Gliscor", 472}
            , {"Mamoswine", 473}
            , {"Porygon-Z", 474}
            , {"Gallade", 475}
            , {"Probopass", 476}
            , {"Dusknoir", 477}
            , {"Froslass", 478}
            , {"Rotom", 479}
            , {"Uxie", 480}
            , {"Mesprit", 481}
            , {"Azelf", 482}
            , {"Dialga", 483}
            , {"Palkia", 484}
            , {"Heatran", 485}
            , {"Regigigas", 486}
            , {"Giratina", 487}
            , {"Cresselia", 488}
            , {"Phione", 489}
            , {"Manaphy", 490}
            , {"Darkrai", 491}
            , {"Shaymin", 492}
            , {"Arceus", 493}
            , {"Victini", 494}
            , {"Snivy", 495}
            , {"Servine", 496}
            , {"Serperior", 497}
            , {"Tepig", 498}
            , {"Pignite", 499}
            , {"Emboar", 500}
            , {"Oshawott", 501}
            , {"Dewott", 502}
            , {"Samurott", 503}
            , {"Patrat", 504}
            , {"Watchog", 505}
            , {"Lillipup", 506}
            , {"Herdier", 507}
            , {"Stoutland", 508}
            , {"Purrloin", 509}
            , {"Liepard", 510}
            , {"Pansage", 511}
            , {"Simisage", 512}
            , {"Pansear", 513}
            , {"Simisear", 514}
            , {"Panpour", 515}
            , {"Simipour", 516}
            , {"Munna", 517}
            , {"Musharna", 518}
            , {"Pidove", 519}
            , {"Tranquill", 520}
            , {"Unfezant", 521}
            , {"Blitzle", 522}
            , {"Zebstrika", 523}
            , {"Roggenrola", 524}
            , {"Boldore", 525}
            , {"Gigalith", 526}
            , {"Woobat", 527}
            , {"Swoobat", 528}
            , {"Drilbur", 529}
            , {"Excadrill", 530}
            , {"Audino", 531}
            , {"Timburr", 532}
            , {"Gurdurr", 533}
            , {"Conkeldurr", 534}
            , {"Tympole", 535}
            , {"Palpitoad", 536}
            , {"Seismitoad", 537}
            , {"Throh", 538}
            , {"Sawk", 539}
            , {"Sewaddle", 540}
            , {"Swadloon", 541}
            , {"Leavanny", 542}
            , {"Venipede", 543}
            , {"Whirlipede", 544}
            , {"Scolipede", 545}
            , {"Cottonee", 546}
            , {"Whimsicott", 547}
            , {"Petilil", 548}
            , {"Lilligant", 549}
            , {"Basculin", 550}
            , {"Sandile", 551}
            , {"Krokorok", 552}
            , {"Krookodile", 553}
            , {"Darumaka", 554}
            , {"Darmanitan", 555}
            , {"Maractus", 556}
            , {"Dwebble", 557}
            , {"Crustle", 558}
            , {"Scraggy", 559}
            , {"Scrafty", 560}
            , {"Sigilyph", 561}
            , {"Yamask", 562}
            , {"Cofagrigus", 563}
            , {"Tirtouga", 564}
            , {"Carracosta", 565}
            , {"Archen", 566}
            , {"Archeops", 567}
            , {"Trubbish", 568}
            , {"Garbodor", 569}
            , {"Zorua", 570}
            , {"Zoroark", 571}
            , {"Minccino", 572}
            , {"Cinccino", 573}
            , {"Gothita", 574}
            , {"Gothorita", 575}
            , {"Gothitelle", 576}
            , {"Solosis", 577}
            , {"Duosion", 578}
            , {"Reuniclus", 579}
            , {"Ducklett", 580}
            , {"Swanna", 581}
            , {"Vanillite", 582}
            , {"Vanillish", 583}
            , {"Vanilluxe", 584}
            , {"Deerling", 585}
            , {"Sawsbuck", 586}
            , {"Emolga", 587}
            , {"Karrablast", 588}
            , {"Escavalier", 589}
            , {"Foongus", 590}
            , {"Amoonguss", 591}
            , {"Frillish", 592}
            , {"Jellicent", 593}
            , {"Alomomola", 594}
            , {"Joltik", 595}
            , {"Galvantula", 596}
            , {"Ferroseed", 597}
            , {"Ferrothorn", 598}
            , {"Klink", 599}
            , {"Klang", 600}
            , {"Klinklang", 601}
            , {"Tynamo", 602}
            , {"Eelektrik", 603}
            , {"Eelektross", 604}
            , {"Elgyem", 605}
            , {"Beheeyem", 606}
            , {"Litwick", 607}
            , {"Lampent", 608}
            , {"Chandelure", 609}
            , {"Axew", 610}
            , {"Fraxure", 611}
            , {"Haxorus", 612}
            , {"Cubchoo", 613}
            , {"Beartic", 614}
            , {"Cryogonal", 615}
            , {"Shelmet", 616}
            , {"Accelgor", 617}
            , {"Stunfisk", 618}
            , {"Mienfoo", 619}
            , {"Mienshao", 620}
            , {"Druddigon", 621}
            , {"Golett", 622}
            , {"Golurk", 623}
            , {"Pawniard", 624}
            , {"Bisharp", 625}
            , {"Bouffalant", 626}
            , {"Rufflet", 627}
            , {"Braviary", 628}
            , {"Vullaby", 629}
            , {"Mandibuzz", 630}
            , {"Heatmor", 631}
            , {"Durant", 632}
            , {"Deino", 633}
            , {"Zweilous", 634}
            , {"Hydreigon", 635}
            , {"Larvesta", 636}
            , {"Volcarona", 637}
            , {"Cobalion", 638}
            , {"Terrakion", 639}
            , {"Virizion", 640}
            , {"Tornadus", 641}
            , {"Thundurus", 642}
            , {"Reshiram", 643}
            , {"Zekrom", 644}
            , {"Landorus", 645}
            , {"Kyurem", 646}
            , {"Keldeo", 647}
            , {"Meloetta", 648}
            , {"Genesect", 649}
            , {"Chespin", 650}
            , {"Quilladin", 651}
            , {"Chesnaught", 652}
            , {"Fennekin", 653}
            , {"Braixen", 654}
            , {"Delphox", 655}
            , {"Froakie", 656}
            , {"Frogadier", 657}
            , {"Greninja", 658}
            , {"Bunnelby", 659}
            , {"Diggersby", 660}
            , {"Fletchling", 661}
            , {"Fletchinder", 662}
            , {"Talonflame", 663}
            , {"Scatterbug", 664}
            , {"Spewpa", 665}
            , {"Vivillon", 666}
            , {"Litleo", 667}
            , {"Pyroar", 668}
            , {"Flabebe", 669}
            , {"Floette", 670}
            , {"Florges", 671}
            , {"Skiddo", 672}
            , {"Gogoat", 673}
            , {"Pancham", 674}
            , {"Pangoro", 675}
            , {"Furfrou", 676}
            , {"Espurr", 677}
            , {"Meowstic", 678}
            , {"Honedge", 679}
            , {"Doublade", 680}
            , {"Aegislash", 681}
            , {"Spritzee", 682}
            , {"Aromatisse", 683}
            , {"Swirlix", 684}
            , {"Slurpuff", 685}
            , {"Inkay", 686}
            , {"Malamar", 687}
            , {"Binacle", 688}
            , {"Barbaracle", 689}
            , {"Skrelp", 690}
            , {"Dragalge", 691}
            , {"Clauncher", 692}
            , {"Clawitzer", 693}
            , {"Helioptile", 694}
            , {"Heliolisk", 695}
            , {"Tyrunt", 696}
            , {"Tyrantrum", 697}
            , {"Amaura", 698}
            , {"Aurorus", 699}
            , {"Sylveon", 700}
            , {"Hawlucha", 701}
            , {"Dedenne", 702}
            , {"Carbink", 703}
            , {"Goomy", 704}
            , {"Sliggoo", 705}
            , {"Goodra", 706}
            , {"Klefki", 707}
            , {"Phantump", 708}
            , {"Trevenant", 709}
            , {"Pumpkaboo", 710}
            , {"Gourgeist", 711}
            , {"Bergmite", 712}
            , {"Avalugg", 713}
            , {"Noibat", 714}
            , {"Noivern", 715}
            , {"Xerneas", 716}
            , {"Yveltal", 717}
        };
        #endregion

        #region Fields
        List<NameUriPair>
            abilities = new List<NameUriPair>(),
            eggGroups = new List<NameUriPair>(),
            //evolutions = new List<NameUriPair>(),
            //moves = new List<NameUriPair>(),
            types = new List<NameUriPair>();
        List<PokeEvolution> evolutions = new List<PokeEvolution>();
        List<Tuple<string, NameUriPair>> moves = new List<Tuple<string, NameUriPair>>(); // learn type - name & resource uri
        string species, growthRate;
        int hp, attack, defense, catchRate, spAttack, spDefense, speed,
            eggCycles, evYield, xpYield, height, weight, happiness;
        Tuple<double, double> mfRatio;

        /// <summary>
        /// The abilities this Pokemon instance can have
        /// </summary>
        public List<NameUriPair> Abilities
        {
            get
            {
                return abilities;
            }
        }
        /// <summary>
        /// The egg groups this Pokemon instance is in
        /// </summary>
        public List<NameUriPair> EggGroups
        {
            get
            {
                return eggGroups;
            }
        }
        /// <summary>
        /// The evolutions this Pokemon instance can evolve into
        /// </summary>
        public List<PokeEvolution> Evolutions
        {
            get
            {
                return evolutions;
            }
        }
        /// <summary>
        /// The moves this Pokemon instance can learn
        /// </summary>
        public List<Tuple<string, NameUriPair>> Moves
        {
            get
            {
                return moves;
            }
        }
        /// <summary>
        /// The types this Pokemon instance is
        /// </summary>
        public List<NameUriPair> Types
        {
            get
            {
                return types;
            }
        }

        /// <summary>
        /// Gets an entry of the Abilities list as a PokeAbility
        /// </summary>
        /// <param name="index">The index of the entry</param>
        /// <returns>The entry of the Abilities list as a PokeAbility</returns>
        public PokeAbility RefAbility(int index)
        {
            return PokeAbility.GetInstance(Abilities[index].Name);
        }
        /// <summary>
        /// Gets an entry of the EggGroups list as a PokeEggGroup
        /// </summary>
        /// <param name="index">The index of the entry</param>
        /// <returns>The entry of the EggGroups list as a PokeEggGroup</returns>
        public PokeEggGroup RefEggGroup(int index)
        {
            return PokeEggGroup.GetInstance(EggGroups[index].Name);
        }
        /// <summary>
        /// Gets an entry of the Abilities list as a PokeAbility
        /// </summary>
        /// <param name="index">The index of the entry</param>
        /// <returns>The entry of the Abilities list as a PokeAbility</returns>
        public PokeMove RefMove(int index)
        {
            return PokeMove.GetInstance(Moves[index].Item2.Name);
        }
        /// <summary>
        /// Gets an entry of the Abilities list as a PokeAbility
        /// </summary>
        /// <param name="index">The index of the entry</param>
        /// <returns>The entry of the Abilities list as a PokeAbility</returns>
        public PokeType RefType(int index)
        {
            return PokeType.GetInstance(Types[index].Name);
        }

        /// <summary>
        /// The species of this Pokemon instance
        /// </summary>
        public string Species
        {
            get
            {
                return species;
            }
        }
        /// <summary>
        /// The growth rate of this Pokemon instance
        /// </summary>
        public string GrowthRate
        {
            get
            {
                return growthRate;
            }
        }

        /// <summary>
        /// The hit points of this Pokemon instance
        /// </summary>
        public int HP
        {
            get
            {
                return hp;
            }
        }
        /// <summary>
        /// The base attack of this Pokemon instance
        /// </summary>
        public int Attack
        {
            get
            {
                return attack;
            }
        }
        /// <summary>
        /// The base defense of this Pokemon instance
        /// </summary>
        public int Defense
        {
            get
            {
                return defense;
            }
        }
        /// <summary>
        /// This Pokemon instance's catch rate
        /// </summary>
        public int CatchRate
        {
            get
            {
                return catchRate;
            }
        }
        /// <summary>
        /// The base special attack of this Pokemon instance
        /// </summary>
        public int SpecialAttack
        {
            get
            {
                return spAttack;
            }
        }
        /// <summary>
        /// The base special defense of this Pokemon instance
        /// </summary>
        public int SpecialDefense
        {
            get
            {
                return spDefense;
            }
        }
        /// <summary>
        /// The base speed of this Pokemon instance
        /// </summary>
        public int Speed
        {
            get
            {
                return speed;
            }
        }

        /// <summary>
        /// The number of egg cycles needed
        /// </summary>
        public int EggCycles
        {
            get
            {
                return eggCycles;
            }
        }
        /// <summary>
        /// The base effort value yield for this Pokemon instance
        /// </summary>
        public int EvYield
        {
            get
            {
                return evYield;
            }
        }
        /// <summary>
        /// The base experience yield for this Pokemon instance
        /// </summary>
        public int ExpYield
        {
            get
            {
                return xpYield;
            }
        }
        /// <summary>
        /// The height of this Pokemon instance
        /// </summary>
        public int Height
        {
            get
            {
                return height;
            }
        }
        /// <summary>
        /// The weight of this Pokemon instance
        /// </summary>
        public int Weight
        {
            get
            {
                return weight;
            }
        }
        /// <summary>
        /// The base happiness of this Pokemon instance
        /// </summary>
        public int BaseHappiness
        {
            get
            {
                return happiness;
            }
        }

        /// <summary>
        /// The types this Pokemon instance is as a flags field
        /// </summary>
        public PokemonType Type
        {
            get
            {
                if ((int)t == -1)
                {
                    PokemonType ret = 0;

                    foreach (NameUriPair pair in types)
                        ret |= PokeType.GetInstance(pair.Name);

                    return t = ret;
                }

                return t;
            }
        }
        #endregion

        protected override void Create(JsonData source)
        {
            id = (int)source["national_id"];

            foreach (JsonData data in source["abilities"])
                abilities.Add(ParseNameUriPair(data));
            foreach (JsonData data in source["egg_groups"])
                eggGroups.Add(ParseNameUriPair(data));
            //foreach (JsonData data in source["evolutions"])
            //    evolutions.Add(ParseNameUriPair(data));
            if (source.Keys.Contains("evolutions"))
            {
                PokeEvolution p = new PokeEvolution();
                if (source.Count == 1)
                {
                    //p.parent = source;
                    p = (PokeEvolution)PokeApiType.Create(source["evolutions"], p);
                    evolutions.Add(p);
                }
                else
                    foreach (JsonData data in source["evolutions"])
                    {
                        p = new PokeEvolution();
                        //p.parent = source;
                        p = (PokeEvolution)PokeApiType.Create(data, p);
                        evolutions.Add(p);
                    }
            }

            foreach (JsonData data in source["moves"])
                moves.Add(new Tuple<string, NameUriPair>(data["learn_type"].ToString(), ParseNameUriPair(data)));
            foreach (JsonData data in source["types"])
                types.Add(ParseNameUriPair(data));

            catchRate = (int)source["catch_rate"];
            species = source["species"].ToString();
            hp = (int)source["hp"];
            attack = (int)source["attack"];
            defense = (int)source["defense"];
            spAttack = (int)source["sp_atk"];
            spDefense = (int)source["sp_def"];
            speed = (int)source["speed"];
            eggCycles = (int)source["egg_cycles"];
            evYield = (int)source["ev_yield"];
            xpYield = (int)source["exp"];
            growthRate = source["growth_rate"].ToString();
            height = (int)source["height"];
            weight = (int)source["weight"];
            happiness = (int)source["happiness"];

            mfRatio = new Tuple<double, double>
            (
                Convert.ToDouble(source["male_female_ratio"].ToString().Split('/')[0], CultureInfo.InvariantCulture) / 100d,
                Convert.ToDouble(source["male_female_ratio"].ToString().Split('/')[1], CultureInfo.InvariantCulture) / 100d
            );
        }

        /// <summary>
        /// Creates an instance of a Pokemon with the given name
        /// </summary>
        /// <param name="name">The name of the Pokemon to instantiate</param>
        /// <returns>The created instance of the Pokemon</returns>
        public static Pokemon GetInstance(string name)
        {
            return GetInstance(IDs[name]);
        }
        /// <summary>
        /// Creates an instance of a Pokemon with the given ID
        /// </summary>
        /// <param name="id">The ID of the Pokemon to instantiate</param>
        /// <returns>The created instance of the Pokemon</returns>
        public static Pokemon GetInstance(int id)
        {
            if (CachedPokemon.ContainsKey(id))
                return CachedPokemon[id];

            Pokemon p = new Pokemon();
            p = (Pokemon)PokeApiType.Create(DataFetcher.GetPokemon(id), p);

            if (ShouldCacheData)
                CachedPokemon.Add(id, p);

            return p;
        }
    }
    /// <summary>
    /// Represents an instance of a Pokémon Type
    /// </summary>
    public class PokeType : PokeApiType
    {
        public static bool ShouldCacheData = true;
        public static Dictionary<int, PokeType> CachedTypes = new Dictionary<int, PokeType>();

        #region public readonly static Dictionary<string, int> IDs = new Dictionary<string, int>() { [...] };
        public readonly static Dictionary<string, int> IDs = new Dictionary<string, int>()
        { 
            {"Normal", 1},
            {"Fighting", 2},
            {"Flying", 3},
            {"Poison", 4},
            {"Ground", 5},
            {"Rock", 6},
            {"Bug", 7},
            {"Ghost", 8},
            {"Steel", 9},
            {"Fire", 10},
            {"Water", 11},
            {"Grass", 12},
            {"Electric", 13},
            {"Pyschic", 14},
            {"Ice", 15},
            {"Dragon", 16},
            {"Dark", 17},
            {"Fairy", 18},
        };
        #endregion

        List<NameUriPair>
            ineffective = new List<NameUriPair>(),
            noEffect = new List<NameUriPair>(),
            resistance = new List<NameUriPair>(),
            superEffective = new List<NameUriPair>(),
            weakness = new List<NameUriPair>();

        /// <summary>
        /// The types this PokeType instance is ineffective against
        /// </summary>
        public List<NameUriPair> Ineffective
        {
            get
            {
                return ineffective;
            }
        }
        /// <summary>
        /// The types this PokeType instance has no effect against
        /// </summary>
        public List<NameUriPair> NoEffect
        {
            get
            {
                return noEffect;
            }
        }
        /// <summary>
        /// The types this PokeType instance is resistant to
        /// </summary>
        public List<NameUriPair> Resistance
        {
            get
            {
                return resistance;
            }
        }
        /// <summary>
        /// The types this PokeType instance is super effective against
        /// </summary>
        public List<NameUriPair> SuperEffective
        {
            get
            {
                return superEffective;
            }
        }
        /// <summary>
        /// The types this PokeType instance is weak to
        /// </summary>
        public List<NameUriPair> Weakness
        {
            get
            {
                return weakness;
            }
        }

        /// <summary>
        /// Gets an entry of the Ineffective list as a PokeType
        /// </summary>
        /// <param name="index">The index of the entry</param>
        /// <returns>The entry of the Ineffective list as a PokeType</returns>
        public PokeType RefIneffective(int index)
        {
            return GetInstance(Ineffective[index].Name);
        }
        /// <summary>
        /// Gets an entry of the NoEffect list as a PokeType
        /// </summary>
        /// <param name="index">The index of the entry</param>
        /// <returns>The entry of the NoEffect list as a PokeType</returns>
        public PokeType RefNoEffect(int index)
        {
            return GetInstance(NoEffect[index].Name);
        }
        /// <summary>
        /// Gets an entry of the Resistance list as a PokeType
        /// </summary>
        /// <param name="index">The index of the entry</param>
        /// <returns>The entry of the Resistance list as a PokeType</returns>
        public PokeType RefResistance(int index)
        {
            return GetInstance(Resistance[index].Name);
        }
        /// <summary>
        /// Gets an entry of the SuperEffective list as a PokeType
        /// </summary>
        /// <param name="index">The index of the entry</param>
        /// <returns>The entry of the SuperEffective list as a PokeType</returns>
        public PokeType RefSuperEffective(int index)
        {
            return GetInstance(SuperEffective[index].Name);
        }
        /// <summary>
        /// Gets an entry of the Weakness list as a PokeType
        /// </summary>
        /// <param name="index">The index of the entry</param>
        /// <returns>The entry of the Weakness list as a PokeType</returns>
        public PokeType RefWeakness(int index)
        {
            return GetInstance(Weakness[index].Name);
        }

        protected override void Create(JsonData source)
        {
            foreach (JsonData data in source["ineffective"])
                ineffective.Add(ParseNameUriPair(data));
            foreach (JsonData data in source["no_effect"])
                noEffect.Add(ParseNameUriPair(data));
            foreach (JsonData data in source["resistance"])
                resistance.Add(ParseNameUriPair(data));
            foreach (JsonData data in source["super_effective"])
                superEffective.Add(ParseNameUriPair(data));
            foreach (JsonData data in source["weakness"])
                weakness.Add(ParseNameUriPair(data));

            if (ShouldCacheData && !CachedTypes.ContainsKey(id))
                CachedTypes.Add(id, this);
        }

        /// <summary>
        /// Creates an instance of a PokeType with the given name
        /// </summary>
        /// <param name="name">The name of the PokeType to instantiate</param>
        /// <returns>The created instance of the PokeType</returns>
        public static PokeType GetInstance(string name)
        {
            return GetInstance(IDs[name]);
        }
        /// <summary>
        /// Creates an instance of a PokeType with the given PokemonType
        /// </summary>
        /// <param name="type">The type of the PokeType to instantiate</param>
        /// <returns>The created instance of the PokeType</returns>
        public static PokeType GetInstance(PokemonType type)
        {
            return GetInstance(type.ID());
        }
        /// <summary>
        /// Creates an instance of a PokeType with the given ID
        /// </summary>
        /// <param name="id">The id of the PokeType to instantiate</param>
        /// <returns>The created instance of the PokeType</returns>
        public static PokeType GetInstance(int id)
        {
            if (CachedTypes.ContainsKey(id))
                return CachedTypes[id];

            PokeType p = new PokeType();
            p = (PokeType)PokeApiType.Create(DataFetcher.GetType(id), p);

            if (ShouldCacheData)
                CachedTypes.Add(id, p);

            return p;
        }

        public static implicit operator PokemonType(PokeType type)
        {
            // lazy<me>
            PokemonType ret = 0;
            Enum.TryParse<PokemonType>(type.name.Replace(' ', '_'), false, out ret);
            return ret;
        }
        
    }
    /// <summary>
    /// Represents an instance of a Pokémon Move
    /// </summary>
    public class PokeMove : PokeApiType
    {
        public static bool ShouldCacheData = true;
        public static Dictionary<int, PokeMove> CachedMoves = new Dictionary<int, PokeMove>();

        #region public readonly static Dictionary<string, int> IDs = new Dictionary<string, int> { [...] };
        public readonly static Dictionary<string, int> IDs = new Dictionary<string, int>
        {
            {"Pound", 1},
            {"Karate chop", 2},
            {"Doubleslap", 3},
            {"Comet punch", 4},
            {"Mega punch", 5},
            {"Pay day", 6},
            {"Fire punch", 7},
            {"Ice punch", 8},
            {"Thunderpunch", 9},
            {"Scratch", 10},
            {"Vicegrip", 11},
            {"Guillotine", 12},
            {"Razor wind", 13},
            {"Swords dance", 14},
            {"Cut", 15},
            {"Gust", 16},
            {"Wing attack", 17},
            {"Whirlwind", 18},
            {"Fly", 19},
            {"Bind", 20},
            {"Slam", 21},
            {"Vine whip", 22},
            {"Stomp", 23},
            {"Double kick", 24},
            {"Mega kick", 25},
            {"Jump kick", 26},
            {"Rolling kick", 27},
            {"Sand attack", 28},
            {"Headbutt", 29},
            {"Horn attack", 30},
            {"Fury attack", 31},
            {"Horn drill", 32},
            {"Tackle", 33},
            {"Body slam", 34},
            {"Wrap", 35},
            {"Take down", 36},
            {"Thrash", 37},
            {"Double edge", 38},
            {"Tail whip", 39},
            {"Poison sting", 40},
            {"Twineedle", 41},
            {"Pin missile", 42},
            {"Leer", 43},
            {"Bite", 44},
            {"Growl", 45},
            {"Roar", 46},
            {"Sing", 47},
            {"Supersonic", 48},
            {"Sonicboom", 49},
            {"Disable", 50},
            {"Acid", 51},
            {"Ember", 52},
            {"Flamethrower", 53},
            {"Mist", 54},
            {"Water gun", 55},
            {"Hydro pump", 56},
            {"Surf", 57},
            {"Ice beam", 58},
            {"Blizzard", 59},
            {"Psybeam", 60},
            {"Bubblebeam", 61},
            {"Aurora beam", 62},
            {"Hyper beam", 63},
            {"Peck", 64},
            {"Drill peck", 65},
            {"Submission", 66},
            {"Low kick", 67},
            {"Counter", 68},
            {"Seismic toss", 69},
            {"Strength", 70},
            {"Absorb", 71},
            {"Mega drain", 72},
            {"Leech seed", 73},
            {"Growth", 74},
            {"Razor leaf", 75},
            {"Solarbeam", 76},
            {"Poisonpowder", 77},
            {"Stun spore", 78},
            {"Sleep powder", 79},
            {"Petal dance", 80},
            {"String shot", 81},
            {"Dragon rage", 82},
            {"Fire spin", 83},
            {"Thundershock", 84},
            {"Thunderbolt", 85},
            {"Thunder wave", 86},
            {"Thunder", 87},
            {"Rock throw", 88},
            {"Earthquake", 89},
            {"Fissure", 90},
            {"Dig", 91},
            {"Toxic", 92},
            {"Confusion", 93},
            {"Psychic", 94},
            {"Hypnosis", 95},
            {"Meditate", 96},
            {"Agility", 97},
            {"Quick attack", 98},
            {"Rage", 99},
            {"Teleport", 100},
            {"Night shade", 101},
            {"Mimic", 102},
            {"Screech", 103},
            {"Double team", 104},
            {"Recover", 105},
            {"Harden", 106},
            {"Minimize", 107},
            {"Smokescreen", 108},
            {"Confuse ray", 109},
            {"Withdraw", 110},
            {"Defense curl", 111},
            {"Barrier", 112},
            {"Light screen", 113},
            {"Haze", 114},
            {"Reflect", 115},
            {"Focus energy", 116},
            {"Bide", 117},
            {"Metronome", 118},
            {"Mirror move", 119},
            {"Selfdestruct", 120},
            {"Egg bomb", 121},
            {"Lick", 122},
            {"Smog", 123},
            {"Sludge", 124},
            {"Bone club", 125},
            {"Fire blast", 126},
            {"Waterfall", 127},
            {"Clamp", 128},
            {"Swift", 129},
            {"Skull bash", 130},
            {"Spike cannon", 131},
            {"Constrict", 132},
            {"Amnesia", 133},
            {"Kinesis", 134},
            {"Softboiled", 135},
            {"Hi jump kick", 136},
            {"Glare", 137},
            {"Dream eater", 138},
            {"Poison gas", 139},
            {"Barrage", 140},
            {"Leech life", 141},
            {"Lovely kiss", 142},
            {"Sky attack", 143},
            {"Transform", 144},
            {"Bubble", 145},
            {"Dizzy punch", 146},
            {"Spore", 147},
            {"Flash", 148},
            {"Psywave", 149},
            {"Splash", 150},
            {"Acid armor", 151},
            {"Crabhammer", 152},
            {"Explosion", 153},
            {"Fury swipes", 154},
            {"Bonemerang", 155},
            {"Rest", 156},
            {"Rock slide", 157},
            {"Hyper fang", 158},
            {"Sharpen", 159},
            {"Conversion", 160},
            {"Tri attack", 161},
            {"Super fang", 162},
            {"Slash", 163},
            {"Substitute", 164},
            {"Struggle", 165},
            {"Sketch", 166},
            {"Triple kick", 167},
            {"Thief", 168},
            {"Spider web", 169},
            {"Mind reader", 170},
            {"Nightmare", 171},
            {"Flame wheel", 172},
            {"Snore", 173},
            {"Curse", 174},
            {"Flail", 175},
            {"Conversion 2", 176},
            {"Aeroblast", 177},
            {"Cotton spore", 178},
            {"Reversal", 179},
            {"Spite", 180},
            {"Powder snow", 181},
            {"Protect", 182},
            {"Mach punch", 183},
            {"Scary face", 184},
            {"Faint attack", 185},
            {"Sweet kiss", 186},
            {"Belly drum", 187},
            {"Sludge bomb", 188},
            {"Mud slap", 189},
            {"Octazooka", 190},
            {"Spikes", 191},
            {"Zap cannon", 192},
            {"Foresight", 193},
            {"Destiny bond", 194},
            {"Perish song", 195},
            {"Icy wind", 196},
            {"Detect", 197},
            {"Bone rush", 198},
            {"Lock on", 199},
            {"Outrage", 200},
            {"Sandstorm", 201},
            {"Giga drain", 202},
            {"Endure", 203},
            {"Charm", 204},
            {"Rollout", 205},
            {"False swipe", 206},
            {"Swagger", 207},
            {"Milk drink", 208},
            {"Spark", 209},
            {"Fury cutter", 210},
            {"Steel wing", 211},
            {"Mean look", 212},
            {"Attract", 213},
            {"Sleep talk", 214},
            {"Heal bell", 215},
            {"Return", 216},
            {"Present", 217},
            {"Frustration", 218},
            {"Safeguard", 219},
            {"Pain split", 220},
            {"Sacred fire", 221},
            {"Magnitude", 222},
            {"Dynamicpunch", 223},
            {"Megahorn", 224},
            {"Dragonbreath", 225},
            {"Baton pass", 226},
            {"Encore", 227},
            {"Pursuit", 228},
            {"Rapid spin", 229},
            {"Sweet scent", 230},
            {"Iron tail", 231},
            {"Metal claw", 232},
            {"Vital throw", 233},
            {"Morning sun", 234},
            {"Synthesis", 235},
            {"Moonlight", 236},
            {"Hidden power", 237},
            {"Cross chop", 238},
            {"Twister", 239},
            {"Rain dance", 240},
            {"Sunny day", 241},
            {"Crunch", 242},
            {"Mirror coat", 243},
            {"Psych up", 244},
            {"Extremespeed", 245},
            {"Ancientpower", 246},
            {"Shadow ball", 247},
            {"Future sight", 248},
            {"Rock smash", 249},
            {"Whirlpool", 250},
            {"Beat up", 251},
            {"Fake out", 252},
            {"Uproar", 253},
            {"Stockpile", 254},
            {"Spit up", 255},
            {"Swallow", 256},
            {"Heat wave", 257},
            {"Hail", 258},
            {"Torment", 259},
            {"Flatter", 260},
            {"Will o wisp", 261},
            {"Memento", 262},
            {"Facade", 263},
            {"Focus punch", 264},
            {"Smellingsalt", 265},
            {"Follow me", 266},
            {"Nature power", 267},
            {"Charge", 268},
            {"Taunt", 269},
            {"Helping hand", 270},
            {"Trick", 271},
            {"Role play", 272},
            {"Wish", 273},
            {"Assist", 274},
            {"Ingrain", 275},
            {"Superpower", 276},
            {"Magic coat", 277},
            {"Recycle", 278},
            {"Revenge", 279},
            {"Brick break", 280},
            {"Yawn", 281},
            {"Knock off", 282},
            {"Endeavor", 283},
            {"Eruption", 284},
            {"Skill swap", 285},
            {"Imprison", 286},
            {"Refresh", 287},
            {"Grudge", 288},
            {"Snatch", 289},
            {"Secret power", 290},
            {"Dive", 291},
            {"Arm thrust", 292},
            {"Camouflage", 293},
            {"Tail glow", 294},
            {"Luster purge", 295},
            {"Mist ball", 296},
            {"Featherdance", 297},
            {"Teeter dance", 298},
            {"Blaze kick", 299},
            {"Mud sport", 300},
            {"Ice ball", 301},
            {"Needle arm", 302},
            {"Slack off", 303},
            {"Hyper voice", 304},
            {"Poison fang", 305},
            {"Crush claw", 306},
            {"Blast burn", 307},
            {"Hydro cannon", 308},
            {"Meteor mash", 309},
            {"Astonish", 310},
            {"Weather ball", 311},
            {"Aromatherapy", 312},
            {"Fake tears", 313},
            {"Air cutter", 314},
            {"Overheat", 315},
            {"Odor sleuth", 316},
            {"Rock tomb", 317},
            {"Silver wind", 318},
            {"Metal sound", 319},
            {"Grasswhistle", 320},
            {"Tickle", 321},
            {"Cosmic power", 322},
            {"Water spout", 323},
            {"Signal beam", 324},
            {"Shadow punch", 325},
            {"Extrasensory", 326},
            {"Sky uppercut", 327},
            {"Sand tomb", 328},
            {"Sheer cold", 329},
            {"Muddy water", 330},
            {"Bullet seed", 331},
            {"Aerial ace", 332},
            {"Icicle spear", 333},
            {"Iron defense", 334},
            {"Block", 335},
            {"Howl", 336},
            {"Dragon claw", 337},
            {"Frenzy plant", 338},
            {"Bulk up", 339},
            {"Bounce", 340},
            {"Mud shot", 341},
            {"Poison tail", 342},
            {"Covet", 343},
            {"Volt tackle", 344},
            {"Magical leaf", 345},
            {"Water sport", 346},
            {"Calm mind", 347},
            {"Leaf blade", 348},
            {"Dragon dance", 349},
            {"Rock blast", 350},
            {"Shock wave", 351},
            {"Water pulse", 352},
            {"Doom desire", 353},
            {"Psycho boost", 354},
            {"Roost", 355},
            {"Gravity", 356},
            {"Miracle eye", 357},
            {"Wake up slap", 358},
            {"Hammer arm", 359},
            {"Gyro ball", 360},
            {"Healing wish", 361},
            {"Brine", 362},
            {"Natural gift", 363},
            {"Feint", 364},
            {"Pluck", 365},
            {"Tailwind", 366},
            {"Acupressure", 367},
            {"Metal burst", 368},
            {"U turn", 369},
            {"Close combat", 370},
            {"Payback", 371},
            {"Assurance", 372},
            {"Embargo", 373},
            {"Fling", 374},
            {"Psycho shift", 375},
            {"Trump card", 376},
            {"Heal block", 377},
            {"Wring out", 378},
            {"Power trick", 379},
            {"Gastro acid", 380},
            {"Lucky chant", 381},
            {"Me first", 382},
            {"Copycat", 383},
            {"Power swap", 384},
            {"Guard swap", 385},
            {"Punishment", 386},
            {"Last resort", 387},
            {"Worry seed", 388},
            {"Sucker punch", 389},
            {"Toxic spikes", 390},
            {"Heart swap", 391},
            {"Aqua ring", 392},
            {"Magnet rise", 393},
            {"Flare blitz", 394},
            {"Force palm", 395},
            {"Aura sphere", 396},
            {"Rock polish", 397},
            {"Poison jab", 398},
            {"Dark pulse", 399},
            {"Night slash", 400},
            {"Aqua tail", 401},
            {"Seed bomb", 402},
            {"Air slash", 403},
            {"X scissor", 404},
            {"Bug buzz", 405},
            {"Dragon pulse", 406},
            {"Dragon rush", 407},
            {"Power gem", 408},
            {"Drain punch", 409},
            {"Vacuum wave", 410},
            {"Focus blast", 411},
            {"Energy ball", 412},
            {"Brave bird", 413},
            {"Earth power", 414},
            {"Switcheroo", 415},
            {"Giga impact", 416},
            {"Nasty plot", 417},
            {"Bullet punch", 418},
            {"Avalanche", 419},
            {"Ice shard", 420},
            {"Shadow claw", 421},
            {"Thunder fang", 422},
            {"Ice fang", 423},
            {"Fire fang", 424},
            {"Shadow sneak", 425},
            {"Mud bomb", 426},
            {"Psycho cut", 427},
            {"Zen headbutt", 428},
            {"Mirror shot", 429},
            {"Flash cannon", 430},
            {"Rock climb", 431},
            {"Defog", 432},
            {"Trick room", 433},
            {"Draco meteor", 434},
            {"Discharge", 435},
            {"Lava plume", 436},
            {"Leaf storm", 437},
            {"Power whip", 438},
            {"Rock wrecker", 439},
            {"Cross poison", 440},
            {"Gunk shot", 441},
            {"Iron head", 442},
            {"Magnet bomb", 443},
            {"Stone edge", 444},
            {"Captivate", 445},
            {"Stealth rock", 446},
            {"Grass knot", 447},
            {"Chatter", 448},
            {"Judgment", 449},
            {"Bug bite", 450},
            {"Charge beam", 451},
            {"Wood hammer", 452},
            {"Aqua jet", 453},
            {"Attack order", 454},
            {"Defend order", 455},
            {"Heal order", 456},
            {"Head smash", 457},
            {"Double hit", 458},
            {"Roar of time", 459},
            {"Spacial rend", 460},
            {"Lunar dance", 461},
            {"Crush grip", 462},
            {"Magma storm", 463},
            {"Dark void", 464},
            {"Seed flare", 465},
            {"Ominous wind", 466},
            {"Shadow force", 467},
            {"Hone claws", 468},
            {"Wide guard", 469},
            {"Guard split", 470},
            {"Power split", 471},
            {"Wonder room", 472},
            {"Psyshock", 473},
            {"Venoshock", 474},
            {"Autotomize", 475},
            {"Rage powder", 476},
            {"Telekinesis", 477},
            {"Magic room", 478},
            {"Smack down", 479},
            {"Storm throw", 480},
            {"Flame burst", 481},
            {"Sludge wave", 482},
            {"Quiver dance", 483},
            {"Heavy slam", 484},
            {"Synchronoise", 485},
            {"Electro ball", 486},
            {"Soak", 487},
            {"Flame charge", 488},
            {"Coil", 489},
            {"Low sweep", 490},
            {"Acid spray", 491},
            {"Foul play", 492},
            {"Simple beam", 493},
            {"Entrainment", 494},
            {"After you", 495},
            {"Round", 496},
            {"Echoed voice", 497},
            {"Chip away", 498},
            {"Clear smog", 499},
            {"Stored power", 500},
            {"Quick guard", 501},
            {"Ally switch", 502},
            {"Scald", 503},
            {"Shell smash", 504},
            {"Heal pulse", 505},
            {"Hex", 506},
            {"Sky drop", 507},
            {"Shift gear", 508},
            {"Circle throw", 509},
            {"Incinerate", 510},
            {"Quash", 511},
            {"Acrobatics", 512},
            {"Reflect type", 513},
            {"Retaliate", 514},
            {"Final gambit", 515},
            {"Bestow", 516},
            {"Inferno", 517},
            {"Water pledge", 518},
            {"Fire pledge", 519},
            {"Grass pledge", 520},
            {"Volt switch", 521},
            {"Struggle bug", 522},
            {"Bulldoze", 523},
            {"Frost breath", 524},
            {"Dragon tail", 525},
            {"Work up", 526},
            {"Electroweb", 527},
            {"Wild charge", 528},
            {"Drill run", 529},
            {"Dual chop", 530},
            {"Heart stamp", 531},
            {"Horn leech", 532},
            {"Sacred sword", 533},
            {"Razor shell", 534},
            {"Heat crash", 535},
            {"Leaf tornado", 536},
            {"Steamroller", 537},
            {"Cotton guard", 538},
            {"Night daze", 539},
            {"Psystrike", 540},
            {"Tail slap", 541},
            {"Hurricane", 542},
            {"Head charge", 543},
            {"Gear grind", 544},
            {"Searing shot", 545},
            {"Techno blast", 546},
            {"Relic song", 547},
            {"Secret sword", 548},
            {"Glaciate", 549},
            {"Bolt strike", 550},
            {"Blue flare", 551},
            {"Fiery dance", 552},
            {"Freeze shock", 553},
            {"Ice burn", 554},
            {"Snarl", 555},
            {"Icicle crash", 556},
            {"V create", 557},
            {"Fusion flare", 558},
            {"Fusion bolt", 559},
            {"Baby doll eyes", 560},
            {"Belch", 561},
            {"Boomburst", 562},
            {"Confide", 563},
            {"Crafty shield", 564},
            {"Dazzling gleam", 565},
            {"Disarming voice", 566},
            {"Draining kiss", 567},
            {"Eerie impulse", 568},
            {"Electric terrain", 569},
            {"Electrify", 570},
            {"Fairy lock", 571},
            {"Fairy wind", 572},
            {"Fell stinger", 573},
            {"Flower shield", 574},
            {"Flying press", 575},
            {"Forests curse", 576},
            {"Freeze dry", 577},
            {"Geomancy", 578},
            {"Grassy terrain", 579},
            {"Infestation", 580},
            {"Ion deluge", 581},
            {"Kings shield", 582},
            {"Lands wrath", 583},
            {"Magnetic flux", 584},
            {"Mat block", 585},
            {"Misty terrain", 586},
            {"Moonblast", 587},
            {"Mystical fire", 588},
            {"Noble roar", 589},
            {"Nuzzle", 590},
            {"Oblivion wing", 591},
            {"Parabolic charge", 592},
            {"Parting shot", 593},
            {"Petal blizzard", 594},
            {"Phantom force", 595},
            {"Play nice", 596},
            {"Play rough", 597},
            {"Powder", 598},
            {"Power up punch", 599},
            {"Rototiller", 600},
            {"Spiky shield", 601},
            {"Sticky web", 602},
            {"Topsy turvy", 603},
            {"Trick or treat", 604},
            {"Venom drench", 605},
            {"Water shuriken", 606},
            {"Aromatic mist", 607},
            {"Shadow rush", 608},
            {"Shadow blast", 609},
            {"Shadow blitz", 610},
            {"Shadow bolt", 611},
            {"Shadow break", 612},
            {"Shadow chill", 613},
            {"Shadow end", 614},
            {"Shadow fire", 615},
            {"Shadow rave", 616},
            {"Shadow storm", 617},
            {"Shadow wave", 618},
            {"Shadow down", 619},
            {"Shadow half", 620},
            {"Shadow hold", 621},
            {"Shadow mist", 622},
            {"Shadow panic", 623},
            {"Shadow shed", 624},
            {"Shadow sky", 625}
        };
        #endregion

        string description;
        int power, pp;
        double accuracy;
        MoveCategory category;

        /// <summary>
        /// A description of the PokeMove instance
        /// </summary>
        public string Description
        {
            get
            {
                return description;
            }
        }
        /// <summary>
        /// The base power of the PokeMove instance
        /// </summary>
        public int Power
        {
            get
            {
                return power;
            }
        }
        /// <summary>
        /// The base power points of the PokeMove instance
        /// </summary>
        public int PP
        {
            get
            {
                return pp;
            }
        }
        /// <summary>
        /// The base accuracy of the PokeMove instance
        /// </summary>
        public double Accurracy
        {
            get
            {
                return accuracy;
            }
        }
        /// <summary>
        /// The category of the PokeMove instance
        /// </summary>
        public MoveCategory Category
        {
            get
            {
                return category;
            }
        }

        protected override void Create(JsonData source)
        {
            description = source["description"].ToString();
            power = (int)source["power"];
            pp = (int)source["pp"];
            accuracy = Convert.ToDouble(source["accuracy"].ToString(), CultureInfo.InvariantCulture) / 100d;
            MoveCategory cat = 0;
            Enum.TryParse<MoveCategory>(source["category"].ToString(), true, out cat);
            category = cat;

            if (ShouldCacheData && !CachedMoves.ContainsKey(id))
                CachedMoves.Add(id, this);
        }

        /// <summary>
        /// Creates an instance of a PokeMove with the given name
        /// </summary>
        /// <param name="name">The name of the PokeMove to instantiate</param>
        /// <returns>The created instance of the PokeMove</returns>
        public static PokeMove GetInstance(string name)
        {
            return GetInstance(IDs[name]);
        }
        /// <summary>
        /// Creates an instance of a PokeMove with the given ID
        /// </summary>
        /// <param name="id">The id of the PokeMove to instantiate</param>
        /// <returns>The created instance of the PokeMove</returns>
        public static PokeMove GetInstance(int id)
        {
            if (CachedMoves.ContainsKey(id))
                return CachedMoves[id];

            PokeMove p = new PokeMove();
            p = (PokeMove)PokeApiType.Create(DataFetcher.GetMove(id), p);

            if (ShouldCacheData)
                CachedMoves.Add(id, p);

            return p;
        }
    }
    /// <summary>
    /// Represents an instance of a Pokémon Ability
    /// </summary>
    public class PokeAbility : PokeApiType
    {
        public static bool ShouldCacheData = true;
        public static Dictionary<int, PokeAbility> CachedAbilities = new Dictionary<int, PokeAbility>();

        #region public readonly static Dictionary<string, int> IDs = new Dictionary<string, int>() { [...] };
        public readonly static Dictionary<string, int> IDs = new Dictionary<string, int>()
        {
            {"Stench", 1},
            {"Drizzle", 2},
            {"Speed boost", 3},
            {"Battle armor", 4},
            {"Sturdy", 5},
            {"Damp", 6},
            {"Limber", 7},
            {"Sand veil", 8},
            {"Static", 9},
            {"Volt absorb", 10},
            {"Water absorb", 11},
            {"Oblivious", 12},
            {"Cloud nine", 13},
            {"Compoundeyes", 14},
            {"Insomnia", 15},
            {"Color change", 16},
            {"Immunity", 17},
            {"Flash fire", 18},
            {"Shield dust", 19},
            {"Own tempo", 20},
            {"Suction cups", 21},
            {"Intimidate", 22},
            {"Shadow tag", 23},
            {"Rough skin", 24},
            {"Wonder guard", 25},
            {"Levitate", 26},
            {"Effect spore", 27},
            {"Synchronize", 28},
            {"Clear body", 29},
            {"Natural cure", 30},
            {"Lightningrod", 31},
            {"Serene grace", 32},
            {"Swift swim", 33},
            {"Chlorophyll", 34},
            {"Illuminate", 35},
            {"Trace", 36},
            {"Huge power", 37},
            {"Poison point", 38},
            {"Inner focus", 39},
            {"Magma armor", 40},
            {"Water veil", 41},
            {"Magnet pull", 42},
            {"Soundproof", 43},
            {"Rain dish", 44},
            {"Sand stream", 45},
            {"Pressure", 46},
            {"Thick fat", 47},
            {"Early bird", 48},
            {"Flame body", 49},
            {"Run away", 50},
            {"Keen eye", 51},
            {"Hyper cutter", 52},
            {"Pickup", 53},
            {"Truant", 54},
            {"Hustle", 55},
            {"Cute charm", 56},
            {"Plus", 57},
            {"Minus", 58},
            {"Forecast", 59},
            {"Sticky hold", 60},
            {"Shed skin", 61},
            {"Guts", 62},
            {"Marvel scale", 63},
            {"Liquid ooze", 64},
            {"Overgrow", 65},
            {"Blaze", 66},
            {"Torrent", 67},
            {"Swarm", 68},
            {"Rock head", 69},
            {"Drought", 70},
            {"Arena trap", 71},
            {"Vital spirit", 72},
            {"White smoke", 73},
            {"Pure power", 74},
            {"Shell armor", 75},
            {"Air lock", 76},
            {"Tangled feet", 77},
            {"Motor drive", 78},
            {"Rivalry", 79},
            {"Steadfast", 80},
            {"Snow cloak", 81},
            {"Gluttony", 82},
            {"Anger point", 83},
            {"Unburden", 84},
            {"Heatproof", 85},
            {"Simple", 86},
            {"Dry skin", 87},
            {"Download", 88},
            {"Iron fist", 89},
            {"Poison heal", 90},
            {"Adaptability", 91},
            {"Skill link", 92},
            {"Hydration", 93},
            {"Solar power", 94},
            {"Quick feet", 95},
            {"Normalize", 96},
            {"Sniper", 97},
            {"Magic guard", 98},
            {"No guard", 99},
            {"Stall", 100},
            {"Technician", 101},
            {"Leaf guard", 102},
            {"Klutz", 103},
            {"Mold breaker", 104},
            {"Super luck", 105},
            {"Aftermath", 106},
            {"Anticipation", 107},
            {"Forewarn", 108},
            {"Unaware", 109},
            {"Tinted lens", 110},
            {"Filter", 111},
            {"Slow start", 112},
            {"Scrappy", 113},
            {"Storm drain", 114},
            {"Ice body", 115},
            {"Solid rock", 116},
            {"Snow warning", 117},
            {"Honey gather", 118},
            {"Frisk", 119},
            {"Reckless", 120},
            {"Multitype", 121},
            {"Flower gift", 122},
            {"Bad dreams", 123},
            {"Pickpocket", 124},
            {"Sheer force", 125},
            {"Contrary", 126},
            {"Unnerve", 127},
            {"Defiant", 128},
            {"Defeatist", 129},
            {"Cursed body", 130},
            {"Healer", 131},
            {"Friend guard", 132},
            {"Weak armor", 133},
            {"Heavy metal", 134},
            {"Light metal", 135},
            {"Multiscale", 136},
            {"Toxic boost", 137},
            {"Flare boost", 138},
            {"Harvest", 139},
            {"Telepathy", 140},
            {"Moody", 141},
            {"Overcoat", 142},
            {"Poison touch", 143},
            {"Regenerator", 144},
            {"Big pecks", 145},
            {"Sand rush", 146},
            {"Wonder skin", 147},
            {"Analytic", 148},
            {"Illusion", 149},
            {"Imposter", 150},
            {"Infiltrator", 151},
            {"Mummy", 152},
            {"Moxie", 153},
            {"Justified", 154},
            {"Rattled", 155},
            {"Magic bounce", 156},
            {"Sap sipper", 157},
            {"Prankster", 158},
            {"Sand force", 159},
            {"Iron barbs", 160},
            {"Zen mode", 161},
            {"Victory star", 162},
            {"Turboblaze", 163},
            {"Teravolt", 164},
            {"Aerilate", 165},
            {"Aroma veil", 166},
            {"Aura break", 167},
            {"Bulletproof", 168},
            {"Cheek pouch", 169},
            {"Competitive", 170},
            {"Dark aura", 171},
            {"Fairy aura", 172},
            {"Flower veil", 173},
            {"Fur coat", 174},
            {"Gale wings", 175},
            {"Gooey", 176},
            {"Grass pelt", 177},
            {"Magician", 178},
            {"Mega launcher", 179},
            {"Parental bond", 180},
            {"Pixilate", 181},
            {"Protean", 182},
            {"Refrigerate", 183},
            {"Stance change", 184},
            {"Strong jaw", 185},
            {"Sweet veil", 186},
            {"Symbiosis", 187},
            {"Tough claws", 188},
            {"Mountaineer", 189},
            {"Wave rider", 190},
            {"Skater", 191},
            {"Thrust", 192},
            {"Perception", 193},
            {"Parry", 194},
            {"Instinct", 195},
            {"Dodge", 196},
            {"Jagged edge", 197},
            {"Frostbite", 198},
            {"Tenacity", 199},
            {"Pride", 200},
            {"Deep sleep", 201},
            {"Power nap", 202},
            {"Spirit", 203},
            {"Warm blanket", 204},
            {"Gulp", 205},
            {"Herbivore", 206},
            {"Sandpit", 207},
            {"Hot blooded", 208},
            {"Medic", 209},
            {"Life force", 210},
            {"Lunchbox", 211},
            {"Nurse", 212},
            {"Melee", 213},
            {"Sponge", 214},
            {"Bodyguard", 215},
            {"Hero", 216},
            {"Last bastion", 217},
            {"Stealth", 218},
            {"Vanguard", 219},
            {"Nomad", 220},
            {"Sequence", 221},
            {"Grass cloak", 222},
            {"Celebrate", 223},
            {"Lullaby", 224},
            {"Calming", 225},
            {"Daze", 226},
            {"Frighten", 227},
            {"Interference", 228},
            {"Mood maker", 229},
            {"Confidence", 230},
            {"Fortune", 231},
            {"Bonanza", 232},
            {"Explode", 233},
            {"Omnipotent", 234},
            {"Share", 235},
            {"Black hole", 236},
            {"Shadow dash", 237},
            {"Sprint", 238},
            {"Disgust", 239},
            {"High rise", 240},
            {"Climber", 241},
            {"Flame boost", 242},
            {"Aqua boost", 243},
            {"Run up", 244},
            {"Conqueror", 245},
            {"Shackle", 246},
            {"Decoy", 247},
            {"Shield", 248}
        };
        #endregion

        string description;

        /// <summary>
        /// The description of this PokeAbility instance
        /// </summary>
        public string Description
        {
            get
            {
                return description;
            }
        }

        protected override void Create(JsonData source)
        {
            description = source["description"].ToString();

            if (ShouldCacheData && !CachedAbilities.ContainsKey(id))
                CachedAbilities.Add(id, this);
        }

        /// <summary>
        /// Creates an instance of a PokeAbility with the given Ability
        /// </summary>
        /// <param name="ability">The Ability of the PokeAbility to instantiate</param>
        /// <returns>The created instance of the PokeAbility</returns>
        public static PokeAbility GetInstance(Ability ability)
        {
            return GetInstance((int)ability + 1);
        }
        /// <summary>
        /// Creates an instance of a PokeAbility with the given name
        /// </summary>
        /// <param name="name">The name of the PokeAbility to instantiate</param>
        /// <returns>The created instance of the PokeAbility</returns>
        public static PokeAbility GetInstance(string name)
        {
            return GetInstance(IDs[name]);
        }
        /// <summary>
        /// Creates an instance of a PokeAbility with the given ID
        /// </summary>
        /// <param name="id">The id of the PokeAbility to instantiate</param>
        /// <returns>The created instance of the PokeAbility</returns>
        public static PokeAbility GetInstance(int id)
        {
            if (CachedAbilities.ContainsKey(id))
                return CachedAbilities[id];

            PokeAbility p = new PokeAbility();
            p = (PokeAbility)PokeApiType.Create(DataFetcher.GetAbility(id), p);

            if (ShouldCacheData)
                CachedAbilities.Add(id, p);

            return p;
        }

        public static implicit operator Ability(PokeAbility ability)
        {
            // lazy<me>
            Ability ret = 0;
            Enum.TryParse<Ability>(ability.name.Replace(' ', '_'), false, out ret);
            return ret;
        }
    }
    /// <summary>
    /// Represents an instance of a Pokémon Egg Group
    /// </summary>
    public class PokeEggGroup : PokeApiType
    {
        public static bool ShouldCacheData = true;
        public static Dictionary<int, PokeEggGroup> CachedEggGroups = new Dictionary<int, PokeEggGroup>();

        #region public readonly static Dictionary<string, int> IDs = new Dictionary<string, int>() { [...] };
        public readonly static Dictionary<string, int> IDs = new Dictionary<string, int>()
        {
            {"Monster", 1},
            {"Water1", 2},
            {"Bug", 3},
            {"Flying", 4},
            {"Ground", 5},
            {"Fairy", 6},
            {"Plant", 7},
            {"Human-like", 8},
            {"Water3", 9},
            {"Mineral", 10},
            {"Indeterminate", 11},
            {"Water2", 12},
            {"Ditto", 13},
            {"Dragon", 14},
            {"Undiscovered", 15}
        };
        #endregion

        List<NameUriPair> pokemon = new List<NameUriPair>();
        /// <summary>
        /// A list of all the Pokemon in this PokeEggGroup
        /// </summary>
        public List<NameUriPair> Pokemon
        {
            get
            {
                return pokemon;
            }
        }

        /// <summary>
        /// Gets an entry of the Pokemon list as a Pokemon
        /// </summary>
        /// <param name="index">The index of the entry</param>
        /// <returns>The entry of the Pokemon list as a Pokemon</returns>
        public Pokemon RefPokemon(int index)
        {
            return NET.Pokemon.GetInstance(Pokemon[index].Name);
        }

        protected override void Create(JsonData source)
        {
            foreach (JsonData data in source["pokemon"])
                pokemon.Add(ParseNameUriPair(data));

            if (ShouldCacheData && !CachedEggGroups.ContainsKey(id))
                CachedEggGroups.Add(id, this);
        }

        /// <summary>
        /// Creates an instance of a PokeEggGroup with the given EggGroup
        /// </summary>
        /// <param name="eggGroup">The EggGroup of the PokeEggGroup to instantiate</param>
        /// <returns>The created instance of the PokeEggGroup</returns>
        public static PokeEggGroup GetInstance(EggGroup eggGroup)
        {
            return GetInstance((int)eggGroup + 1);
        }
        /// <summary>
        /// Creates an instance of a PokeEggGroup with the given name
        /// </summary>
        /// <param name="name">The name of the PokeEggGroup to instantiate</param>
        /// <returns>The created instance of the PokeEggGroup</returns>
        public static PokeEggGroup GetInstance(string name)
        {
            return GetInstance(IDs[name]);
        }
        /// <summary>
        /// Creates an instance of a PokeEggGroup with the given ID
        /// </summary>
        /// <param name="id">The id of the PokeEggGroup to instantiate</param>
        /// <returns>The created instance of the PokeEggGroup</returns>
        public static PokeEggGroup GetInstance(int id)
        {
            if (CachedEggGroups.ContainsKey(id))
                return CachedEggGroups[id];

            PokeEggGroup p = new PokeEggGroup();
            p = (PokeEggGroup)PokeApiType.Create(DataFetcher.GetEggGroup(id), p);

            if (ShouldCacheData)
                CachedEggGroups.Add(id, p);

            return p;
        }

        public static implicit operator EggGroup(PokeEggGroup eggGroup)
        {
            return (EggGroup)(eggGroup.id + 1);
        }
    }
    /// <summary>
    /// Represents an instance of a Pokémon Game
    /// </summary>
    public class PokeGame : PokeApiType
    {
        public static bool ShouldCacheData = true;
        public static Dictionary<int, PokeGame> CachedGames = new Dictionary<int, PokeGame>();

        #region public readonly static Dictionary<string, int> IDs = new Dictionary<string, int>() { [...] };
        public readonly static Dictionary<string, int> IDs = new Dictionary<string, int>()
        {
            {"Red (jpn)", 1},
            {"Green (jpn)", 2},
            {"Blue (jpn)", 3},
            {"Red", 4},
            {"Blue", 5},
            {"Yellow", 6},
            {"Gold", 7},
            {"Silver", 8},
            {"Crystal", 9},
            {"Ruby", 10},
            {"Sapphire", 11},
            {"Firered", 12},
            {"Leafgreen", 13},
            {"Emerald", 14},
            {"Diamond", 15},
            {"Pearl", 16},
            {"Platinum", 17},
            {"Heartgold", 18},
            {"Soulsilver", 19},
            {"Black", 20},
            {"White", 21},
            {"Black 2", 22},
            {"White 2", 23},
            {"X", 24},
            {"Y", 25}
        };
        #endregion

        int year, gen;
        /// <summary>
        /// The year the PokeGame instance was released
        /// </summary>
        public int ReleaseYear
        {
            get
            {
                return year;
            }
        }
        /// <summary>
        /// The generation this PokeGame instance belongs to
        /// </summary>
        public int Generation
        {
            get
            {
                return gen;
            }
        }

        protected override void Create(JsonData source)
        {
            year = (int)source["release_year"];
            gen = (int)source["generation"];

            if (ShouldCacheData && !CachedGames.ContainsKey(id))
                CachedGames.Add(id, this);
        }

        /// <summary>
        /// Creates an instance of a PokeGame with the given PokemonGame
        /// </summary>
        /// <param name="game">The PokemonGame of the PokeGame to instantiate</param>
        /// <returns>The created instance of the PokeGame</returns>
        public static PokeGame GetInstance(PokemonGame game)
        {
            return GetInstance((int)game + 1);
        }
        /// <summary>
        /// Creates an instance of a PokeGame with the given name
        /// </summary>
        /// <param name="name">The name of the PokeGame to instantiate</param>
        /// <returns>The created instance of the PokeGame</returns>
        public static PokeGame GetInstance(string name)
        {
            return GetInstance(IDs[name]);
        }
        /// <summary>
        /// Creates an instance of a PokeGame with the given ID
        /// </summary>
        /// <param name="id">The id of the PokeGame to instantiate</param>
        /// <returns>The created instance of the PokeGame</returns>
        public static PokeGame GetInstance(int id)
        {
            if (CachedGames.ContainsKey(id))
                return CachedGames[id];

            PokeGame p = new PokeGame();
            p = (PokeGame)PokeApiType.Create(DataFetcher.GetGame(id), p);

            if (ShouldCacheData)
                CachedGames.Add(id, p);

            return p;
        }

        public static implicit operator PokemonGame(PokeGame game)
        {
            return (PokemonGame)(game.id - 1);
        }
    }
    /// <summary>
    /// [API DATABASE NOT FINISHED]
    /// Represents an isntance of a description of a Pokémon in a Game
    /// </summary>
    [Obsolete] // database still WIP
    public class PokeDescription : PokeApiType
    {
        public static bool ShouldCacheData = true;
        public static Dictionary<int, PokeDescription> CachedDescriptions = new Dictionary<int, PokeDescription>();

        List<NameUriPair> games = new List<NameUriPair>();
        NameUriPair pokemon = new NameUriPair();
        /// <summary>
        /// A list of games this PokeDescription instance is in
        /// </summary>
        public List<NameUriPair> Games
        {
            get
            {
                return games;
            }
        }
        /// <summary>
        /// The pokemon this PokeDescription instance is for
        /// </summary>
        NameUriPair Pokemon
        {
            get
            {
                return pokemon;
            }
        }

        /// <summary>
        /// Gets an entry of the Games list as a PokeGame
        /// </summary>
        /// <param name="index">The index of the entry</param>
        /// <returns>The entry of the Games list as a PokeGame</returns>
        public PokeGame RefGame(int index)
        {
            return PokeGame.GetInstance(Games[index].Name);
        }

        protected override void Create(JsonData source)
        {
            foreach (JsonData data in source["games"])
                games.Add(ParseNameUriPair(data));

            pokemon = ParseNameUriPair(source["pokemon"]);

            if (ShouldCacheData && !CachedDescriptions.ContainsKey(id))
                CachedDescriptions.Add(id, this);
        }

        /// <summary>
        /// Creates an instance of a PokeDescription with the given ID
        /// </summary>
        /// <param name="id">The id of the PokeDescription to instantiate</param>
        /// <returns>The created instance of the PokeDescription</returns>
        public static PokeDescription GetInstance(int id)
        {
            if (CachedDescriptions.ContainsKey(id))
                return CachedDescriptions[id];

            PokeDescription p = new PokeDescription();
            p = (PokeDescription)PokeApiType.Create(DataFetcher.GetDescription(id), p);

            if (ShouldCacheData)
                CachedDescriptions.Add(id, p);

            return p;
        }
    }
    /// <summary>
    /// Represents the sprite of a Pokémon
    /// </summary>
    public class PokeSprite : PokeApiType
    {
        public static bool ShouldCacheData = true;
        public static Dictionary<int, PokeSprite> CachedSprites = new Dictionary<int, PokeSprite>();

        #region public static Dictionary<string, int> IDs = new Dictionary<string, int>() { [...] };
        public static Dictionary<string, int> IDs = new Dictionary<string, int>()
        { 
            {"Bulbasaur Red&Blue", 1},
            {"Bulbasaur", 2},
            {"Ivysaur", 3},
            {"Venusaur", 4},
            {"Charmander", 5},
            {"Charmeleon", 6},
            {"Charizard", 7},
            {"Squirtle", 8},
            {"Wartortle", 9},
            {"Blastoise", 10},
            {"Caterpie", 11},
            {"Metapod", 12},
            {"Butterfree", 13},
            {"Weedle", 14},
            {"Kakuna", 15},
            {"Beedrill", 16},
            {"Pidgey", 17},
            {"Pidgeotto", 18},
            {"Pidgeot", 19},
            {"Rattata", 20},
            {"Raticate", 21},
            {"Spearow", 22},
            {"Fearow", 23},
            {"Ekans", 24},
            {"Arbok", 25},
            {"Pikachu", 26},
            {"Raichu", 27},
            {"Sandshrew", 28},
            {"Sandslash", 29},
            {"Nidoran F", 30},
            {"Nidorina", 31},
            {"Nidoqueen", 32},
            {"Nidoran M", 33},
            {"Nidorino", 34},
            {"Nidoking", 35},
            {"Clefairy", 36},
            {"Clefable", 37},
            {"Vulpix", 38},
            {"Ninetales", 39},
            {"Jigglypuff", 40},
            {"Wigglytuff", 41},
            {"Zubat", 42},
            {"Golbat", 43},
            {"Oddish", 44},
            {"Gloom", 45},
            {"Vileplume", 46},
            {"Paras", 47},
            {"Parasect", 48},
            {"Venonat", 49},
            {"Venomoth", 50},
            {"Diglett", 51},
            {"Dugtrio", 52},
            {"Meowth", 53},
            {"Persian", 54},
            {"Psyduck", 55},
            {"Golduck", 56},
            {"Mankey", 57},
            {"Primeape", 58},
            {"Growlithe", 59},
            {"Arcanine", 60},
            {"Poliwag", 61},
            {"Poliwhirl", 62},
            {"Poliwrath", 63},
            {"Abra", 64},
            {"Kadabra", 65},
            {"Alakazam", 66},
            {"Machop", 67},
            {"Machoke", 68},
            {"Machamp", 69},
            {"Bellsprout", 70},
            {"Weepinbell", 71},
            {"Victreebel", 72},
            {"Tentacool", 73},
            {"Tentacruel", 74},
            {"Geodude", 75},
            {"Graveler", 76},
            {"Golem", 77},
            {"Ponyta", 78},
            {"Rapidash", 79},
            {"Slowpoke", 80},
            {"Slowbro", 81},
            {"Magnemite", 82},
            {"Magneton", 83},
            {"Farfetchd", 84},
            {"Doduo", 85},
            {"Dodrio", 86},
            {"Seel", 87},
            {"Dewgong", 88},
            {"Grimer", 89},
            {"Muk", 90},
            {"Shellder", 91},
            {"Cloyster", 92},
            {"Gastly", 93},
            {"Haunter", 94},
            {"Gengar", 95},
            {"Onix", 96},
            {"Drowzee", 97},
            {"Hypno", 98},
            {"Krabby", 99},
            {"Kingler", 100},
            {"Voltorb", 101},
            {"Electrode", 102},
            {"Exeggcute", 103},
            {"Exeggutor", 104},
            {"Cubone", 105},
            {"Marowak", 106},
            {"Hitmonlee", 107},
            {"Hitmonchan", 108},
            {"Lickitung", 109},
            {"Koffing", 110},
            {"Weezing", 111},
            {"Rhyhorn", 112},
            {"Rhydon", 113},
            {"Chansey", 114},
            {"Tangela", 115},
            {"Kangaskhan", 116},
            {"Horsea", 117},
            {"Seadra", 118},
            {"Goldeen", 119},
            {"Seaking", 120},
            {"Staryu", 121},
            {"Starmie", 122},
            {"Mr. Mime", 123},
            {"Scyther", 124},
            {"Jynx", 125},
            {"Electabuzz", 126},
            {"Magmar", 127},
            {"Pinsir", 128},
            {"Tauros", 129},
            {"Magikarp", 130},
            {"Gyarados", 131},
            {"Lapras", 132},
            {"Ditto", 133},
            {"Eevee", 134},
            {"Vaporeon", 135},
            {"Jolteon", 136},
            {"Flareon", 137},
            {"Porygon", 138},
            {"Omanyte", 139},
            {"Omastar", 140},
            {"Kabuto", 141},
            {"Kabutops", 142},
            {"Aerodactyl", 143},
            {"Snorlax", 144},
            {"Articuno", 145},
            {"Zapdos", 146},
            {"Moltres", 147},
            {"Dratini", 148},
            {"Dragonair", 149},
            {"Dragonite", 150},
            {"Mewtwo", 151},
            {"Mew", 152},
            {"Chikorita", 153},
            {"Bayleef", 154},
            {"Meganium", 155},
            {"Cyndaquil", 156},
            {"Quilava", 157},
            {"Typhlosion", 158},
            {"Totodile", 159},
            {"Croconaw", 160},
            {"Feraligatr", 161},
            {"Sentret", 162},
            {"Furret", 163},
            {"Hoothoot", 164},
            {"Noctowl", 165},
            {"Ledyba", 166},
            {"Ledian", 167},
            {"Spinarak", 168},
            {"Ariados", 169},
            {"Crobat", 170},
            {"Chinchou", 171},
            {"Lanturn", 172},
            {"Pichu", 173},
            {"Cleffa", 174},
            {"Igglybuff", 175},
            {"Togepi", 176},
            {"Togetic", 177},
            {"Natu", 178},
            {"Xatu", 179},
            {"Mareep", 180},
            {"Flaaffy", 181},
            {"Ampharos", 182},
            {"Bellossom", 183},
            {"Marill", 184},
            {"Azumarill", 185},
            {"Sudowoodo", 186},
            {"Politoed", 187},
            {"Hoppip", 188},
            {"Skiploom", 189},
            {"Jumpluff", 190},
            {"Aipom", 191},
            {"Sunkern", 192},
            {"Sunflora", 193},
            {"Yanma", 194},
            {"Wooper", 195},
            {"Quagsire", 196},
            {"Espeon", 197},
            {"Umbreon", 198},
            {"Murkrow", 199},
            {"Slowking", 200},
            {"Misdreavus", 201},
            {"Unown", 202},
            {"Wobbuffet", 203},
            {"Girafarig", 204},
            {"Pineco", 205},
            {"Forretress", 206},
            {"Dunsparce", 207},
            {"Gligar", 208},
            {"Steelix", 209},
            {"Snubbull", 210},
            {"Granbull", 211},
            {"Qwilfish", 212},
            {"Scizor", 213},
            {"Shuckle", 214},
            {"Heracross", 215},
            {"Sneasel", 216},
            {"Teddiursa", 217},
            {"Ursaring", 218},
            {"Slugma", 219},
            {"Magcargo", 220},
            {"Swinub", 221},
            {"Piloswine", 222},
            {"Corsola", 223},
            {"Remoraid", 224},
            {"Octillery", 225},
            {"Delibird", 226},
            {"Mantine", 227},
            {"Skarmory", 228},
            {"Houndour", 229},
            {"Houndoom", 230},
            {"Kingdra", 231},
            {"Phanpy", 232},
            {"Donphan", 233},
            {"Porygon2", 234},
            {"Stantler", 235},
            {"Smeargle", 236},
            {"Tyrogue", 237},
            {"Hitmontop", 238},
            {"Smoochum", 239},
            {"Elekid", 240},
            {"Magby", 241},
            {"Miltank", 242},
            {"Blissey", 243},
            {"Raikou", 244},
            {"Entei", 245},
            {"Suicune", 246},
            {"Larvitar", 247},
            {"Pupitar", 248},
            {"Tyranitar", 249},
            {"Lugia", 250},
            {"Ho-oh", 251},
            {"Celebi", 252},
            {"Treecko", 253},
            {"Grovyle", 254},
            {"Sceptile", 255},
            {"Torchic", 256},
            {"Combusken", 257},
            {"Blaziken", 258},
            {"Mudkip", 259},
            {"Marshtomp", 260},
            {"Swampert", 261},
            {"Poochyena", 262},
            {"Mightyena", 263},
            {"Zigzagoon", 264},
            {"Linoone", 265},
            {"Wurmple", 266},
            {"Silcoon", 267},
            {"Beautifly", 268},
            {"Cascoon", 269},
            {"Dustox", 270},
            {"Lotad", 271},
            {"Lombre", 272},
            {"Ludicolo", 273},
            {"Seedot", 274},
            {"Nuzleaf", 275},
            {"Shiftry", 276},
            {"Taillow", 277},
            {"Swellow", 278},
            {"Wingull", 279},
            {"Pelipper", 280},
            {"Ralts", 281},
            {"Kirlia", 282},
            {"Gardevoir", 283},
            {"Surskit", 284},
            {"Masquerain", 285},
            {"Shroomish", 286},
            {"Breloom", 287},
            {"Slakoth", 288},
            {"Vigoroth", 289},
            {"Slaking", 290},
            {"Nincada", 291},
            {"Ninjask", 292},
            {"Shedinja", 293},
            {"Whismur", 294},
            {"Loudred", 295},
            {"Exploud", 296},
            {"Makuhita", 297},
            {"Hariyama", 298},
            {"Azurill", 299},
            {"Nosepass", 300},
            {"Skitty", 301},
            {"Delcatty", 302},
            {"Sableye", 303},
            {"Mawile", 304},
            {"Aron", 305},
            {"Lairon", 306},
            {"Aggron", 307},
            {"Meditite", 308},
            {"Medicham", 309},
            {"Electrike", 310},
            {"Manectric", 311},
            {"Plusle", 312},
            {"Minun", 313},
            {"Volbeat", 314},
            {"Illumise", 315},
            {"Roselia", 316},
            {"Gulpin", 317},
            {"Swalot", 318},
            {"Carvanha", 319},
            {"Sharpedo", 320},
            {"Wailmer", 321},
            {"Wailord", 322},
            {"Numel", 323},
            {"Camerupt", 324},
            {"Torkoal", 325},
            {"Spoink", 326},
            {"Grumpig", 327},
            {"Spinda", 328},
            {"Trapinch", 329},
            {"Vibrava", 330},
            {"Flygon", 331},
            {"Cacnea", 332},
            {"Cacturne", 333},
            {"Swablu", 334},
            {"Altaria", 335},
            {"Zangoose", 336},
            {"Seviper", 337},
            {"Lunatone", 338},
            {"Solrock", 339},
            {"Barboach", 340},
            {"Whiscash", 341},
            {"Corphish", 342},
            {"Crawdaunt", 343},
            {"Baltoy", 344},
            {"Claydol", 345},
            {"Lileep", 346},
            {"Cradily", 347},
            {"Anorith", 348},
            {"Armaldo", 349},
            {"Feebas", 350},
            {"Milotic", 351},
            {"Castform", 352},
            {"Kecleon", 353},
            {"Shuppet", 354},
            {"Banette", 355},
            {"Duskull", 356},
            {"Dusclops", 357},
            {"Tropius", 358},
            {"Chimecho", 359},
            {"Absol", 360},
            {"Wynaut", 361},
            {"Snorunt", 362},
            {"Glalie", 363},
            {"Spheal", 364},
            {"Sealeo", 365},
            {"Walrein", 366},
            {"Clamperl", 367},
            {"Huntail", 368},
            {"Gorebyss", 369},
            {"Relicanth", 370},
            {"Luvdisc", 371},
            {"Bagon", 372},
            {"Shelgon", 373},
            {"Salamence", 374},
            {"Beldum", 375},
            {"Metang", 376},
            {"Metagross", 377},
            {"Regirock", 378},
            {"Regice", 379},
            {"Registeel", 380},
            {"Latias", 381},
            {"Latios", 382},
            {"Kyogre", 383},
            {"Groudon", 384},
            {"Rayquaza", 385},
            {"Jirachi", 386},
            {"Deoxys", 387},
            {"Turtwig", 388},
            {"Grotle", 389},
            {"Torterra", 390},
            {"Chimchar", 391},
            {"Monferno", 392},
            {"Infernape", 393},
            {"Piplup", 394},
            {"Prinplup", 395},
            {"Empoleon", 396},
            {"Starly", 397},
            {"Staravia", 398},
            {"Staraptor", 399},
            {"Bidoof", 400},
            {"Bibarel", 401},
            {"Kricketot", 402},
            {"Kricketune", 403},
            {"Shinx", 404},
            {"Luxio", 405},
            {"Luxray", 406},
            {"Budew", 407},
            {"Roserade", 408},
            {"Cranidos", 409},
            {"Rampardos", 410},
            {"Shieldon", 411},
            {"Bastiodon", 412},
            {"Burmy", 413},
            {"Wormadam", 414},
            {"Mothim", 415},
            {"Combee", 416},
            {"Vespiquen", 417},
            {"Pachirisu", 418},
            {"Buizel", 419},
            {"Floatzel", 420},
            {"Cherubi", 421},
            {"Cherrim", 422},
            {"Shellos", 423},
            {"Gastrodon", 424},
            {"Ambipom", 425},
            {"Drifloon", 426},
            {"Drifblim", 427},
            {"Buneary", 428},
            {"Lopunny", 429},
            {"Mismagius", 430},
            {"Honchkrow", 431},
            {"Glameow", 432},
            {"Purugly", 433},
            {"Chingling", 434},
            {"Stunky", 435},
            {"Skuntank", 436},
            {"Bronzor", 437},
            {"Bronzong", 438},
            {"Bonsly", 439},
            {"Mime Jr.", 440},
            {"Happiny", 441},
            {"Chatot", 442},
            {"Spiritomb", 443},
            {"Gible", 444},
            {"Gabite", 445},
            {"Garchomp", 446},
            {"Munchlax", 447},
            {"Riolu", 448},
            {"Lucario", 449},
            {"Hippopotas", 450},
            {"Hippowdon", 451},
            {"Skorupi", 452},
            {"Drapion", 453},
            {"Croagunk", 454},
            {"Toxicroak", 455},
            {"Carnivine", 456},
            {"Finneon", 457},
            {"Lumineon", 458},
            {"Mantyke", 459},
            {"Snover", 460},
            {"Abomasnow", 461},
            {"Weavile", 462},
            {"Magnezone", 463},
            {"Lickilicky", 464},
            {"Rhyperior", 465},
            {"Tangrowth", 466},
            {"Electivire", 467},
            {"Magmortar", 468},
            {"Togekiss", 469},
            {"Yanmega", 470},
            {"Leafeon", 471},
            {"Glaceon", 472},
            {"Gliscor", 473},
            {"Mamoswine", 474},
            {"Porygon-Z", 475},
            {"Gallade", 476},
            {"Probopass", 477},
            {"Dusknoir", 478},
            {"Froslass", 479},
            {"Rotom", 480},
            {"Uxie", 481},
            {"Mesprit", 482},
            {"Azelf", 483},
            {"Dialga", 484},
            {"Palkia", 485},
            {"Heatran", 486},
            {"Regigigas", 487},
            {"Giratina", 488},
            {"Cresselia", 489},
            {"Phione", 490},
            {"Manaphy", 491},
            {"Darkrai", 492},
            {"Shaymin", 493},
            {"Arceus", 494},
            {"Victini", 495},
            {"Snivy", 496},
            {"Servine", 497},
            {"Serperior", 498},
            {"Tepig", 499},
            {"Pignite", 500},
            {"Emboar", 501},
            {"Oshawott", 502},
            {"Dewott", 503},
            {"Samurott", 504},
            {"Patrat", 505},
            {"Watchog", 506},
            {"Lillipup", 507},
            {"Herdier", 508},
            {"Stoutland", 509},
            {"Purrloin", 510},
            {"Liepard", 511},
            {"Pansage", 512},
            {"Simisage", 513},
            {"Pansear", 514},
            {"Simisear", 515},
            {"Panpour", 516},
            {"Simipour", 517},
            {"Munna", 518},
            {"Musharna", 519},
            {"Pidove", 520},
            {"Tranquill", 521},
            {"Unfezant", 522},
            {"Blitzle", 523},
            {"Zebstrika", 524},
            {"Roggenrola", 525},
            {"Boldore", 526},
            {"Gigalith", 527},
            {"Woobat", 528},
            {"Swoobat", 529},
            {"Drilbur", 530},
            {"Excadrill", 531},
            {"Audino", 532},
            {"Timburr", 533},
            {"Gurdurr", 534},
            {"Conkeldurr", 535},
            {"Tympole", 536},
            {"Palpitoad", 537},
            {"Seismitoad", 538},
            {"Throh", 539},
            {"Sawk", 540},
            {"Sewaddle", 541},
            {"Swadloon", 542},
            {"Leavanny", 543},
            {"Venipede", 544},
            {"Whirlipede", 545},
            {"Scolipede", 546},
            {"Cottonee", 547},
            {"Whimsicott", 548},
            {"Petilil", 549},
            {"Lilligant", 550},
            {"Basculin", 551},
            {"Sandile", 552},
            {"Krokorok", 553},
            {"Krookodile", 554},
            {"Darumaka", 555},
            {"Darmanitan", 556},
            {"Maractus", 557},
            {"Dwebble", 558},
            {"Crustle", 559},
            {"Scraggy", 560},
            {"Scrafty", 561},
            {"Sigilyph", 562},
            {"Yamask", 563},
            {"Cofagrigus", 564},
            {"Tirtouga", 565},
            {"Carracosta", 566},
            {"Archen", 567},
            {"Archeops", 568},
            {"Trubbish", 569},
            {"Garbodor", 570},
            {"Zorua", 571},
            {"Zoroark", 572},
            {"Minccino", 573},
            {"Cinccino", 574},
            {"Gothita", 575},
            {"Gothorita", 576},
            {"Gothitelle", 577},
            {"Solosis", 578},
            {"Duosion", 579},
            {"Reuniclus", 580},
            {"Ducklett", 581},
            {"Swanna", 582},
            {"Vanillite", 583},
            {"Vanillish", 584},
            {"Vanilluxe", 585},
            {"Deerling", 586},
            {"Sawsbuck", 587},
            {"Emolga", 588},
            {"Karrablast", 589},
            {"Escavalier", 590},
            {"Foongus", 591},
            {"Amoonguss", 592},
            {"Frillish", 593},
            {"Jellicent", 594},
            {"Alomomola", 595},
            {"Joltik", 596},
            {"Galvantula", 597},
            {"Ferroseed", 598},
            {"Ferrothorn", 599},
            {"Klink", 600},
            {"Klang", 601},
            {"Klinklang", 602},
            {"Tynamo", 603},
            {"Eelektrik", 604},
            {"Eelektross", 605},
            {"Elgyem", 606},
            {"Beheeyem", 607},
            {"Litwick", 608},
            {"Lampent", 609},
            {"Chandelure", 610},
            {"Axew", 611},
            {"Fraxure", 612},
            {"Haxorus", 613},
            {"Cubchoo", 614},
            {"Beartic", 615},
            {"Cryogonal", 616},
            {"Shelmet", 617},
            {"Accelgor", 618},
            {"Stunfisk", 619},
            {"Mienfoo", 620},
            {"Mienshao", 621},
            {"Druddigon", 622},
            {"Golett", 623},
            {"Golurk", 624},
            {"Pawniard", 625},
            {"Bisharp", 626},
            {"Bouffalant", 627},
            {"Rufflet", 628},
            {"Braviary", 629},
            {"Vullaby", 630},
            {"Mandibuzz", 631},
            {"Heatmor", 632},
            {"Durant", 633},
            {"Deino", 634},
            {"Zweilous", 635},
            {"Hydreigon", 636},
            {"Larvesta", 637},
            {"Volcarona", 638},
            {"Cobalion", 639},
            {"Terrakion", 640},
            {"Virizion", 641},
            {"Tornadus", 642},
            {"Thundurus", 643},
            {"Reshiram", 644},
            {"Zekrom", 645},
            {"Landorus", 646},
            {"Kyurem", 647},
            {"Keldeo", 648},
            {"Meloetta", 649},
            {"Genesect", 650},
            {"Chespin", 651},
            {"Quilladin", 652},
            {"Chesnaught", 653},
            {"Fennekin", 654},
            {"Braixen", 655},
            {"Delphox", 656},
            {"Froakie", 657},
            {"Frogadier", 658},
            {"Greninja", 659},
            {"Bunnelby", 660},
            {"Diggersby", 661},
            {"Fletchling", 662},
            {"Fletchinder", 663},
            {"Talonflame", 664},
            {"Scatterbug", 665},
            {"Spewpa", 666},
            {"Vivillon", 667},
            {"Litleo", 668},
            {"Pyroar", 669},
            {"Flabebe", 670},
            {"Floette", 671},
            {"Florges", 672},
            {"Skiddo", 673},
            {"Gogoat", 674},
            {"Pancham", 675},
            {"Pangoro", 676},
            {"Furfrou", 677},
            {"Espurr", 678},
            {"Meowstic", 679},
            {"Honedge", 680},
            {"Doublade", 681},
            {"Aegislash", 682},
            {"Spritzee", 683},
            {"Aromatisse", 684},
            {"Swirlix", 685},
            {"Slurpuff", 686},
            {"Inkay", 687},
            {"Malamar", 688},
            {"Binacle", 689},
            {"Barbaracle", 690},
            {"Skrelp", 691},
            {"Dragalge", 692},
            {"Clauncher", 693},
            {"Clawitzer", 694},
            {"Helioptile", 695},
            {"Heliolisk", 696},
            {"Tyrunt", 697},
            {"Tyrantrum", 698},
            {"Amaura", 699},
            {"Aurorus", 700},
            {"Sylveon", 701},
            {"Hawlucha", 702},
            {"Dedenne", 703},
            {"Carbink", 704},
            {"Goomy", 705},
            {"Sliggoo", 706},
            {"Goodra", 707},
            {"Klefki", 708},
            {"Phantump", 709},
            {"Trevenant", 710},
            {"Pumpkaboo", 711},
            {"Gourgeist", 712},
            {"Bergmite", 713},
            {"Avalugg", 714},
            {"Noibat", 715},
            {"Noivern", 716},
            {"Xerneas", 717},
            {"Yveltal", 718},
            {"Zygarde", 719}
        };
        #endregion

        bool creating = true, loadImg = false;
        byte[] imgData = null;

        public PokeSprite(bool loadImageAfterCreation = false)
        {
            LoadImageAfterCreation = loadImageAfterCreation;
        }

        NameUriPair pokemon;
        Uri imageUri;

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
                    throw new InvalidOperationException("LoadImageAftercreation should only be called before the PokeSprite is initialized");

                creating = value;
            }
        }

        /// <summary>
        /// The Pokemon this PokeSprite instance is for
        /// </summary>
        public NameUriPair Pokemon
        {
            get
            {
                return pokemon;
            }
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
            get
            {
                return imageUri;
            }
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
                imgData = new WebClient().DownloadData(imageUri);
        }

        protected override void Create(JsonData source)
        {
            pokemon = ParseNameUriPair(source["pokemon"].ToString());
            imageUri = new Uri("http://www.pokeapi/co" + source["image"].ToString());

            if (LoadImageAfterCreation)
                LoadImageData();

            creating = false;

            if (ShouldCacheData && !CachedSprites.ContainsKey(id))
                CachedSprites.Add(id, this);
        }

        /// <summary>
        /// Creates an instance of a PokeSprite with the given name
        /// </summary>
        /// <param name="name">The name of the PokeSprite to instantiate</param>
        /// <param name="loadImageAfterCreation">Wether the image data should be loaded directly after the PokeSprite has been created or not</param>
        /// <returns>The created instance of the PokeSprite</returns>
        public static PokeSprite GetInstance(string name, bool loadImageAfterCreation = false)
        {
            return GetInstance(IDs[name], loadImageAfterCreation);
        }
        /// <summary>
        /// Creates an instance of a PokeSprite with the given ID
        /// </summary>
        /// <param name="id">The id of the PokeSprite to instantiate</param>
        /// <param name="loadImageAfterCreation">Wether the image data should be loaded directly after the PokeSprite has been created or not</param>
        /// <returns>The created instance of the PokeSprite</returns>
        public static PokeSprite GetInstance(int id, bool loadImageAfterCreation = false)
        {
            if (CachedSprites.ContainsKey(id))
                return CachedSprites[id];

            PokeSprite p = new PokeSprite(loadImageAfterCreation);
            p = (PokeSprite)PokeApiType.Create(DataFetcher.GetSprite(id), p);

            if (ShouldCacheData)
                CachedSprites.Add(id, p);

            return p;
        }
    }
    /// <summary>
    /// [API DATABASE NOT FINISHED]
    /// Represents an evolution of a Pokemon
    /// Not an API class
    /// </summary>
    [Obsolete] // database still WIP
    public class PokeEvolution : PokeApiType
    {
        object methodPrecision;
        string method, to;

        //internal JsonData parent;

        /// <summary>
        /// When or how the Pokemon evolves with the Method, eg. the level needed.
        /// </summary>
        public object MethodPrecision
        {
            get
            {
                return methodPrecision;
            }
        }
        /// <summary>
        /// How the Pokemon evolves
        /// </summary>
        public string Method
        {
            get
            {
                return method;
            }
        }
        /// <summary>
        /// The name of the Pokemon it will evolve to
        /// </summary>
        public string EvolveTo
        {
            get
            {
                return to;
            }
        }

        /// <summary>
        /// The Pokemon it evolves to, as a Pokemon
        /// </summary>
        public Pokemon ToPokemon
        {
            get
            {
                return Pokemon.GetInstance(to);
            }
        }

        protected override void Create(JsonData source)
        {
            if (source.Keys.Contains("level"))
                methodPrecision = (int)source["level"];

            method = source["method"].ToString();
            uri = new Uri("http://www.pokeapi.co/" + source["resource_uri"]);
            to = source["to"].ToString();

            name = source["name"].ToString();
            created = ParseDateString(source["created"].ToString());
            modified = ParseDateString(source["modified"].ToString());
        }

        protected override bool OverrideDefaultParsing()
        {
            return true;
        }
    }
}
