using System;
using System.Collections.Generic;
using System.Linq;

namespace PokeAPI
{
    public struct BerryFlavorMap
    {
        /// <summary>
        /// The power of the referenced flavor for this berry.
        /// </summary>
        public int Potency
        {
            get;
        }

        /// <summary>
        /// The referenced berry flavor.
        /// </summary>
        public NamedApiResource<BerryFlavor> Flavor
        {
            get;
        }
    }
    public struct FlavorBerryMap
    {
        /// <summary>
        /// The power of the flavor for the referenced flavor.
        /// </summary>
        public int Potency
        {
            get;
        }

        /// <summary>
        /// The referenced berry.
        /// </summary>
        public NamedApiResource<Berry> Berry
        {
            get;
        }
    }

    public class Berry : NamedApiObject
    {
        /// <summary>
        /// Time it takes the tree to grow one stage. Berry trees go through four of these growth stages before they can be picked.
        /// </summary>
        public TimeSpan GrowthTime
        {
            get;
        }

        /// <summary>
        /// The maximum number of these berries that can grow on one tree in Generation IV.
        /// </summary>
        public int MaxHarvest
        {
            get;
        }

        /// <summary>
        /// The power of the move "Natural Gift" when used with this Berry.
        /// </summary>
        public int NaturalGiftPower
        {
            get;
        }

        /// <summary>
        /// The size of this Berry, in millimeters.
        /// </summary>
        public int Size
        {
            get;
        }

        /// <summary>
        /// The smoothness of this Berry, used in making Pokéblocks or Poffins.
        /// </summary>
        public int Smoothness
        {
            get;
        }

        /// <summary>
        /// The speed at which this Berry dries out the soil as it grows. A higher rate means the soil dries more quickly.
        /// </summary>
        public int SoilDryness
        {
            get;
        }

        /// <summary>
        /// The firmness of this berry, used in making Pokéblocks or Poffins.
        /// </summary>
        public NamedApiResource<BerryFirmness> Firmness
        {
            get;
        }

        /// <summary>
        /// A list of references to each flavor a berry can have and the potency of each of those flavors in regard to this berry.
        /// </summary>
        public BerryFlavorMap[] Flavors
        {
            get;
        }

        /// <summary>
        ///  	Berries are actually items. This is a reference to the item specific data for this berry.
        /// </summary>
        public NamedApiResource<Item> Item
        {
            get;
        }

        /// <summary>
        /// The Type the move "Natural Gift" has when used with this Berry.
        /// </summary>
        public NamedApiResource<PokemonType> NaturalGiftType
        {
            get;
        }
    }

    public class BerryFirmness : NamedApiObject
    {
        /// <summary>
        /// A list of the berries with this firmness.
        /// </summary>
        public NamedApiResource<Berry> Berries
        {
            get;
        }

        /// <summary>
        /// The name of this berry firmness listed in different languages.
        /// </summary>
        public ResourceName[] Names
        {
            get;
        }
    }

    public class BerryFlavor : NamedApiObject
    {
        /// <summary>
        /// A list of the berries with this flavor.
        /// </summary>
        public FlavorBerryMap[] Berries
        {
            get;
        }

        /// <summary>
        /// The contest type that correlates with this berry flavor.
        /// </summary>
        public NamedApiResource<ContestType> ContestType
        {
            get;
        }

        /// <summary>
        /// The name of this berry flavor listed in different languages.
        /// </summary>
        public ResourceName[] Names
        {
            get;
        }
    }
}
