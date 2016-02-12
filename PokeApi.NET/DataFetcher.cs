using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LitJson;

namespace PokeAPI
{
    /// <summary>
    /// Retrieves JSON data from the http://pokeapi.co/ site.
    /// </summary>
    public static class DataFetcher
    {
        readonly static string
            BASE_URL = "http://pokeapi.co/api/v2/",

        #region url fragment strings
            BERRY          = "berry/"         ,
            BERRY_FIRMNESS = "berry-firmness/",
            BERRY_FLAVOR   = "berry-flavor/"  ,

            CONTEST_TYPE         = "contest-type/"        ,
            CONTEST_EFFECT       = "contest-effect/"      ,
            SUPER_CONTEST_EFFECT = "super-contest-effect/",

            ENCOUNTER_METHOD          = "encounter-method/"         ,
            ENCOUNTER_CONDITION       = "encounter-condition/"      ,
            ENCOUNTER_CONDITION_VALUE = "encounter-condition-value/",

            EVOLUTION_CHAIN   = "evolution-chain/"  ,
            EVOLUTION_TRIGGER = "evolution-trigger/",

            GENERATION    = "generation/"   ,
            POKEDEX       = "pokedex/"      ,
            VERSION       = "version/"      ,
            VERSION_GROUP = "version-group/",

            ITEM              = "item/"             ,
            ITEM_ATTRIBUTE    = "item-attribute/"   ,
            ITEM_CATEGORY     = "item-category/"    ,
            ITEM_FLING_EFFECT = "item-fling-effect/",
            ITEM_POCKET       = "item-pocket/"      ,

            MOVE              = "move/"             ,
            MOVE_AILMENT      = "move-ailment/"     ,
            MOVE_BATTLE_STYLE = "move-battle-style/",
            MOVE_CATEGORY     = "move-category/"    ,
            MOVE_DAMAGE_CLASS = "move-damage-class/",
            MOVE_LEARN_METHOD = "move-learn-method/",
            MOVE_TARGET       = "move-target/"      ,

            LOCATION      = "location/"     ,
            LOCATION_AREA = "location-area/",
            PAL_PARK_AREA = "pal-park-area/",
            REGION        = "region/"       ,

            ABILITY         = "ability/"        ,
            CHARACTERISTIC  = "characteristic/" ,
            EGG_GROUP       = "egg-group/"      ,
            GENDER          = "gender/"         ,
            GROWTH_RATE     = "growth-rate/"    ,
            NATURE          = "nature/"         ,
            POKEATHLON_STAT = "pokeathlon-stat/",
            POKEMON         = "pokemon/"        ,
            POKEMON_COLOUR  = "pokemon-color/"  ,
            POKEMON_FORM    = "pokemon-form/"   ,
            POKEMON_HABITAT = "pokemon-habitat/",
            POKEMON_SHAPE   = "pokemon-shape/"  ,
            POKEMON_SPECIES = "pokemon-species/",
            STAT            = "stat/"           ,
            TYPE            = "type/"           ,

            LANGUAGE = "language/";
        #endregion

        static bool shouldCache = true;

        internal static IHttpClientAdapter client = new HttpClientDefaultAdapter();

        static async Task<JsonData> GetJsonAsync(string obj) => JsonMapper.ToObject(await client.GetStringAsync(BASE_URL + obj));

