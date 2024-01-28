using System;
using System.Collections.Generic;
using System.Linq;
using Vagabondo.Generators;

namespace Vagabondo
{
    public class TravelManager
    {
        private static TravelManager _travelerManager = new TravelManager();
        public static TravelManager Instance { get { return _travelerManager; } }

        private TownGenerator townGenerator;

        private TravelerData travelerData;

        private TownData currTown;
        private Dictionary<string, TownData> nextDestinations;


        private TravelManager()
        {
            travelerData = new TravelerData();
            townGenerator = new TownGenerator();
        }

        public void Init()
        {
            initFirstTown();
            EventManager.PublishTravelerChanged(travelerData);
        }

        private void initFirstTown()
        {
            nextDestinations = generateNextDestinations(1);
            var townName = nextDestinations.Keys.ToList()[0];
            TravelTo(townName);
        }


        public void PerformAction(GameAction action)
        {
            var resultText = action.Perform(this);
            //EventManager.PublishTravelerChanged(travelerData);
            EventManager.PublishActionPerformed(resultText);
        }

        public void TravelTo(string townName)
        {
            var destination = nextDestinations[townName];
            currTown = destination;
            EventManager.PublishTownChanged(currTown);

            foreach (var merchItem in travelerData.merchandise)
                updatePrice(merchItem);
            EventManager.PublishTravelerChanged(travelerData);

            const int nDestinations = 3;
            nextDestinations = generateNextDestinations(nDestinations, currTown);
            EventManager.PublishDestinationsChanged(nextDestinations.Values.ToList());
        }

        public void AddMoney(int delta)
        {
            travelerData.money += delta;
            EventManager.PublishTravelerChanged(travelerData);
        }

        public void AddTrinket(Trinket trinket)
        {
            travelerData.trinkets.Add(trinket);
            EventManager.PublishTravelerChanged(travelerData);
        }

        public void RemoveTrinket(Trinket trinket)
        {
            travelerData.trinkets.Remove(trinket);
            EventManager.PublishTravelerChanged(travelerData);
        }


        public void SellMerchandiseItem(MerchandiseItem merchItem)
        {
            travelerData.money += merchItem.price;
            travelerData.merchandise.Remove(merchItem);
            EventManager.PublishTravelerChanged(travelerData);
        }

        public void AddMerchandiseItem(MerchandiseItem merchItem)
        {
            travelerData.merchandise.Add(merchItem);
            updatePrice(merchItem);
            EventManager.PublishTravelerChanged(travelerData);
        }


        public void IncrementKnowledge(KnowledgeType knowledgeType)
        {
            int oldValue;
            travelerData.knowledge.TryGetValue(knowledgeType, out oldValue);
            travelerData.knowledge[knowledgeType] = oldValue + 1;
            EventManager.PublishTravelerChanged(travelerData);
        }

        private void updatePrice(MerchandiseItem merchItem)
        {
            //TODO: use quality and townData to influence price
            merchItem.price = merchItem.basePrice + (Math.Abs(currTown.GetHashCode())) % 100; //some deterministic variation
        }


        private Dictionary<string, TownData> generateNextDestinations(int nDestinations, TownData lastTown = null)
        {
            var result = new Dictionary<string, TownData>();
            for (int i = 0; i < nDestinations; i++)
            {
                var townData = townGenerator.GenerateTownData(lastTown); //TODO: make sure town id is not used again
                result[townData.name] = townData;
            }

            return result;
        }
    }
}
