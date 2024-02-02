using System.Collections.Generic;
using Vagabondo.Actions;

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
        Library,
    }


    public class Town
    {
        public string name;
        public TownSize size;
        public string description;
        public Biome biome;
        public Dominion dominion;
        public HashSet<TownBuilding> buildings;
        public List<GameAction> actions;

        public Town(string name)
        {
            this.name = name;
        }
    }
}