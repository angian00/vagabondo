using System;
using System.Collections.Generic;
using Vagabondo.Actions;
using Vagabondo.DataModel;
using Vagabondo.Generators;
using Vagabondo.Utils;

namespace Vagabondo.Managers
{
    public class TravelManager
    {
        private static TravelManager _travelerManager;
        public static TravelManager Instance { get { return _travelerManager; } }

        private TownGenerator townGenerator;

        public Traveler travelerData;

        private Town currentTown;
        private Quest activeQuest;
        private List<Town> nextDestinations;


        public static void Init()
        {
            _travelerManager = new TravelManager();
            _travelerManager.initData();
        }

        private TravelManager()
        {
            travelerData = new Traveler();
            townGenerator = new TownGenerator();
            NewQuest();
        }

        private void initData()
        {
            var firstTown = townGenerator.GenerateTown(null);
            TravelTo(firstTown);
            EventManager.PublishTravelerChanged(travelerData);
        }

        public void PerformAction(GameAction action)
        {
            var actionResult = action.Perform(this);
            EventManager.PublishActionPerformed(actionResult);
        }

        public void TravelTo(Town destination)
        {
            currentTown = destination;

            ActionGenerator.GenerateActions(currentTown);
            maybeAddQuestAction(currentTown);

            EventManager.PublishTownChanged(currentTown);

            PriceEvaluator.UpdatePrices(travelerData.merchandise);
            EventManager.PublishTravelerChanged(travelerData);

            nextDestinations = generateNextDestinations(currentTown);
            EventManager.PublishDestinationsChanged(nextDestinations);
        }

        public void AddMoney(int delta)
        {
            travelerData.money += delta;
            if (travelerData.money < 0)
                throw new Exception("traveler money cannot become negative!");

            EventManager.PublishTravelerChanged(travelerData);
        }

        public void AddHealth(int delta)
        {
            travelerData.health += delta;
            EventManager.PublishTravelerChanged(travelerData);

            if (travelerData.health <= 0)
                EventManager.PublishGameOver();
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
            travelerData.stats[statId] += delta;
            EventManager.PublishTravelerChanged(travelerData);

            foreach (var dest in nextDestinations)
                updateNVisibleHints(dest, travelerData);

            EventManager.PublishDestinationsChanged(nextDestinations);
        }


        //public void AddTrinket(Trinket trinket)
        //{
        //    travelerData.trinkets.Add(trinket);
        //    EventManager.PublishTravelerChanged(travelerData);
        //}

        //public void RemoveTrinket(Trinket trinket)
        //{
        //    travelerData.trinkets.Remove(trinket);
        //    EventManager.PublishTravelerChanged(travelerData);
        //}


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

        public GameItem RemoveAnyItem()
        {
            var item = RandomUtils.RandomChoose(travelerData.merchandise);
            RemoveItem(item);

            return item;
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


        private List<Town> generateNextDestinations(Town currTown)
        {
            var result = new List<Town>();

            for (int i = 0; i < currTown.nDestinations; i++)
            {
                var townData = townGenerator.GenerateTown(currTown);
                townData.hints = generateTownHints(townData, travelerData);

                result.Add(townData);
            }

            return result;
        }

        private HashSet<string> generateTownHints(Town townData, Traveler travelerData)
        {
            var allHints = generateAllTownHints(townData, travelerData);

            var res = new HashSet<string>();
            var nHints = Math.Min(GameParams.nMaxHints, allHints.Count);
            while (res.Count < nHints)
                res.Add(RandomUtils.RandomChoose(allHints));

            return res;
        }

        private List<string> generateAllTownHints(Town townData, Traveler travelerData)
        {
            var res = new List<string>();

            res.Add($"It is ruled by the {townData.dominion.name}");

            switch (townData.biome)
            {
                case Biome.Forest:
                    res.Add("It is surrounded by a dense forest");
                    break;
                case Biome.Plains:
                    res.Add("It is surrounded by plains");
                    break;
                case Biome.Hills:
                    res.Add("It is surrounded by hills");
                    break;
                case Biome.Mountains:
                    res.Add("It is surrounded by mountains");
                    break;
                case Biome.Lake:
                    res.Add("It is near a lake");
                    break;

                    //case Biome.Desert:
                    //res.Add("It is surrounded by a desert");
                    //break;
            }

            switch (townData.size)
            {
                case TownSize.Hamlet:
                    res.Add("It is a tiny hamlet");
                    break;
                case TownSize.Village:
                    res.Add("It is a small village");
                    break;
                case TownSize.Town:
                    res.Add("It is a medium-sized town");
                    break;
                case TownSize.City:
                    res.Add("It is a city");
                    break;
            }

            foreach (var building in townData.buildings)
                res.Add($"There is a {DataUtils.EnumToStr(building).ToLower()} in this settlement");

            foreach (var trait in townData.traits)
            {
                switch (trait)
                {
                    case DominionTrait.Rich:
                        res.Add("There are a lot of wealthy people");
                        break;
                    case DominionTrait.Poor:
                        res.Add("It is poor");
                        break;
                    case DominionTrait.Wild:
                        res.Add("It is surrounded by the wilderness");
                        break;
                    case DominionTrait.Rural:
                        res.Add("It is surrounded by farms and fields");
                        break;
                    case DominionTrait.Industrial:
                        res.Add("It is known for its industry");
                        break;
                    case DominionTrait.HighCrime:
                        res.Add("There is a lot of crime");
                        break;
                    case DominionTrait.Fanatic:
                        res.Add("Lots of religious fanatics there");
                        break;
                }
            }

            return res;
        }

        private void updateNVisibleHints(Town townData, Traveler travelerData)
        {
            //TODO: implement updateNVisibleHints logic
            townData.nVisibleHints = townData.hints.Count;
        }



        private void maybeAddQuestAction(Town townData)
        {
            if (UnityEngine.Random.value < GameParams.questActionProbability)
            {
                var questState = activeQuest.GetCurrentState();
                var questAction = new QuestAction(questState, townData);
                townData.actions.Add(questAction);
            }
        }
    }
}
