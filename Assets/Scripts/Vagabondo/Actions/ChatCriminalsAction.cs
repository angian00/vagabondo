using System;
using System.Collections.Generic;
using Vagabondo.DataModel;
using Vagabondo.Generators;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.Actions
{
    public class ChatCriminalsAction : GameAction
    {
        public ChatCriminalsAction(Town townData) : base(GameActionType.ChatCriminals, townData)
        {
            this.title = "Wander the streets";
            this.description = "Look for adventures (or trouble) in the seediest part of the town";
        }

        public override GameActionResult Perform(TravelManager travelManager)
        {
            var effectTypes = new List<GameActionEffectType>() {
                GameActionEffectType.Learn,
                GameActionEffectType.Trade,
                GameActionEffectType.MakeEnemies,
                GameActionEffectType.LoseMoney,
            };

            //TODO: result influenced by Knowledge.Street
            var effectType = RandomUtils.RandomChoose(effectTypes);
            switch (effectType)
            {
                case GameActionEffectType.Learn:
                    return performLearn(travelManager);
                case GameActionEffectType.Trade:
                    return performTrade(travelManager);
                case GameActionEffectType.MakeEnemies:
                    return performMakeEnemies(travelManager);
                case GameActionEffectType.LoseMoney:
                    return performLoseMoney(travelManager);
            }

            throw new Exception($"Invalid effectType: {DataUtils.EnumToStr(effectType)}");
        }

        private GameActionResult performTrade(TravelManager travelManager)
        {
            //TODO: ask for amount of money to spend
            var dealCost = 30; //TODO: check current traveler money
            travelManager.AddMoney(-dealCost);

            var tool = MerchandiseGenerator.GenerateItem(ItemCategory.Tool);

            travelManager.AddItem(tool);
            return new GameActionResult($"You are approached by a sketchy figure in an alley, proposing you a deal. " +
                $"You pay {dealCost}$, and get a <style=C1>{tool.name}</style> in exchange.");
        }

        private GameActionResult performLearn(TravelManager travelManager)
        {
            travelManager.IncrementStat(StatId.StreetSmarts);
            return new GameActionResult($"You get more used to navigating in such an hostile environment");
        }

        private GameActionResult performMakeEnemies(TravelManager travelManager)
        {
            travelManager.DecrementStat(StatId.Reputation);
            return new GameActionResult($"You are spotted in the seedy part of town, and people start talking");
        }

        private GameActionResult performLoseMoney(TravelManager travelManager)
        {
            var stolenAmount = 30; //TODO: check current traveler money

            travelManager.AddMoney(-stolenAmount);
            return new GameActionResult($"You are threatened by an armed guy, and forced to give him your money!"); //FUTURE: add choice tree
        }

    }

}
