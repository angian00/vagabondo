using System;
using System.Collections.Generic;
using System.Linq;
using Vagabondo.Actions;
using Vagabondo.DataModel;
using Vagabondo.Generators;
using Vagabondo.Utils;

namespace Vagabondo.Managers
{
    public class TravelManager
    {
        private static TravelManager _travelerManager = new TravelManager();
        public static TravelManager Instance { get { return _travelerManager; } }

        private TownGenerator townGenerator;

        private Traveler travelerData;

        private Town currentTown;
        private Quest activeQuest;
        private Dictionary<string, Town> nextDestinations;


        private TravelManager()
        {
            travelerData = new Traveler();
            townGenerator = new TownGenerator();
            NewQuest();
        }

        public void Init()
        {
            initFirstTown();
            EventManager.PublishTravelerChanged(travelerData);
        }

        private void initFirstTown()
        {
            nextDestinations = generateNextDestinations(1);
            var townName = nextDestinations.Keys.First();
            TravelTo(townName);
        }


        public void PerformAction(GameAction action)
        {
            var actionResult = action.Perform(this);
            EventManager.PublishActionPerformed(actionResult);
        }

        public void TravelTo(string townName)
        {
            var destination = nextDestinations[townName];
            currentTown = destination;

            ActionGenerator.GenerateActions(currentTown);
            maybeAddQuestAction(currentTown);

            EventManager.PublishTownChanged(currentTown);

            PriceEvaluator.UpdatePrices(travelerData.merchandise);
            EventManager.PublishTravelerChanged(travelerData);

            const int nDestinations = 3;
            nextDestinations = generateNextDestinations(nDestinations, currentTown);
            EventManager.PublishDestinationsChanged(nextDestinations.Values.ToList());
        }

        public void AddMoney(int delta)
        {
            travelerData.money += delta;
            EventManager.PublishTravelerChanged(travelerData);
        }

        public void AddHealth(int delta)
        {
            travelerData.health += delta;
            EventManager.PublishTravelerChanged(travelerData);

            //TODO: defeat event
            //if (travelerData.health < 0)
            //    EventManager.PublishTravelerChanged(travelerData);

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


        public void AddItem(GameItem item)
        {
            travelerData.merchandise.Add(item);
            EventManager.PublishTravelerChanged(travelerData);
        }

        public void RemoveItem(GameItem item)
        {
            travelerData.merchandise.Remove(item);
            EventManager.PublishTravelerChanged(travelerData);
        }

        public void RemoveAnyItem()
        {
            var item = RandomUtils.RandomChoose(travelerData.merchandise);
            RemoveItem(item);
        }

        public void TradeItem(GameItem item, bool isTravelerSelling)
        {
            if (isTravelerSelling)
            {
                travelerData.money += item.currentPrice;
                travelerData.merchandise.Remove(item);
            }
            else
            {
                travelerData.money -= item.currentPrice;
                travelerData.merchandise.Add(item);
            }
            EventManager.PublishTravelerChanged(travelerData);
        }


        public void IncrementStat(StatId statId)
        {
            addToStat(statId, 1);
        }

        public void DecrementStat(StatId statId)
        {
            addToStat(statId, -1);
        }

        private void addToStat(StatId statId, int delta)
        {
            int oldValue;
            travelerData.stats.TryGetValue(statId, out oldValue);
            travelerData.stats[statId] = oldValue + delta;
            EventManager.PublishTravelerChanged(travelerData);
        }

        public void AddMemory(Memory memory)
        {
            travelerData.memories.Add(memory);
            EventManager.PublishTravelerChanged(travelerData);
        }

        public void RemoveQuestFragments(Guid questId)
        {
            travelerData.memories.RemoveAll(m =>
               (m is QuestFragmentMemory) && (((QuestFragmentMemory)m).questId == questId));
            EventManager.PublishTravelerChanged(travelerData);
        }

        public void NewQuest()
        {
            activeQuest = QuestGenerator.GenerateQuest();
        }


        private Dictionary<string, Town> generateNextDestinations(int nDestinations, Town lastTown = null)
        {
            var result = new Dictionary<string, Town>();
            for (int i = 0; i < nDestinations; i++)
            {
                var townData = townGenerator.GenerateTown(lastTown); //TODO: make sure town name is not used again

                result[townData.name] = townData;
            }

            return result;
        }


        private void maybeAddQuestAction(Town townData)
        {
            const float questActionProbability = 0.8f;

            if (UnityEngine.Random.value < questActionProbability)
            {
                var questState = activeQuest.GetCurrentState();
                var questAction = new QuestAction(questState, townData);
                townData.actions.Add(questAction);
            }
        }
    }
}
