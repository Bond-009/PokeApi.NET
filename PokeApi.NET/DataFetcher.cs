using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LitJson;

namespace PokeAPI
{
    // TODO:
    //   * docs
    //   * utility stuff from v1?

    /// <summary>
    /// Retrieves JSON data from the http://pokeapi.co/ site.
    /// </summary>
    public static class DataFetcher
    {
        internal readonly static string
            SITE_URL = "http://pokeapi.co",
            BASE_URL = SITE_URL + "/api/v2/",
            SLASH    = "/";

        static bool shouldCache = true;

        internal static IHttpClientAdapter client = new HttpClientDefaultAdapter();

        static async Task<JsonData> GetJsonAsync(Uri    url) => JsonMapper.ToObject(await client.GetStringAsync(url.AbsoluteUri));
        static async Task<JsonData> GetJsonAsync(string obj) => JsonMapper.ToObject(await client.GetStringAsync(BASE_URL + obj ));

        #region static Dictionary<Type, string> UrlOfType = new Dictionary<Type, string> { [...] };
        static Dictionary<Type, string> UrlOfType = new Dictionary<Type, string>
        {
            { typeof(ContestEffect     ), "contest-effect"       },
            { typeof(SuperContestEffect), "super-contest-effect" },
            { typeof(Characteristic    ), "characteristic"       },

            { typeof(Berry        ), "berry"          },
            { typeof(BerryFirmness), "berry-firmness" },
            { typeof(BerryFlavor  ), "berry-flavor"   },

            { typeof(ContestType), "contest-type" },

            { typeof(EncounterMethod        ), "encounter-method"          },
            { typeof(EncounterCondition     ), "encounter-condition"       },
            { typeof(EncounterConditionValue), "encounter-condition-value" },

            { typeof(EvolutionChain  ), "evolution-chain"   },
            { typeof(EvolutionTrigger), "evolution-trigger" },

            { typeof(Generation  ), "generation"    },
            { typeof(Pokedex     ), "pokedex"       },
            { typeof(GameVersion ), "version"       },
            { typeof(VersionGroup), "version-group" },

            { typeof(Item           ), "item"              },
            { typeof(ItemAttribute  ), "item-attribute"    },
            { typeof(ItemCategory   ), "item-category"     },
            { typeof(ItemFlingEffect), "item-fling-effect" },
            { typeof(ItemPocket     ), "item-pocket"       },

            { typeof(Move           ), "move"              },
            { typeof(MoveAilment    ), "move-ailment"      },
            { typeof(MoveBattleStyle), "move-battle-style" },
            { typeof(MoveCategory   ), "move-category"     },
            { typeof(MoveDamageClass), "move-damage-class" },
            { typeof(MoveLearnMethod), "move-learn-method" },
            { typeof(MoveTarget     ), "move-target"       },

            { typeof(Location    ), "location"      },
            { typeof(LocationArea), "location-area" },
            { typeof(PalParkArea ), "pal-park-area" },
            { typeof(Region      ), "region"        },

            { typeof(Ability       ), "ability"         },
            { typeof(EggGroup      ), "egg-group"       },
            { typeof(Gender        ), "gender"          },
            { typeof(GrowthRate    ), "growth-rate"     },
            { typeof(Nature        ), "nature"          },
            { typeof(PokeathlonStat), "pokeathlon-stat" },
            { typeof(Pokemon       ), "pokemon"         },
            { typeof(PokemonColour ), "pokemon-color"   },
            { typeof(PokemonForm   ), "pokemon-form"    },
            { typeof(PokemonHabitat), "pokemon-habitat" },
            { typeof(PokemonShape  ), "pokemon-shape"   },
            { typeof(PokemonSpecies), "pokemon-species" },
            { typeof(Stat          ), "stat"            },
            { typeof(PokemonType   ), "type"            },

            { typeof(Language), "language" }
        };
        #endregion

