using System.Collections.Generic;

namespace Vagabondo
{
    public struct Trinket
    {
        public string text;

        public Trinket(string text)
        {
            this.text = text;
        }
    }

    public struct TravelMemory
    {
        public string text;

        public TravelMemory(string text)
        {
            this.text = text;
        }
    }

    public enum KnowledgeType
    {
        Nature,
        Languages,
        Exoterism,
        Politics,
    }


    public class TravelerData
    {
        public static int startMoney = 100;
        //public static int startMoney = 0;

        public int money = startMoney;
        public List<MerchandiseItem> merchandise = new();
        public List<Trinket> trinkets = new();
        public List<TravelMemory> memories = new();
        public Dictionary<KnowledgeType, int> knowledge = new();
    }
}