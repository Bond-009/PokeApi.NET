using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LitJson;

namespace PokeAPI
{
    /// <summary>
    /// Gets JSON data from the PokeApi.co site
    /// </summary>
    public static class DataFetcher
    {
        const int
            AMT_PKMN =  719,
            AMT_TYPE =   19,
            AMT_MOVE =  626,
            AMT_ABTY =  249,
            AMT_EGGG =   16,
            AMT_DESC = 6611,
            AMT_SPRT =  720,
            AMT_GAME =   26;

        readonly static string
            BASE_URL = "http://pokeapi.co/api/v1/",
            F_SLASH  = "/",

            POKEDEX  = "pokedex/1"   ,
            POKEMON  = "pokemon/"    ,
            TYPE     = "type/"       ,
            MOVE     = "move/"       ,
            ABILITY  = "ability/"    ,
            EGGGROUP = "egg/"        ,
            DESCR    = "description/",
            SPRITE   = "sprite/"     ,
            GAME     = "game/"       ;

        internal static HttpClient client = new HttpClient();

        static Cache<JsonData> dex = new Cache<JsonData>(async () => Maybe.Just(await GetJsonAsync(POKEDEX)));
        static Cache<int, JsonData>
            pkmn    = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(POKEMON  + i))),
            type    = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(TYPE     + i))),
            move    = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(MOVE     + i))),
            ability = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(ABILITY  + i))),
            eggG    = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(EGGGROUP + i))),
            desc    = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(DESCR    + i))),
            sprite  = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(SPRITE   + i))),
            game    = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(GAME     + i)));

        /// <summary>
        /// The cached Pokedex.
        /// </summary>
        public static JsonData Pokedex => dex.TryGetDef();

        public static CacheGetter Pokemon     { get; } = new CacheGetter(pkmn   );
        public static CacheGetter Type        { get; } = new CacheGetter(type   );
        public static CacheGetter Move        { get; } = new CacheGetter(move   );
        public static CacheGetter Ability     { get; } = new CacheGetter(ability);
        public static CacheGetter EggGroup    { get; } = new CacheGetter(eggG   );
        public static CacheGetter Description { get; } = new CacheGetter(desc   );
        public static CacheGetter Sprite      { get; } = new CacheGetter(sprite );
        public static CacheGetter Game        { get; } = new CacheGetter(game   );

        /// <summary>
        /// Specifies whether fetched data should be cached or not. Default is true.
        /// </summary>
        public static bool ShouldCacheData
        {
            get
            {
                return dex.IsActive;
            }
            set
            {
                dex.IsActive = pkmn.IsActive = type.IsActive = move.IsActive = ability.IsActive = eggG.IsActive = desc.IsActive = sprite.IsActive = game.IsActive
                    = value;
            }
        }

        static async Task<JsonData> GetJsonAsync(string obj) => JsonMapper.ToObject(await client.GetStringAsync(BASE_URL + obj));

        public static async Task<JsonData> GetPokedex() => await dex.Get();

        public static async Task<JsonData> GetPokemon(int id) => await pkmn.Get(id);
        public static async void CacheAllPokemon()
        {
            Task t = Task.Factory.StartNew(() => pkmn.Get(1));

            for (int i = 2; i < AMT_PKMN; i++)
                t = t.ContinueWith(_ => pkmn.Get(i));

            await t;
        }

        public static async Task<JsonData> GetType(int id) => await type.Get(id);
        public static async void CacheAllTypes()
        {
            Task t = Task.Factory.StartNew(() => type.Get(1));

            for (int i = 2; i < AMT_TYPE; i++)
                t = t.ContinueWith(_ => type.Get(i));

            await t;
        }

        public static async Task<JsonData> GetMove(int id) => await move.Get(id);
        public static async void CacheAllMoves()
        {
            Task t = Task.Factory.StartNew(() => move.Get(1));

            for (int i = 2; i < AMT_MOVE; i++)
                t = t.ContinueWith(_ => move.Get(i));

            await t;
        }

        public static async Task<JsonData> GetAbility(int id) => await ability.Get(id);
        public static async void CacheAllAbilities()
        {
            Task t = Task.Factory.StartNew(() => ability.Get(1));

            for (int i = 2; i < AMT_ABTY; i++)
                t = t.ContinueWith(_ => ability.Get(i));

            await t;
        }

        public static async Task<JsonData> GetEggGroup(int id) => await eggG.Get(id);
        public static async void CacheAllEggGroups()
        {
            Task t = Task.Factory.StartNew(() => eggG.Get(1));

            for (int i = 2; i < AMT_EGGG; i++)
                t = t.ContinueWith(_ => eggG.Get(i));

            await t;
        }

        public static async Task<JsonData> GetDescription(int id) => await desc.Get(id);
        public static async void CacheAllDescriptions()
        {
            Task t = Task.Factory.StartNew(() => desc.Get(1));

            for (int i = 2; i < AMT_DESC; i++)
                t = t.ContinueWith(_ => desc.Get(i));

            await t;
        }

        public static async Task<JsonData> GetSprite(int id) => await sprite.Get(id);
        public static async void CacheAllSprites()
        {
            Task t = Task.Factory.StartNew(() => sprite.Get(1));

            for (int i = 2; i < AMT_SPRT; i++)
                t = t.ContinueWith(_ => sprite.Get(i));

            await t;
        }

        public static async Task<JsonData> GetGame(int id) => await game.Get(id);
        public static async void CacheAllGames()
        {
            Task t = Task.Factory.StartNew(() => game.Get(1));

            for (int i = 2; i < AMT_GAME; i++)
                t = t.ContinueWith(_ => game.Get(i));

            await t;
        }
    }
}