        #region caches
        readonly static Cache<int, JsonData>
             coneff  = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(      CONTEST_EFFECT + i))),
            sconeff  = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(SUPER_CONTEST_EFFECT + i))),
            evochain = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(EVOLUTION_CHAIN      + i))),
            charact  = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(CHARACTERISTIC       + i))),

            i_berry     = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(BERRY          + i))),
            i_berryfirm = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(BERRY_FIRMNESS + i))),
            i_berryflav = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(BERRY_FLAVOR   + i))),

            i_contype = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(CONTEST_TYPE + i))),

            i_encmtd   = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(ENCOUNTER_METHOD          + i))),
            i_enccond  = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(ENCOUNTER_CONDITION       + i))),
            i_enccondv = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(ENCOUNTER_CONDITION_VALUE + i))),

            i_evotrig = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(EVOLUTION_TRIGGER + i))),

            i_gen    = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(GENERATION    + i))),
            i_dex    = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(POKEDEX       + i))),
            i_ver    = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(VERSION       + i))),
            i_vergrp = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(VERSION_GROUP + i))),

            i_item     = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(ITEM              + i))),
            i_itemattr = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(ITEM_ATTRIBUTE    + i))),
            i_itemcat  = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(ITEM_CATEGORY     + i))),
            i_itemfeff = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(ITEM_FLING_EFFECT + i))),
            i_itemp    = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(ITEM_POCKET       + i))),

            i_move     = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(MOVE              + i))),
            i_moveail  = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(MOVE_AILMENT      + i))),
            i_movebst  = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(MOVE_BATTLE_STYLE + i))),
            i_movecat  = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(MOVE_CATEGORY     + i))),
            i_movedmgc = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(MOVE_DAMAGE_CLASS + i))),
            i_movelmtd = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(MOVE_LEARN_METHOD + i))),
            i_movetar  = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(MOVE_TARGET       + i))),

            i_loc    = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(LOCATION      + i))),
            i_loca   = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(LOCATION_AREA + i))),
            i_pparka = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(PAL_PARK_AREA + i))),
            i_reg    = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(REGION        + i))),

            i_ability = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(ABILITY         + i))),
            i_egggr   = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(EGG_GROUP       + i))),
            i_gender  = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(GENDER          + i))),
            i_growr   = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(GROWTH_RATE     + i))),
            i_nat     = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(NATURE          + i))),
            i_pathls  = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(POKEATHLON_STAT + i))),
            i_pokemon = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(POKEMON         + i))),
            i_pcol    = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(POKEMON_COLOUR  + i))),
            i_pform   = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(POKEMON_FORM    + i))),
            i_phabit  = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(POKEMON_HABITAT + i))),
            i_pshape  = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(POKEMON_SHAPE   + i))),
            i_pspec   = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(POKEMON_SPECIES + i))),
            i_stat    = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(STAT            + i))),
            i_type    = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(TYPE            + i))),

            i_lang = new Cache<int, JsonData>(async i => Maybe.Just(await GetJsonAsync(LANGUAGE + i)));

        readonly static Cache<string, JsonData>
            s_berry     = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(BERRY          + s))),
            s_berryfirm = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(BERRY_FIRMNESS + s))),
            s_berryflav = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(BERRY_FLAVOR   + s))),

            s_contype = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(CONTEST_TYPE + s))),

            s_encmtd   = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(ENCOUNTER_METHOD          + s))),
            s_enccond  = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(ENCOUNTER_CONDITION       + s))),
            s_enccondv = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(ENCOUNTER_CONDITION_VALUE + s))),

            s_evotrig = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(EVOLUTION_TRIGGER + s))),

            s_gen    = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(GENERATION    + s))),
            s_dex    = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(POKEDEX       + s))),
            s_ver    = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(VERSION       + s))),
            s_vergrp = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(VERSION_GROUP + s))),

            s_item     = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(ITEM              + s))),
            s_itemattr = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(ITEM_ATTRIBUTE    + s))),
            s_itemcat  = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(ITEM_CATEGORY     + s))),
            s_itemfeff = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(ITEM_FLING_EFFECT + s))),
            s_itemp    = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(ITEM_POCKET       + s))),

            s_move     = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(MOVE              + s))),
            s_moveail  = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(MOVE_AILMENT      + s))),
            s_movebst  = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(MOVE_BATTLE_STYLE + s))),
            s_movecat  = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(MOVE_CATEGORY     + s))),
            s_movedmgc = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(MOVE_DAMAGE_CLASS + s))),
            s_movelmtd = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(MOVE_LEARN_METHOD + s))),
            s_movetar  = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(MOVE_TARGET       + s))),

            s_loc    = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(LOCATION      + s))),
            s_loca   = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(LOCATION_AREA + s))),
            s_pparka = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(PAL_PARK_AREA + s))),
            s_reg    = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(REGION        + s))),

            s_ability = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(ABILITY         + s))),
            s_egggr   = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(EGG_GROUP       + s))),
            s_gender  = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(GENDER          + s))),
            s_growr   = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(GROWTH_RATE     + s))),
            s_nat     = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(NATURE          + s))),
            s_pathls  = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(POKEATHLON_STAT + s))),
            s_pokemon = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(POKEMON         + s))),
            s_pcol    = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(POKEMON_COLOUR  + s))),
            s_pform   = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(POKEMON_FORM    + s))),
            s_phabit  = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(POKEMON_HABITAT + s))),
            s_pshape  = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(POKEMON_SHAPE   + s))),
            s_pspec   = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(POKEMON_SPECIES + s))),
            s_stat    = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(STAT            + s))),
            s_type    = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(TYPE            + s))),

            s_lang = new Cache<string, JsonData>(async s => Maybe.Just(await GetJsonAsync(LANGUAGE + s)));
        #endregion

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

        #region chache getters
        public readonly static CacheGetter<int, JsonData>
            ContestEffect      = new CacheGetter<int, JsonData>(coneff  ),
            SuperContestEffect = new CacheGetter<int, JsonData>(sconeff ),
            EvolutionChain     = new CacheGetter<int, JsonData>(evochain),
            Characteristic     = new CacheGetter<int, JsonData>(charact ),

            Berry         = new CacheGetter<int, JsonData>(i_berry    ),
            BerryFirmness = new CacheGetter<int, JsonData>(i_berryfirm),
            BerryFlavor   = new CacheGetter<int, JsonData>(i_berryflav),

            ContestType = new CacheGetter<int, JsonData>(i_contype),

            EncounterMethod         = new CacheGetter<int, JsonData>(i_encmtd  ),
            EncounterCondition      = new CacheGetter<int, JsonData>(i_enccond ),
            EncounterConditionValue = new CacheGetter<int, JsonData>(i_enccondv),

            EvolutionTrigger = new CacheGetter<int, JsonData>(i_evotrig),

            Generation   = new CacheGetter<int, JsonData>(i_gen   ),
            Pokedex      = new CacheGetter<int, JsonData>(i_dex   ),
            Version      = new CacheGetter<int, JsonData>(i_ver   ),
            VersionGroup = new CacheGetter<int, JsonData>(i_vergrp),

            Item            = new CacheGetter<int, JsonData>(i_item    ),
            ItemAttribute   = new CacheGetter<int, JsonData>(i_itemattr),
            ItemCategory    = new CacheGetter<int, JsonData>(i_itemcat ),
            ItemFlingEffect = new CacheGetter<int, JsonData>(i_itemfeff),
            ItemPocket      = new CacheGetter<int, JsonData>(i_itemp   ),

            Move            = new CacheGetter<int, JsonData>(i_move    ),
            MoveAilment     = new CacheGetter<int, JsonData>(i_moveail ),
            MoveBattleStyle = new CacheGetter<int, JsonData>(i_movebst ),
            MoveCategory    = new CacheGetter<int, JsonData>(i_movecat ),
            MoveDamageClass = new CacheGetter<int, JsonData>(i_movedmgc),
            MoveLearnMethod = new CacheGetter<int, JsonData>(i_movelmtd),
            MoveTarget      = new CacheGetter<int, JsonData>(i_movetar ),

            Location     = new CacheGetter<int, JsonData>(i_loc   ),
            LocationArea = new CacheGetter<int, JsonData>(i_loca  ),
            PalParkArea  = new CacheGetter<int, JsonData>(i_pparka),
            Region       = new CacheGetter<int, JsonData>(i_reg   ),

            Ability        = new CacheGetter<int, JsonData>(i_ability),
            EggGroup       = new CacheGetter<int, JsonData>(i_egggr  ),
            Gender         = new CacheGetter<int, JsonData>(i_gender ),
            GrowthRate     = new CacheGetter<int, JsonData>(i_growr  ),
            Nature         = new CacheGetter<int, JsonData>(i_nat    ),
            PokeathlonStat = new CacheGetter<int, JsonData>(i_pathls ),
            Pokemon        = new CacheGetter<int, JsonData>(i_pokemon),
            PokemonColour  = new CacheGetter<int, JsonData>(i_pcol   ),
            PokemonForm    = new CacheGetter<int, JsonData>(i_pform  ),
            PokemonHabitat = new CacheGetter<int, JsonData>(i_phabit ),
            PokemonShape   = new CacheGetter<int, JsonData>(i_pshape ),
            PokemonSpecies = new CacheGetter<int, JsonData>(i_pspec  ),
            Stat           = new CacheGetter<int, JsonData>(i_stat   ),
            Type           = new CacheGetter<int, JsonData>(i_type   ),

            Language = new CacheGetter<int, JsonData>(i_lang);
        public readonly static CacheGetter<string, JsonData>
            BerryByName         = new CacheGetter<string, JsonData>(s_berry    ),
            BerryFirmnessByName = new CacheGetter<string, JsonData>(s_berryfirm),
            BerryFlavorByName   = new CacheGetter<string, JsonData>(s_berryflav),

            ContestTypeByName = new CacheGetter<string, JsonData>(s_contype),

            EncounterMethodByName         = new CacheGetter<string, JsonData>(s_encmtd  ),
            EncounterConditionByName      = new CacheGetter<string, JsonData>(s_enccond ),
            EncounterConditionValueByName = new CacheGetter<string, JsonData>(s_enccondv),

            EvolutionTriggerByName = new CacheGetter<string, JsonData>(s_evotrig),

            GenerationByName   = new CacheGetter<string, JsonData>(s_gen   ),
            PokedexByName      = new CacheGetter<string, JsonData>(s_dex   ),
            VersionByName      = new CacheGetter<string, JsonData>(s_ver   ),
            VersionGroupByName = new CacheGetter<string, JsonData>(s_vergrp),

            ItemByName            = new CacheGetter<string, JsonData>(s_item    ),
            ItemAttributeByName   = new CacheGetter<string, JsonData>(s_itemattr),
            ItemCategoryByName    = new CacheGetter<string, JsonData>(s_itemcat ),
            ItemFlingEffectByName = new CacheGetter<string, JsonData>(s_itemfeff),
            ItemPocketByName      = new CacheGetter<string, JsonData>(s_itemp   ),

            MoveByName            = new CacheGetter<string, JsonData>(s_move    ),
            MoveAilmentByName     = new CacheGetter<string, JsonData>(s_moveail ),
            MoveBattleStyleByName = new CacheGetter<string, JsonData>(s_movebst ),
            MoveCategoryByName    = new CacheGetter<string, JsonData>(s_movecat ),
            MoveDamageClassByName = new CacheGetter<string, JsonData>(s_movedmgc),
            MoveLearnMethodByName = new CacheGetter<string, JsonData>(s_movelmtd),
            MoveTargetByName      = new CacheGetter<string, JsonData>(s_movetar ),

            LocationByName     = new CacheGetter<string, JsonData>(s_loc   ),
            LocationAreaByName = new CacheGetter<string, JsonData>(s_loca  ),
            PalParkAreaByName  = new CacheGetter<string, JsonData>(s_pparka),
            RegionByName       = new CacheGetter<string, JsonData>(s_reg   ),

            AbilityByName        = new CacheGetter<string, JsonData>(s_ability),
            EggGroupByName       = new CacheGetter<string, JsonData>(s_egggr  ),
            GenderByName         = new CacheGetter<string, JsonData>(s_gender ),
            GrowthRateByName     = new CacheGetter<string, JsonData>(s_growr  ),
            NatureByName         = new CacheGetter<string, JsonData>(s_nat    ),
            PokeathlonStatByName = new CacheGetter<string, JsonData>(s_pathls ),
            PokemonByName        = new CacheGetter<string, JsonData>(s_pokemon),
            PokemonColourByName  = new CacheGetter<string, JsonData>(s_pcol   ),
            PokemonFormByName    = new CacheGetter<string, JsonData>(s_pform  ),
            PokemonHabitatByName = new CacheGetter<string, JsonData>(s_phabit ),
            PokemonShapeByName   = new CacheGetter<string, JsonData>(s_pshape ),
            PokemonSpeciesByName = new CacheGetter<string, JsonData>(s_pspec  ),
            StatByName           = new CacheGetter<string, JsonData>(s_stat   ),
            TypeByName           = new CacheGetter<string, JsonData>(s_type   ),

            LanguageByName = new CacheGetter<string, JsonData>(s_lang);
        #endregion

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
                coneff.IsActive = sconeff.IsActive = evochain.IsActive = charact.IsActive
                    = i_berry.IsActive = i_berryfirm.IsActive = i_berryflav.IsActive
                    = i_encmtd.IsActive = i_enccond.IsActive = i_enccondv.IsActive
                    = i_gen.IsActive = i_dex.IsActive = i_ver.IsActive = i_vergrp.IsActive
                    = i_item.IsActive = i_itemattr.IsActive = i_itemcat.IsActive = i_itemfeff.IsActive = i_itemp.IsActive
                    = i_move.IsActive = i_moveail.IsActive = i_movebst.IsActive = i_movecat.IsActive = i_movedmgc.IsActive = i_movelmtd.IsActive = i_movetar.IsActive
                    = i_loc.IsActive = i_loca.IsActive = i_pparka.IsActive = i_reg.IsActive
                    = i_ability.IsActive = i_egggr.IsActive = i_gender.IsActive = i_growr.IsActive = i_nat.IsActive = i_pathls.IsActive = i_pokemon.IsActive
                        = i_pcol.IsActive = i_pform.IsActive = i_phabit.IsActive = i_pshape.IsActive = i_pspec.IsActive = i_stat.IsActive = i_type.IsActive
                    = i_contype.IsActive = i_evotrig.IsActive = i_lang.IsActive
                    = shouldCache = value;
            }
        }

        #region get*
        public static async Task<JsonData> GetContestEffect     (int id) => await  coneff .Get(id);
        public static async Task<JsonData> GetSuperContestEffect(int id) => await sconeff .Get(id);
        public static async Task<JsonData> GetEvolutionChain    (int id) => await evochain.Get(id);
        public static async Task<JsonData> GetCharacteristic    (int id) => await charact .Get(id);

        public static async Task<JsonData> GetBerry        (int id) => await i_berry    .Get(id);
        public static async Task<JsonData> GetBerryFirmness(int id) => await i_berryfirm.Get(id);
        public static async Task<JsonData> GetBerryFlavor  (int id) => await i_berryflav.Get(id);

        public static async Task<JsonData> GetContestType(int id) => await i_contype.Get(id);

        public static async Task<JsonData> GetEncounterMethod        (int id) => await i_encmtd  .Get(id);
        public static async Task<JsonData> GetEncounterCondition     (int id) => await i_enccond .Get(id);
        public static async Task<JsonData> GetEncounterConditionValue(int id) => await i_enccondv.Get(id);

        public static async Task<JsonData> GetEvolutionTrigger(int id) => await i_evotrig.Get(id);

        public static async Task<JsonData> GetGeneration  (int id) => await i_gen   .Get(id);
        public static async Task<JsonData> GetPokedex     (int id) => await i_dex   .Get(id);
        public static async Task<JsonData> GetVersion     (int id) => await i_ver   .Get(id);
        public static async Task<JsonData> GetVersionGroup(int id) => await i_vergrp.Get(id);

        public static async Task<JsonData> GetItem           (int id) => await i_item    .Get(id);
        public static async Task<JsonData> GetItemAttribute  (int id) => await i_itemattr.Get(id);
        public static async Task<JsonData> GetItemCategory   (int id) => await i_itemcat .Get(id);
        public static async Task<JsonData> GetItemFlingEffect(int id) => await i_itemfeff.Get(id);
        public static async Task<JsonData> GetItemPocket     (int id) => await i_itemp   .Get(id);

        public static async Task<JsonData> GetMove           (int id) => await i_move    .Get(id);
        public static async Task<JsonData> GetMoveAilment    (int id) => await i_moveail .Get(id);
        public static async Task<JsonData> GetMoveBattleStyle(int id) => await i_movebst .Get(id);
        public static async Task<JsonData> GetMoveCategory   (int id) => await i_movecat .Get(id);
        public static async Task<JsonData> GetMoveDamageClass(int id) => await i_movedmgc.Get(id);
        public static async Task<JsonData> GetMoveLearnMethod(int id) => await i_movelmtd.Get(id);
        public static async Task<JsonData> GetMoveTarget     (int id) => await i_movetar .Get(id);

        public static async Task<JsonData> GetLocation    (int id) => await i_loc   .Get(id);
        public static async Task<JsonData> GetLocationArea(int id) => await i_loca  .Get(id);
        public static async Task<JsonData> GetPalParkAre  (int id) => await i_pparka.Get(id);
        public static async Task<JsonData> GetRegion      (int id) => await i_reg   .Get(id);

        public static async Task<JsonData> GetAbility       (int id) => await i_ability.Get(id);
        public static async Task<JsonData> GetEggGroup      (int id) => await i_egggr  .Get(id);
        public static async Task<JsonData> GetGender        (int id) => await i_gender .Get(id);
        public static async Task<JsonData> GetGrowthRate    (int id) => await i_growr  .Get(id);
        public static async Task<JsonData> GetNature        (int id) => await i_nat    .Get(id);
        public static async Task<JsonData> GetPokeathlonStat(int id) => await i_pathls .Get(id);
        public static async Task<JsonData> GetPokemon       (int id) => await i_pokemon.Get(id);
        public static async Task<JsonData> GetPokemonColour (int id) => await i_pcol   .Get(id);
        public static async Task<JsonData> GetPokemonForm   (int id) => await i_pform  .Get(id);
        public static async Task<JsonData> GetPokemonHabitat(int id) => await i_phabit .Get(id);
        public static async Task<JsonData> GetPokemonShape  (int id) => await i_pshape .Get(id);
        public static async Task<JsonData> GetPokemonSpecies(int id) => await i_pspec  .Get(id);
        public static async Task<JsonData> GetStat          (int id) => await i_stat   .Get(id);
        public static async Task<JsonData> GetType          (int id) => await i_type   .Get(id);

        public static async Task<JsonData> GetLanguage(int id) => await i_lang.Get(id);

        // ---

        public static async Task<JsonData> GetBerry        (string name) => await s_berry    .Get(name);
        public static async Task<JsonData> GetBerryFirmness(string name) => await s_berryfirm.Get(name);
        public static async Task<JsonData> GetBerryFlavor  (string name) => await s_berryflav.Get(name);

        public static async Task<JsonData> GetContestType(string name) => await s_contype.Get(name);

        public static async Task<JsonData> GetEncounterMethod        (string name) => await s_encmtd  .Get(name);
        public static async Task<JsonData> GetEncounterCondition     (string name) => await s_enccond .Get(name);
        public static async Task<JsonData> GetEncounterConditionValue(string name) => await s_enccondv.Get(name);

        public static async Task<JsonData> GetEvolutionTrigger(string name) => await s_evotrig.Get(name);

        public static async Task<JsonData> GetGeneration  (string name) => await s_gen   .Get(name);
        public static async Task<JsonData> GetPokedex     (string name) => await s_dex   .Get(name);
        public static async Task<JsonData> GetVersion     (string name) => await s_ver   .Get(name);
        public static async Task<JsonData> GetVersionGroup(string name) => await s_vergrp.Get(name);

        public static async Task<JsonData> GetItem           (string name) => await s_item    .Get(name);
        public static async Task<JsonData> GetItemAttribute  (string name) => await s_itemattr.Get(name);
        public static async Task<JsonData> GetItemCategory   (string name) => await s_itemcat .Get(name);
        public static async Task<JsonData> GetItemFlingEffect(string name) => await s_itemfeff.Get(name);
        public static async Task<JsonData> GetItemPocket     (string name) => await s_itemp   .Get(name);

        public static async Task<JsonData> GetMove           (string name) => await s_move    .Get(name);
        public static async Task<JsonData> GetMoveAilment    (string name) => await s_moveail .Get(name);
        public static async Task<JsonData> GetMoveBattleStyle(string name) => await s_movebst .Get(name);
        public static async Task<JsonData> GetMoveCategory   (string name) => await s_movecat .Get(name);
        public static async Task<JsonData> GetMoveDamageClass(string name) => await s_movedmgc.Get(name);
        public static async Task<JsonData> GetMoveLearnMethod(string name) => await s_movelmtd.Get(name);
        public static async Task<JsonData> GetMoveTarget     (string name) => await s_movetar .Get(name);

        public static async Task<JsonData> GetLocation    (string name) => await s_loc   .Get(name);
        public static async Task<JsonData> GetLocationArea(string name) => await s_loca  .Get(name);
        public static async Task<JsonData> GetPalParkAre  (string name) => await s_pparka.Get(name);
        public static async Task<JsonData> GetRegion      (string name) => await s_reg   .Get(name);

        public static async Task<JsonData> GetAbility       (string name) => await s_ability.Get(name);
        public static async Task<JsonData> GetEggGroup      (string name) => await s_egggr  .Get(name);
        public static async Task<JsonData> GetGender        (string name) => await s_gender .Get(name);
        public static async Task<JsonData> GetGrowthRate    (string name) => await s_growr  .Get(name);
        public static async Task<JsonData> GetNature        (string name) => await s_nat    .Get(name);
        public static async Task<JsonData> GetPokeathlonStat(string name) => await s_pathls .Get(name);
        public static async Task<JsonData> GetPokemon       (string name) => await s_pokemon.Get(name);
        public static async Task<JsonData> GetPokemonColour (string name) => await s_pcol   .Get(name);
        public static async Task<JsonData> GetPokemonForm   (string name) => await s_pform  .Get(name);
        public static async Task<JsonData> GetPokemonHabitat(string name) => await s_phabit .Get(name);
        public static async Task<JsonData> GetPokemonShape  (string name) => await s_pshape .Get(name);
        public static async Task<JsonData> GetPokemonSpecies(string name) => await s_pspec  .Get(name);
        public static async Task<JsonData> GetStat          (string name) => await s_stat   .Get(name);
        public static async Task<JsonData> GetType          (string name) => await s_type   .Get(name);

        public static async Task<JsonData> GetLanguage(string name) => await s_lang.Get(name);
        #endregion
    }
}
