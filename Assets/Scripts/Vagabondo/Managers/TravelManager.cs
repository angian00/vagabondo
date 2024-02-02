using System;
using System.Collections.Generic;
using System.Linq;
using Vagabondo.Actions;
using Vagabondo.DataModel;
using Vagabondo.Generators;

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
            currentTown.actions = generateActions(currentTown);

            EventManager.PublishTownChanged(currentTown);

            foreach (var merchItem in travelerData.merchandise)
                updatePrice(merchItem);
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


        private void updatePrice(MerchandiseItem merchItem)
        {
            //TODO: use quality and townData to influence price
            merchItem.price = merchItem.basePrice + (Math.Abs(currentTown.GetHashCode())) % 100; //some deterministic variation
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


        private List<GameAction> generateActions(Town townData)
        {
            const float questActionProbability = 0.8f;

            var actions = new List<GameAction>();
            actions.Add(new ForageAction(townData.biome));
            //if (hasWilderness())
            actions.Add(new ExploreAction());
            //if (hasCrime())
            actions.Add(new SketchyDealAction(townData));
            //if (friendly)
            actions.Add(new FoodGiftAction(townData));


            foreach (var building in townData.buildings)
            {
                GameAction action = null;
                switch (building)
                {
                    case TownBuilding.Tavern:
                        action = new TavernAction();
                        break;
                    case TownBuilding.Library:
                        action = new LibraryAction();
                        break;
                }

                if (action != null)
                    actions.Add(action);
            }

            if (UnityEngine.Random.value < questActionProbability)
            {
                var questState = activeQuest.GetCurrentState();
                var questAction = new QuestAction(questState);
                actions.Add(questAction);
            }

            return actions;
        }
    }
}
