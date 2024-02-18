using System;
using System.Collections.Generic;
using Vagabondo.DataModel;
using Vagabondo.Generators;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.TownActions
{
    public class TavernAction : TownAction
    {
        public static int tavernCost = 10; //TODO: make tavernCost variable


        public TavernAction(Town townData) : base(GameActionType.Tavern, townData)
        {
            this.title = "Go to the tavern";
            this.description = "Spend the night in the local watering hole";
        }

        public override bool isBuildingAction() => true;

        public override TownActionResult Perform(TravelManager travelManager)
        {
            var effectTypes = new List<TownActionEffectType>() {
                TownActionEffectType.Trade,
                TownActionEffectType.Gossip,
                TownActionEffectType.MakeFriends,
                TownActionEffectType.MakeEnemies,
                TownActionEffectType.Injury,
            };

            //DEBUG
            var effectType = RandomUtils.RandomChoose(effectTypes);
            switch (effectType)
            {
                case TownActionEffectType.Trade:
                    return performTrade(travelManager);
                case TownActionEffectType.Gossip:
                    return performGossip(travelManager);
                case TownActionEffectType.MakeFriends:
                    return performMakeFriends(travelManager);
                case TownActionEffectType.MakeEnemies:
                    return performMakeEnemies(travelManager);
                case TownActionEffectType.Injury:
                    return performInjury(travelManager);
            }

            throw new Exception($"Invalid effectType: {DataUtils.EnumToStr(effectType)}");
        }


        private TownActionResult performTrade(TravelManager travelManager)
        {
            var shopInventory = MerchandiseGenerator.GenerateInventory(ShopType.Tavern);
            PriceEvaluator.UpdatePrices(shopInventory, townData);


            Predicate<GameItem> canBuy = ShopInfo.BuyFilter[ShopType.Tavern];

            var shopInfo = new ShopInfo("Tavern", shopInventory, canBuy);
            return new ShopActionResult($"You have the opportunity to buy some food and beverages, " +
                "or even sell some of your own", shopInfo);
        }

        private TownActionResult performGossip(TravelManager travelManager)
        {
            travelManager.IncrementStat(StatId.Diplomacy);

            var description = "You learn some interesting facts about the local government";
            var resultText = StringUtils.BuildResultTextStat(StatId.Diplomacy, 1);

            return new TownActionResult(description, resultText);
        }

        private TownActionResult performMakeFriends(TravelManager travelManager)
        {
            //TODO: check if you can spend money to make friends
            travelManager.IncrementStat(StatId.Reputation);

            var description = "You spend some time making friends with the other patrons";
            var resultText = StringUtils.BuildResultTextStat(StatId.Reputation, 1);

            return new TownActionResult(description, resultText);
        }

        private TownActionResult performMakeEnemies(TravelManager travelManager)
        {
            travelManager.DecrementStat(StatId.Reputation);

            var description = "You try to make friends, but get only hostile stares in return. You should work more on your people skills!";
            var resultText = StringUtils.BuildResultTextStat(StatId.Reputation, -1);

            return new TownActionResult(description, resultText);
        }

        private TownActionResult performInjury(TravelManager travelManager)
        {
            const int maxInjury = 5;

            var injuryAmount = UnityEngine.Random.Range(1, maxInjury);
            travelManager.AddHealth(-injuryAmount);

            var description = "You get involved in a fight and get the worst of it";
            var resultText = StringUtils.BuildResultTextHealth(-injuryAmount);


            return new TownActionResult(description, resultText);
        }
    }
}
