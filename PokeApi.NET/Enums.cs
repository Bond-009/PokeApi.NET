using System;

namespace PokeAPI.NET
{
    /// <summary>
    /// Represents the category of a PokeMove
    /// </summary>
    public enum MoveCategory
    {
        /// <summary>
        /// A physical move
        /// </summary>
        Physical,
        /// <summary>
        /// A special move
        /// </summary>
        Special,
        /// <summary>
        /// A status-affecting move
        /// </summary>
        Status
    }
    /// <summary>
    /// Represents the types a Pokémon can be, as flags.
    /// </summary>
    [Flags]
    public enum PokemonType : int
    {
        /// <summary>
        /// The ??? type
        /// </summary>
        None = 0,
        /// <summary>
        /// The bug type
        /// </summary>
        Bug = 1,
        /// <summary>
        /// The dark type
        /// </summary>
        Dark = 2,
        /// <summary>
        /// The dragon type
        /// </summary>
        Dragon = 4,
        /// <summary>
        /// The electric type
        /// </summary>
        Electric = 8,
        /// <summary>
        /// The fairy type
        /// </summary>
        Fairy = 16,
        /// <summary>
        /// The fighting type
        /// </summary>
        Fight = 32,
        /// <summary>
        /// The fire type
        /// </summary>
        Fire = 64,
        /// <summary>
        /// The flying type
        /// </summary>
        Flying = 128,
        /// <summary>
        /// The ghost type
        /// </summary>
        Ghost = 256,

