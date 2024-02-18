using System.Collections.Generic;
using System.Text;
using Vagabondo.TownActions;
using Vagabondo.Utils;

namespace Vagabondo.DataModel
{
    public enum TownSize
    {
        Hamlet,
        Village,
        Town,
        City,
    }

    public enum TownTrait
    {
        Default,

        Rich,
        Poor,

        Wild,
        Rural,
        Industrial,

        HighCrime,

        Fanatic,

    }


    public enum TownBuilding
    {
        Church,
        Monastery,

        TownHall,
        //Manor,
        //Castle,

        ShopGeneric,
        Emporium,
        //Clothier,
        Bakery,
        Butchery,
        //Smith,
        //Carpenter,
        //Shoemaker,

        Farm,
        //Mine,
        //Mill,

        Tavern,
        Library,
        //Barber,
        //Bathhouse,
        //Stables,
    }

    public class TownTemplate
    {
        public TownSize size;
        public int nMaxBuildings;
        public List<TownTemplateBuildingInfo> buildings;
        public int nDestinations;
        public float baseAbundance;
        public int frequency;


        public Town Instantiate()
        {
            var town = new Town();
            town.size = size;
            town.nDestinations = nDestinations;
            town.baseAbundance = baseAbundance;

            return town;
        }
    }

    public class TownTemplateBuildingInfo
    {
        public TownBuilding buildingType;
        public float probability;
        public TownTrait traitNeeded = TownTrait.Default;
        public TownTrait traitExcluded = TownTrait.Default;
    }

    public class Town
    {
        public string name;
        public TownSize size;
        public string description;
        public Biome biome;
        public Dominion dominion;
        public int nDestinations;
        public float baseAbundance;

        public HashSet<TownTrait> traits = new();
        public HashSet<TownBuilding> buildings = new();
        public List<TownAction> actions = new();
        public HashSet<string> hints = new();
        public int nVisibleHints = 3;


        public string Dump()
        {
            StringBuilder res = new StringBuilder();

            res.Append($"{DataUtils.EnumToStr(size)} - {dominion.name} \n");
            res.Append($"biome: {DataUtils.EnumToStr(biome)}; ");
            res.Append("buildings: ");
            foreach (var building in buildings)
            {
                res.Append(DataUtils.EnumToStr(building));
                res.Append(", ");
            }
            res.Append("\n");

            res.Append("traits: ");
            foreach (var trait in traits)
            {
                res.Append(DataUtils.EnumToStr(trait));
                res.Append(", ");
            }

            return res.ToString();
        }
    }
}