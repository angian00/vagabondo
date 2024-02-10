using System;
using System.Collections.Generic;
using Vagabondo.DataModel;
using Vagabondo.Generators;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.Actions
{
    public class ChatLocalsAction : GameAction
    {
        public ChatLocalsAction(Town townData) : base(GameActionType.ChatLocals, townData)
        {
            this.title = "Chat with the local folk";
            this.description = "";
        }

        public override GameActionResult Perform(TravelManager travelManager)
        {
            var effectTypes = new List<GameActionEffectType>() {
                GameActionEffectType.Learn,
                //FUTURE: learn recipe
                GameActionEffectType.ReceiveItem,
                GameActionEffectType.MakeFriends,
                GameActionEffectType.MakeEnemies,
                GameActionEffectType.Injury,
            };

            //TODO: result influenced by Knowledge.Diplomacy
            var effectType = RandomUtils.RandomChoose(effectTypes);
            switch (effectType)
            {
                case GameActionEffectType.Learn:
                    return performLearn(travelManager);
                case GameActionEffectType.ReceiveItem:
                    return performReceiveItem(travelManager);
                case GameActionEffectType.MakeFriends:
                    return performMakeFriends(travelManager);
                case GameActionEffectType.MakeEnemies:
                    return performMakeEnemies(travelManager);
                case GameActionEffectType.Injury:
                    return performInjury(travelManager);
            }

            throw new Exception($"Invalid effectType: {DataUtils.EnumToStr(effectType)}");
        }

        private GameActionResult performLearn(TravelManager travelManager)
        {
            travelManager.IncrementStat(StatId.Diplomacy);
            return new GameActionResult($"You learn some interesting facts about the local people");
        }

        private GameActionResult performReceiveItem(TravelManager travelManager)
        {
            var tool = MerchandiseGenerator.GenerateItem(ItemCategory.Tool);

            travelManager.AddItem(tool);
            return new GameActionResult($"As a sign of good will, you are gifted a useful <style=C1>{tool}</style>");
        }

        private GameActionResult performMakeFriends(TravelManager travelManager)
        {
            travelManager.IncrementStat(StatId.Reputation);
            return new GameActionResult($"You make friends with some of the locals");
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
