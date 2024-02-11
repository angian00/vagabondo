using System.Collections.Generic;
using System.Text;
using Vagabondo.Actions;
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
        public int frequency;
    }

    public class TownTemplateBuildingInfo
    {
        public TownBuilding buildingType;
        public float probability;
        public DominionTrait traitNeeded = DominionTrait.Default;
        public DominionTrait traitExcluded = DominionTrait.Default;
    }

    public class Town
    {
        public string name;
        public TownSize size;
        public string description;
        public Biome biome;
        public Dominion dominion;
        public int nDestinations;
        public HashSet<DominionTrait> traits = new();
        public HashSet<TownBuilding> buildings = new();
        public List<GameAction> actions = new();
        public HashSet<string> hints = new();
        public int nVisibleHints = 3;

        public Town(string name)
        {
            this.name = name;
        }

        public string Dump()
        {
            StringBuilder res = new StringBuilder();

            res.Append(DataUtils.EnumToStr(size) + " - " + dominion.name + "\n");
            //res.Append(DataUtils.EnumToStr(biome) + "\n");
            res.Append("traits: ");
            foreach (var trait in traits)
            {
                res.Append(DataUtils.EnumToStr(trait));
                res.Append(", ");
            }
            res.Append("\n");

            res.Append("buildings: ");
            foreach (var building in buildings)
            {
                res.Append(DataUtils.EnumToStr(building));
                res.Append(", ");
            }

            return res.ToString();
        }
    }
}