        /// <summary>
        /// The grass type
        /// </summary>
        Grass = 512,
        /// <summary>
        /// The ground type
        /// </summary>
        Ground = 1024,
        /// <summary>
        /// The ice type
        /// </summary>
        Ice = 2048,
        /// <summary>
        /// The normal type
        /// </summary>
        Normal = 4096,
        /// <summary>
        /// The poison type
        /// </summary>
        Poison = 8192,
        /// <summary>
        /// The psychic type
        /// </summary>
        Psychic = 16384,
        /// <summary>
        /// The rock type
        /// </summary>
        Rock = 32768,
        /// <summary>
        /// The steel type
        /// </summary>
        Steel = 65536,
        /// <summary>
        /// The water type
        /// </summary>
        Water = 131072
    }
    /// <summary>
    /// Represents the ability of a Pokémon
    /// </summary>
    public enum Ability
    {
        #region A-L
        Adaptability,
        Aerilate,
        Aftermath,
        Air_Lock,
        Analytic,
        Anger_Point,
        Anticipation,
        Arena_Trap,
        Aroma_Veil,
        Aura_Break,
        Bad_Dreams,
        Battle_Armor,
        Big_Pecks,
        Blaze,
        Bulletproof,
        Cheek_Pouch,
        Chlorophyll,
        Clear_Body,
        Cloud_Nine,
        Competitive,
        Compoundeyes,
        Contrary,
        Cursed_Body,
        Cute_Charm,
        Damp,
        Dark_Aura,
        Defeatist,
        Defiant,
        Download,
        Drizzle,
        Drought,
        Dry_Skin,
        Early_Bird,
        Effect_Spore,
        Fairy_Aura,
        Filter,
        Flame_Body,
        Flare_Boost,
        Flash_Fire,
        Flower_Gift,
        Flower_Veil,
        Forecast,
        Forewarn,
        Friend_Guard,
        Frisk,
        Fur_Coat,
        Gale_Wings,
        Gluttony,
        Gooey,
        Grass_Pelt,
        Guts,
        Harvest,
        Healer,
        Heatproof,
        Heavy_Metal,
        Honey_Gather,
        Huge_Power,
        Hustle,
        Hydration,
        Hyper_Cutter,
        Ice_Body,
        Illuminate,
        Illusion,
        Immunity,
        Imposter,
        Infiltrator,
        Inner_Focus,
        Insomnia,
        Intimidate,
        Iron_Barbs,
        Iron_Fist,
        Justified,
        Keen_Eye,
        Klutz,
        Leaf_Guard,
        Levitate,
        Light_Metal,
        Lightningrod,
        Limber,
        Liquid_Ooze
        #endregion
,
        #region M-Z
        Magic_Bounce,
        Magic_Guard,
        Magician,
        Magma_Armor,
        Magnet_Pull,
        Marvel_Scale,
        Mega_Launcher,
        Minus,
        Mold_Breaker,
        Moody,
        Motor_Drive,
        Moxie,
        Multiscale,
        Multitype,
        Mummy,
        Natural_Cure,
        No_Guard,
        Normalize,
        Oblivious,
        Overcoat,
        Overgrow,
        Own_Tempo,
        Parental_Bond,
        Pickpocket,
        Pickup,
        Pixilate,
        Plus,
        Poison_Heal,
        Poison_Point,
        Poison_Touch,
        Prankster,
        Pressure,
        Protean,
        Pure_Power,
        Quick_Feet,
        Rain_Dish,
        Rattled,
        Reckless,
        Refrigerate,
        Refrigerator,
        Rivalry,
        Rock_Head,
        Rough_Skin,
        Run_Away,
        Sand_Force,
        Sand_Rush,
        Sand_Stream,
        Sand_Veil,
        Sap_Sipper,
        Scrappy,
        Serene_Grace,
        Shadow_Tag,
        Shed_Skin,
        Sheer_Force,
        Shell_Armor,
        Shield_Dust,
        Simple,
        Skill_Link,
        Slow_Start,
        Sniper,
        Sniw_Cloak,
        Snow_Warning,
        Solar_Power,
        Solid_Rock,
        Soundproof,
        Speed_Boost,
        Stall,
        Stance_Change,
        Static,
        Steadfast,
        Stench,
        Sticky_Hold,
        Storm_Drain,
        Strong_Jaw,
        Sturdy,
        Suction_Cups,
        Super_Luck,
        Swarm,
        Sweet_Veil,
        Swift_Swim,
        Symbiosis,
        Synchronize,
        Tangled_Feet,
        Technician,
        Telepathy,
        Teravolt,
        Thick_Fat,
        Tinted_Lens,
        Torrent,
        Tough_Claws,
        Toxic_Boost,
        Trace,
        Truant,
        Turboblaze,
        Unaware,
        Unburden,
        Unnerve,
        Victory_Star,
        Vital_Spirit,
        Volt_Absorb,
        Water_Absorb,
        Water_Veil,
        Weak_Armor,
        White_Smoke,
        Wonder_Guard,
        Winder_Skin,
        Zen_Mode
        #endregion
    }
    /// <summary>
    /// Represents the egg group of a Pokémon
    /// </summary>
    public enum EggGroup
    {
        Monster,
        Water1,
        Bug,
        Flying,
        Ground,
        Fairy,
        Plant,
        Human_Like,
        Water3,
        Mineral,
        Intermidate,
        Water2,
        Ditto,
        Dragon,
        /// <summary>
        /// Cannot make eggs
        /// </summary>
        Undiscovered
    }
    /// <summary>
    /// Represents a Pokémon game
    /// </summary>
    public enum PokemonGame
    {
        /// <summary>
        /// Red (Japan)
        /// </summary>
        Red_Jpn,
        /// <summary>
        /// Green (Japan)
        /// </summary>
        Green_Jpn,
        /// <summary>
        /// Blue (Japan)
        /// </summary>
        Blue_Jpn,
        Red,
        Blue,
        Yellow,
        Gold,
        Silver,
        Crystal,
        Ruby,
        Sapphire,
        Emerald,
        Firered,
        Leafgreen,
        Pearl,
        Diamond,
        Platinum,
        Heartgold,
        Soulsilver,
        Black,
        White,
        Black_2,
        White_2,
        X,
        Y
    }
}
