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
        Tavern,
        Church,
        Monastery,

        TownHall,
        Manor,
        Castle,

        Marketplace,

        ShopGeneric,
        Clothier,
        Baker,
        Butcher,
        Smith,
        Carpenter,
        Shoemaker,

        Farm,
        Mine,
        Mill,

        Library,
        Barber,
        Bathhouse,
        //Stables,
    }

    public class TownTemplate
    {
        public int nMaxBuildings;
        public List<TownTemplateBuildingInfo> buildings;
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
        public HashSet<DominionTrait> traits;
        public HashSet<TownBuilding> buildings;
        public List<GameAction> actions;

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