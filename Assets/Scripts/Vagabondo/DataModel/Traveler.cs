using System.Collections.Generic;

namespace Vagabondo.DataModel
{
    public class Traveler
    {
        public static int startMoney = 100;
        //public static int startMoney = 0;

        public int money = startMoney;
        public List<MerchandiseItem> merchandise = new();
        public List<Trinket> trinkets = new();
        public List<Memory> memories = new();
        public Dictionary<KnowledgeType, int> knowledge = new();
    }
}