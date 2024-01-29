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

    public class Town
    {
        public string name;
        public string description;
        public List<GameAction> actions;
        public Biome biome;
        public Dominion dominion;


        public Town(string name)
        {
            this.name = name;
        }
    }
}