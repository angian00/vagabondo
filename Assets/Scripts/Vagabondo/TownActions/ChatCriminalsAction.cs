using System;
using System.Collections.Generic;
using Vagabondo.DataModel;
using Vagabondo.Generators;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.TownActions
{
    public class ChatCriminalsAction : TownAction
    {
        public ChatCriminalsAction(Town townData) : base(GameActionType.ChatCriminals, townData)
        {
            this.title = "Wander the streets";
            this.description = "Look for adventures (or trouble) in the seediest part of the town";
        }

        public override bool isEventAction() => true;

        public override TownActionResult Perform(TravelManager travelManager)
        {
            var effectTypes = new List<TownActionEffectType>() {
                TownActionEffectType.Learn,
                TownActionEffectType.Trade,
                TownActionEffectType.MakeEnemies,
                TownActionEffectType.LoseMoney,
            };

            //TODO: result influenced by Knowledge.Street
            var effectType = RandomUtils.RandomChoose(effectTypes);
            switch (effectType)
            {
                case TownActionEffectType.Learn:
                    return performLearn(travelManager);
                case TownActionEffectType.Trade:
                    return performTrade(travelManager);
                case TownActionEffectType.MakeEnemies:
                    return performMakeEnemies(travelManager);
                case TownActionEffectType.LoseMoney:
                    return performLoseMoney(travelManager);
            }

            throw new Exception($"Invalid effectType: {DataUtils.EnumToStr(effectType)}");
        }

        private TownActionResult performTrade(TravelManager travelManager)
        {
            const int maxDealCost = 50;
            string description;
            string resultText;

            var dealCost = UnityEngine.Random.Range(1, maxDealCost);
            if (travelManager.travelerData.money < dealCost)
            {
                travelManager.DecrementStat(StatId.StreetSmarts);
                travelManager.DecrementStat(StatId.StreetSmarts);

                description = "Seeing you don't have much money, the people on the street look at you with contempt";
                resultText = StringUtils.BuildResultTextStat(StatId.StreetSmarts, -2);

                return new TownActionResult(description, resultText);
            }

            travelManager.AddMoney(-dealCost);
            //TODO: generate item with appropriate price
            var item = ShopInventoryGenerator.GenerateItem(ItemCategory.Tool);
            travelManager.AddItem(item);

            description = "You are approached by a sketchy figure in an alley, proposing you a deal";
            resultText = StringUtils.BuildResultTextMoney(-dealCost) + "\n\n" +
                StringUtils.BuildResultTextItem(item, true);

            return new TownActionResult(description, resultText);
        }

        private TownActionResult performLearn(TravelManager travelManager)
        {
            travelManager.IncrementStat(StatId.StreetSmarts);

            var description = "You get more used to navigating in such an hostile environment";
            var resultText = StringUtils.BuildResultTextStat(StatId.StreetSmarts, -1);

            return new TownActionResult(description, resultText);
        }

        private TownActionResult performMakeEnemies(TravelManager travelManager)
        {
            travelManager.DecrementStat(StatId.Reputation);

            var description = "You are spotted in the seedy part of town, and people start talking";
            var resultText = StringUtils.BuildResultTextStat(StatId.Reputation, -1);


            return new TownActionResult(description, resultText);
        }

        private TownActionResult performLoseMoney(TravelManager travelManager)
        {
            const int maxStolenAmount = 50;
            string description;
            string resultText;

            var stolenAmount = UnityEngine.Random.Range(1, maxStolenAmount);
            if (travelManager.travelerData.money < stolenAmount)
            {
                stolenAmount = travelManager.travelerData.money;
                var injuryAmount = 3;
                travelManager.AddMoney(-stolenAmount);
                travelManager.AddHealth(-injuryAmount);

                //FUTURE: add choice tree
                description = "You are threatened by an armed guy, and forced to give him your money!" +
                    " Since you don't have much money, the thief beats you up anyway";
                resultText = StringUtils.BuildResultTextMoney(-stolenAmount) + "\n\n" + StringUtils.BuildResultTextHealth(-injuryAmount);

                return new TownActionResult(description, resultText);
            }

            travelManager.AddMoney(-stolenAmount);

            //FUTURE: add choice tree
            description = "You are threatened by an armed guy, and forced to give him your money!";
            resultText = StringUtils.BuildResultTextMoney(-stolenAmount);

            return new TownActionResult(description, resultText);
        }

    }
}
