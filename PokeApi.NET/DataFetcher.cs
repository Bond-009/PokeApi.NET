using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using LitJson;

namespace PokeAPI.NET
{
    public static class DataFetcher
    {
        #region Fields
        static WebClient client = new WebClient();

        /// <summary>
        /// The Pokédex is cached in this variable if ShouldCacheData equals true.
        /// </summary>
        public static JsonData Pokédex = null;
        /// <summary>
        /// All Pokémon data is cached in this variable if ShouldCacheData equals true.
        /// </summary>
        public static Dictionary<int, JsonData> PokemonData = new Dictionary<int, JsonData>();
        /// <summary>
        /// All Type data is cached in this variable if ShouldCacheData equals true.
        /// </summary>
        public static Dictionary<int, JsonData> TypeData = new Dictionary<int, JsonData>();
        /// <summary>
        /// All Move data is cached in this variable if ShouldCacheData equals true.
        /// </summary>
        public static Dictionary<int, JsonData> MoveData = new Dictionary<int, JsonData>();
        /// <summary>
        /// All Ability data is cached in this variable if ShouldCacheData equals true.
        /// </summary>
        public static Dictionary<int, JsonData> AbilityData = new Dictionary<int, JsonData>();
        /// <summary>
        /// All Egg groups data is cached in this variable if ShouldCacheData equals true.
        /// </summary>
        public static Dictionary<int, JsonData> EggGroupData = new Dictionary<int, JsonData>();
        /// <summary>
        /// All Description data is cached in this variable if ShouldCacheData equals true.
        /// </summary>
        [Obsolete] // database still WIP
        public static Dictionary<int, JsonData> DescriptionData = new Dictionary<int, JsonData>();
        /// <summary>
        /// All Sprite data is cached in this variable if ShouldCacheData equals true.
        /// </summary>
        public static Dictionary<int, JsonData> SpriteData = new Dictionary<int, JsonData>();
        /// <summary>
        /// All Game data is cached in this variable if ShouldCacheData equals true.
        /// </summary>
        public static Dictionary<int, JsonData> GameData = new Dictionary<int, JsonData>();

        /// <summary>
        /// Specifies if the program should cache the data. Default is true.
        /// </summary>
        public static bool ShouldCacheData = true;
        #endregion

        #region Methods
        /// <summary>
        /// Gets the Pokédex data
        /// </summary>
        /// <returns>The Pokédex data</returns>
        public static JsonData GetPokedex()
        {
            if (Pokédex != null)
                return Pokédex;

            JsonData data = JsonMapper.ToObject(client.DownloadString("http://pokeapi.co/api/v1/pokedex/1/"));

            if (ShouldCacheData)
                Pokédex = data;

            return data;
        }

        /// <summary>
        /// Gets the data of the Pokémon with the specified ID
        /// </summary>
        /// <param name="id">The ID of the Pokémon to get</param>
        /// <returns>The raw data of the Pokémon</returns>
        public static JsonData GetPokemon(int id)
        {
            if (PokemonData.ContainsKey(id))
                return PokemonData[id];

            JsonData data = JsonMapper.ToObject(client.DownloadString("http://www.pokeapi.co/api/v1/pokemon/" + id + "/"));

            if (ShouldCacheData)
                PokemonData.Add(id, data);

            //IDs.Add(data["name"].ToString(), id);

            return data;
        }
        /// <summary>
        /// Puts all Pokémon data into the Pokémon cache. Does not require ShouldCacheData to be true.
        /// </summary>
        public static void CacheAllPokemon()
        {
            for (int i = 1; i <= 718; i++)
            {
                if (PokemonData.ContainsKey(i))
                    continue;

                Debug.WriteLine("Chaching Pokémon " + i);

                PokemonData.Add(i, JsonMapper.ToObject(client.DownloadString("http://www.pokeapi.co/api/v1/pokemon/" + i + "/")));
            }
        }

        /// <summary>
        /// Gets the data of the Type with the specified ID
        /// </summary>
        /// <param name="id">The ID of the Type to get</param>
        /// <returns>The raw data of the Type</returns>
        public static JsonData GetType(int id)
        {
            if (TypeData.ContainsKey(id))
                return TypeData[id];

            JsonData data = JsonMapper.ToObject(client.DownloadString("http://pokeapi.co/api/v1/type/" + id + "/"));

            if (ShouldCacheData)
                TypeData.Add(id, data);

            return data;
        }
        /// <summary>
        /// Puts all Type data into the Type cache. Does not require ShouldCacheData to be true.
        /// </summary>
        public static void CacheAllTypes()
        {
            for (int i = 1; i <= 18; i++)
            {
                if (TypeData.ContainsKey(i))
                    continue;

                Debug.WriteLine("Chaching Type " + i);

                TypeData.Add(i, JsonMapper.ToObject(client.DownloadString("http://www.pokeapi.co/api/v1/type/" + i + "/")));
            }
        }

        /// <summary>
        /// Gets the data of the Move with the specified ID
        /// </summary>
        /// <param name="id">The ID of the Move to get</param>
        /// <returns>The raw data of the Move</returns>
        public static JsonData GetMove(int id)
        {
            if (MoveData.ContainsKey(id))
                return MoveData[id];

            JsonData data = JsonMapper.ToObject(client.DownloadString("http://pokeapi.co/api/v1/move/" + id + "/"));

            if (ShouldCacheData)
                MoveData.Add(id, data);

            return data;
        }
        /// <summary>
        /// Puts all Move data into the Move cache. Does not require ShouldCacheData to be true.
        /// </summary>
        public static void CacheAllMoves()
        {
            for (int i = 1; i <= 625; i++)
            {
                if (MoveData.ContainsKey(i))
                    continue;

                Debug.WriteLine("Chaching Move " + i);

                MoveData.Add(i, JsonMapper.ToObject(client.DownloadString("http://www.pokeapi.co/api/v1/move/" + i + "/")));
            }
        }

