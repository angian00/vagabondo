using System.Collections.Generic;

namespace Vagabondo.DataModel
{
    public class Traveler
    {
        public static int startMoney = 100;
        //public static int startMoney = 0;
        public static int startHealth = 10;

        public int money = startMoney;
        public int health = startHealth;
        public List<GameItem> merchandise = new();
        public List<Trinket> trinkets = new();
        public List<Memory> memories = new();
        public Dictionary<StatId, int> stats = new();
    }
}