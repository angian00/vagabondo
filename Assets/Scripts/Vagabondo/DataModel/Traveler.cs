using System.Collections.Generic;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.DataModel
{
    public class Traveler
    {
        public int money;
        public int health;
        public int nutrition;
        public List<GameItem> merchandise = new();
        public List<Trinket> trinkets = new();
        public List<Memory> memories = new();
        public Dictionary<StatId, int> stats = new();

        public Traveler()
        {
            money = GameParams.Instance.startMoney;
            health = GameParams.Instance.startHealth;
            nutrition = GameParams.Instance.startNutrition;

            foreach (StatId statId in DataUtils.EnumValues<StatId>())
            {
                stats.Add(statId, 0);
            }
        }
    }
}