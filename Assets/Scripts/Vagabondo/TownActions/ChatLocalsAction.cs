using System;
using System.Collections.Generic;
using Vagabondo.DataModel;
using Vagabondo.Generators;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.TownActions
{
    public class ChatLocalsAction : TownAction
    {
        public ChatLocalsAction(Town townData) : base(GameActionType.ChatLocals, townData)
        {
            this.title = "Chat with the local folk";
            this.description = "";
        }

        public override bool isEventAction() => true;

        public override TownActionResult Perform(TravelManager travelManager)
        {
            var effectTypes = new List<TownActionEffectType>() {
                TownActionEffectType.Learn,
                //FUTURE: learn recipe
                TownActionEffectType.ReceiveItem,
                TownActionEffectType.MakeFriends,
                TownActionEffectType.MakeEnemies,
                TownActionEffectType.Injury,
            };

            //TODO: result influenced by Knowledge.Diplomacy
            var effectType = RandomUtils.RandomChoose(effectTypes);
            switch (effectType)
            {
                case TownActionEffectType.Learn:
                    return performLearn(travelManager);
                case TownActionEffectType.ReceiveItem:
                    return performReceiveItem(travelManager);
                case TownActionEffectType.MakeFriends:
                    return performMakeFriends(travelManager);
                case TownActionEffectType.MakeEnemies:
                    return performMakeEnemies(travelManager);
                case TownActionEffectType.Injury:
                    return performInjury(travelManager);
            }

            throw new Exception($"Invalid effectType: {DataUtils.EnumToStr(effectType)}");
        }

        private TownActionResult performLearn(TravelManager travelManager)
        {
            travelManager.IncrementStat(StatId.Diplomacy);

            var description = "You learn some interesting facts about the local people";
            var resultText = StringUtils.BuildResultTextStat(StatId.Diplomacy, 1);

            return new TownActionResult(description, resultText);
        }

        private TownActionResult performReceiveItem(TravelManager travelManager)
        {
            var item = MerchandiseGenerator.GenerateItem(ItemCategory.Tool);

            travelManager.AddItem(item);

            var description = "As a sign of good will, the locals make you a present";
            var resultText = StringUtils.BuildResultTextItem(item, true);

            return new TownActionResult(description, resultText);
        }

        private TownActionResult performMakeFriends(TravelManager travelManager)
        {
            travelManager.IncrementStat(StatId.Reputation);

            var description = "You make friends with some of the locals";
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

            var description = "You get involved in a fight and get the worst of it!";
            var resultText = StringUtils.BuildResultTextHealth(-injuryAmount);

            return new TownActionResult(description, resultText);
        }
    }

}
