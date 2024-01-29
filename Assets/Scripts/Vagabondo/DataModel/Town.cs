using System.Collections.Generic;
using Vagabondo.Actions;

namespace Vagabondo.DataModel
{
    public enum Biome
    {
        Forest,
        Plains,
        Hills,
        Mountains,
        Lake,
    }

    public enum TownBuilding
    {
        Tavern,
        Library,
    }


    public class Town
    {
        public string name;
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