        readonly static Dictionary<Type, Cache<int   , JsonData>>    caches = UrlOfType        .ToDictionary(kvp => kvp.Key, kvp => new Cache<int   , JsonData>(async i => Maybe.Just(await GetJsonAsync(kvp.Value + SLASH + i))));
        readonly static Dictionary<Type, Cache<string, JsonData>> strCaches = UrlOfType.Skip(3).ToDictionary(kvp => kvp.Key, kvp => new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(kvp.Value + SLASH + s))));

        readonly static Dictionary<Type, Cache<Uri   , JsonData>> urlCaches = UrlOfType        .ToDictionary(kvp => kvp.Key, kvp => new Cache<Uri   , JsonData>(async u => Maybe.Just(JsonMapper.ToObject(await client.GetStringAsync(u.AbsoluteUri)))));

        readonly static Dictionary<Type, Cache<ValueTuple<int, int>, JsonData>> listCaches = UrlOfType.ToDictionary(kvp => kvp.Key, kvp => new Cache<ValueTuple<int, int>, JsonData>(async t => Maybe.Just(await GetJsonAsync(kvp.Value + SLASH + "?offset=" + t.Item1 + "&limit=" + t.Item2))));

        readonly static Cache<Uri, JsonData> miscCache = new Cache<Uri, JsonData>(async u => Maybe.Just(await GetJsonAsync(u)));

        /// <summary>
        /// Sets the <see cref="IHttpClientAdapter" /> the data fetcher uses.
        /// </summary>
        /// <param name="client">The <see cref="IHttpClientAdapter" /> to use.</param>
        public static void SetHttpClient(IHttpClientAdapter client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            DataFetcher.client = client;
        }

        public static CacheGetter<int   , JsonData> ChacheOf     <T>() where T :      ApiObject => new CacheGetter<int   , JsonData>(   caches[typeof(T)]);
        public static CacheGetter<string, JsonData> CacheOfByName<T>() where T : NamedApiObject => new CacheGetter<string, JsonData>(strCaches[typeof(T)]);
        public static CacheGetter<Uri   , JsonData> CacheOfByUrl <T>() where T :      ApiObject => new CacheGetter<Uri   , JsonData>(urlCaches[typeof(T)]);

        public static CacheGetter<ValueTuple<int, int>, JsonData> ListCacheOf<T>() where T : ApiObject => new CacheGetter<ValueTuple<int, int>, JsonData>(listCaches[typeof(T)]);

        /// <summary>
        /// Gets or sets whether fetched data should be cached or not. Default is true.
        /// </summary>
        /// <remarks>Controls the <see cref="CacheGetter{TKey, TValue}.IsActive" /> value of all caches in the <see cref="DataFetcher" /> class. The value returned by the getter is the value passed to the last set call.</remarks>
        public static bool ShouldCacheData
        {
            get
            {
                return shouldCache;
            }
            set
            {
                if (value == shouldCache)
                    return;

                foreach (var c in     caches.Values) c.IsActive = value;
                foreach (var c in  strCaches.Values) c.IsActive = value;
                foreach (var c in listCaches.Values) c.IsActive = value;
                miscCache.IsActive = value;

                shouldCache = value;
            }
        }

        public static Task<JsonData> GetJsonOf<T>(int    id  ) where T :      ApiObject =>    caches[typeof(T)].Get(id  );
        public static Task<JsonData> GetJsonOf<T>(string name) where T : NamedApiObject => strCaches[typeof(T)].Get(name);
        public static Task<JsonData> GetJsonOf<T>(Uri    url ) where T :      ApiObject => urlCaches[typeof(T)].Get(url );
        public static Task<JsonData> GetJsonOfAny(Uri    url )                          => miscCache           .Get(url );

        public static Task<JsonData> GetListJsonOf<T>(int offset, int limit) where T : ApiObject => listCaches[typeof(T)].Get(ValueTuple.Create(offset, limit));

        public static void ClearAll()
        {
                caches.Clear();
             strCaches.Clear();
             urlCaches.Clear();
            listCaches.Clear();
            miscCache .Clear();
        }

        public static async Task<T> GetApiObject     <T>(int    id  ) where T :      ApiObject => JsonMapper.ToObject<T>(await GetJsonOf<T>(id  ));
        public static async Task<T> GetApiObject     <T>(Uri    url ) where T :      ApiObject => JsonMapper.ToObject<T>(await GetJsonOf<T>(url ));
        public static async Task<T> GetNamedApiObject<T>(string name) where T : NamedApiObject => JsonMapper.ToObject<T>(await GetJsonOf<T>(name));
        public static async Task<T> GetNamedApiObject<T>(Uri    url ) where T : NamedApiObject => JsonMapper.ToObject<T>(await GetJsonOf<T>(url ));

        public static async Task<T> GetAny<T>(Uri url)
        {
            JsonData j = await GetJsonOfAny(url);
            return JsonMapper.ToObject<T>(j);
        }

        public static async Task<ResourceList<T, TInner>> GetResourceList<T, TInner>(int limit = 20)
            where TInner : ApiObject
            where T : ApiResource<TInner>
        {
            var f = JsonMapper.ToObject<ResourceListFragment<T, TInner>>(await GetListJsonOf<TInner>(0, limit));

            return new ResourceList<T, TInner>(f.Count, limit, f);
        }
    }
}
