using System.Collections.Generic;

namespace Vagabondo
{
    public enum Biome
    {
        Forest,
        Plains,
        Hills,
        Mountains,
        Lake,
    }

    public class TownData
    {
        public string name;
        public string description;
        public List<GameAction> actions;
        public Biome biome;
        public DominionData dominion;


        public TownData(string name)
        {
            this.name = name;
        }
    }
}