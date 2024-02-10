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

        public override bool isEventAction() => true;

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

            var description = "You learn some interesting facts about the local people";
            var resultText = StringUtils.BuildResultTextStat(StatId.Diplomacy, 1);

            return new GameActionResult(description, resultText);
        }

        private GameActionResult performReceiveItem(TravelManager travelManager)
        {
            var item = MerchandiseGenerator.GenerateItem(ItemCategory.Tool);

            travelManager.AddItem(item);

            var description = "As a sign of good will, the locals make you a present";
            var resultText = StringUtils.BuildResultTextItem(item, true);

            return new GameActionResult(description, resultText);
        }

        private GameActionResult performMakeFriends(TravelManager travelManager)
        {
            travelManager.IncrementStat(StatId.Reputation);

            var description = "You make friends with some of the locals";
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

            var description = "You get involved in a fight and get the worst of it!";
            var resultText = StringUtils.BuildResultTextHealth(-injuryAmount);

            return new GameActionResult(description, resultText);
        }
    }

}
