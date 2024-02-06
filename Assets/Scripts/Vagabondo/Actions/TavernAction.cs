using System;
using System.Collections.Generic;
using Vagabondo.DataModel;
using Vagabondo.Generators;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.Actions
{
    public class TavernAction : GameAction
    {
        public static int tavernCost = 10; //TODO: make tavernCost variable


        public TavernAction() : base(GameActionType.Tavern)
        {
            this.title = "Go to the tavern";
            this.description = "Spend the night in the local watering hole";
        }

        public override GameActionResult Perform(TravelManager travelManager)
        {
            var effectTypes = new List<GameActionEffectType>() {
                GameActionEffectType.Trade,
                GameActionEffectType.Gossip,
                GameActionEffectType.MakeFriends,
                GameActionEffectType.MakeEnemies,
                GameActionEffectType.Injury,
            };

            //TODO: check if you can spend money to make friends

            //DEBUG
            //var effectType = RandomUtils.RandomChoose(effectTypes);
            var effectType = GameActionEffectType.Trade;
            switch (effectType)
            {
                case GameActionEffectType.Trade:
                    return performTrade(travelManager);
                case GameActionEffectType.Gossip:
                    return performGossip(travelManager);
                case GameActionEffectType.MakeFriends:
                    return performMakeFriends(travelManager);
                case GameActionEffectType.MakeEnemies:
                    return performMakeEnemies(travelManager);
                case GameActionEffectType.Injury:
                    return performInjury(travelManager);
            }

            throw new Exception($"Invalid effectType: {DataUtils.EnumToStr(effectType)}");
        }


        private GameActionResult performTrade(TravelManager travelManager)
        {
            var shopInventory = MerchandiseGenerator.GenerateInventoryFood(ShopType.Tavern);
            PriceEvaluator.UpdatePrices(shopInventory);
            return new ShopActionResult($"You have the opportunity to buy some food and beverages, " +
                "or even sell some of your own", shopInventory);
        }

        private GameActionResult performGossip(TravelManager travelManager)
        {
            travelManager.IncrementStat(StatId.Diplomacy);
            return new GameActionResult($"You learn some interesting facts about the local government");
        }

        private GameActionResult performMakeFriends(TravelManager travelManager)
        {
            travelManager.IncrementStat(StatId.Reputation);
            return new GameActionResult($"You spend some time making friends with the other patrons");
        }

        private GameActionResult performMakeEnemies(TravelManager travelManager)
        {
            travelManager.DecrementStat(StatId.Reputation);
            return new GameActionResult($"You try to make friends, but get only hostile stares in return. You should work more on your people skills.");
        }

        private GameActionResult performInjury(TravelManager travelManager)
        {
            const int maxInjury = 5;

            var injuryAmount = UnityEngine.Random.Range(1, maxInjury);
            travelManager.AddHealth(-injuryAmount);

            return new GameActionResult($"You get involved in a fight and get the worst of it");
        }
    }
}
