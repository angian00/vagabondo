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


        public TavernAction(Town townData) : base(GameActionType.Tavern, townData)
        {
            this.title = "Go to the tavern";
            this.description = "Spend the night in the local watering hole";
        }

        public override bool isBuildingAction() => true;

        public override GameActionResult Perform(TravelManager travelManager)
        {
            var effectTypes = new List<GameActionEffectType>() {
                GameActionEffectType.Trade,
                GameActionEffectType.Gossip,
                GameActionEffectType.MakeFriends,
                GameActionEffectType.MakeEnemies,
                GameActionEffectType.Injury,
            };

            //DEBUG
            var effectType = RandomUtils.RandomChoose(effectTypes);
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
            var shopInventory = MerchandiseGenerator.GenerateInventory(ShopType.Tavern);
            PriceEvaluator.UpdatePrices(shopInventory);


            Predicate<GameItem> canBuy = ShopInfo.BuyFilter[ShopType.Tavern];

            var shopInfo = new ShopInfo("Tavern", shopInventory, canBuy);
            return new ShopActionResult($"You have the opportunity to buy some food and beverages, " +
                "or even sell some of your own", shopInfo);
        }

        private GameActionResult performGossip(TravelManager travelManager)
        {
            travelManager.IncrementStat(StatId.Diplomacy);

            var description = "You learn some interesting facts about the local government";
            var resultText = StringUtils.BuildResultTextStat(StatId.Diplomacy, 1);

            return new GameActionResult(description, resultText);
        }

        private GameActionResult performMakeFriends(TravelManager travelManager)
        {
            //TODO: check if you can spend money to make friends
            travelManager.IncrementStat(StatId.Reputation);

            var description = "You spend some time making friends with the other patrons";
            var resultText = StringUtils.BuildResultTextStat(StatId.Reputation, 1);

            return new GameActionResult(description, resultText);
        }

        private GameActionResult performMakeEnemies(TravelManager travelManager)
        {
            travelManager.DecrementStat(StatId.Reputation);

            var description = "You try to make friends, but get only hostile stares in return. You should work more on your people skills!";
            var resultText = StringUtils.BuildResultTextStat(StatId.Reputation, -1);

            return new GameActionResult(description, resultText);
        }

        private GameActionResult performInjury(TravelManager travelManager)
        {
            const int maxInjury = 5;

            var injuryAmount = UnityEngine.Random.Range(1, maxInjury);
            travelManager.AddHealth(-injuryAmount);

            var description = "You get involved in a fight and get the worst of it";
            var resultText = StringUtils.BuildResultTextHealth(-injuryAmount);


            return new GameActionResult(description, resultText);
        }
    }
}