        /// <summary>
        /// Gets the data of the Ability with the specified ID
        /// </summary>
        /// <param name="id">The ID of the Ability to get</param>
        /// <returns>The raw data of the Ability</returns>
        public static JsonData GetAbility(int id)
        {
            if (AbilityData.ContainsKey(id))
                return AbilityData[id];

            JsonData data = JsonMapper.ToObject(client.DownloadString("http://pokeapi.co/api/v1/ability/" + id + "/"));

            if (ShouldCacheData)
                AbilityData.Add(id, data);

            return data;
        }
        /// <summary>
        /// Puts all Ability data into the Ability cache. Does not require ShouldCacheData to be true.
        /// </summary>
        public static void CacheAllAbilities()
        {
            for (int i = 1; i <= 248; i++)
            {
                if (AbilityData.ContainsKey(i))
                    continue;

                Debug.WriteLine("Chaching Ability " + i);

                AbilityData.Add(i, JsonMapper.ToObject(client.DownloadString("http://www.pokeapi.co/api/v1/ability/" + i + "/")));
            }
        }

        /// <summary>
        /// Gets the data of the Egg group with the specified ID
        /// </summary>
        /// <param name="id">The ID of the Egg group to get</param>
        /// <returns>The raw data of the Egg group</returns>
        public static JsonData GetEggGroup(int id)
        {
            if (EggGroupData.ContainsKey(id))
                return EggGroupData[id];

            JsonData data = JsonMapper.ToObject(client.DownloadString("http://pokeapi.co/api/v1/egg/" + id + "/"));

            if (ShouldCacheData)
                EggGroupData.Add(id, data);

            return data;
        }
        /// <summary>
        /// Puts all Egg group data into the Egg group cache. Does not require ShouldCacheData to be true.
        /// </summary>
        public static void CacheAllEggGroups()
        {
            for (int i = 1; i <= 15; i++)
            {
                if (EggGroupData.ContainsKey(i))
                    continue;

                Debug.WriteLine("Chaching Egg group " + i);

                EggGroupData.Add(i, JsonMapper.ToObject(client.DownloadString("http://www.pokeapi.co/api/v1/egg/" + i + "/")));
            }
        }

        /// <summary>
        /// Gets the data of the Description with the specified ID
        /// </summary>
        /// <param name="id">The ID of the Description to get</param>
        /// <returns>The raw data of the Description</returns>
        [Obsolete] // database still WIP
        public static JsonData GetDescription(int id)
        {
            if (DescriptionData.ContainsKey(id))
                return DescriptionData[id];

            JsonData data = JsonMapper.ToObject(client.DownloadString("http://pokeapi.co/api/v1/description/" + id + "/"));

            if (ShouldCacheData)
                DescriptionData.Add(id, data);

            return data;
        }
        /// <summary>
        /// Puts all Description data into the Description cache. Does not require ShouldCacheData to be true.
        /// </summary>
        [Obsolete] // database still WIP
        public static void CacheAllDescriptions()
        {
            for (int i = 1; i <= 1; i++)
            {
                if (DescriptionData.ContainsKey(i))
                    continue;

                Debug.WriteLine("Chaching Description " + i);

                DescriptionData.Add(i, JsonMapper.ToObject(client.DownloadString("http://www.pokeapi.co/api/v1/description/" + i + "/")));
            }
        }

        /// <summary>
        /// Gets the data of the Sprite with the specified ID
        /// </summary>
        /// <param name="id">The ID of the Sprite to get</param>
        /// <returns>The raw data of the Sprite</returns>
        public static JsonData GetSprite(int id)
        {
            if (SpriteData.ContainsKey(id))
                return SpriteData[id];

            JsonData data = JsonMapper.ToObject(client.DownloadString("http://pokeapi.co/api/v1/sprite/" + id + "/"));

            if (ShouldCacheData)
                SpriteData.Add(id, data);

            return data;
        }
        /// <summary>
        /// Puts all Sprite data into the Sprite cache. Does not require ShouldCacheData to be true.
        /// </summary>
        public static void CacheAllSprites()
        {
            for (int i = 1; i <= 719; i++)
            {
                if (SpriteData.ContainsKey(i))
                    continue;

                Debug.WriteLine("Chaching Sprite " + i);

                SpriteData.Add(i, JsonMapper.ToObject(client.DownloadString("http://www.pokeapi.co/api/v1/sprite/" + i + "/")));
            }
        }

        /// <summary>
        /// Gets the data of the Game with the specified ID
        /// </summary>
        /// <param name="id">The ID of the Game to get</param>
        /// <returns>The raw data of the Game</returns>
        public static JsonData GetGame(int id)
        {
            if (GameData.ContainsKey(id))
                return GameData[id];

            JsonData data = JsonMapper.ToObject(client.DownloadString("http://pokeapi.co/api/v1/game/" + id + "/"));

            if (ShouldCacheData)
                GameData.Add(id, data);

            return data;
        }
        /// <summary>
        /// Puts all Game data into the Game cache. Does not require ShouldCacheData to be true.
        /// </summary>
        public static void CacheAllGames()
        {
            for (int i = 1; i <= 25; i++)
            {
                if (GameData.ContainsKey(i))
                    continue;

                Debug.WriteLine("Chaching Game " + i);

                GameData.Add(i, JsonMapper.ToObject(client.DownloadString("http://www.pokeapi.co/api/v1/game/" + i + "/")));
            }
        }
        #endregion
    }
}
