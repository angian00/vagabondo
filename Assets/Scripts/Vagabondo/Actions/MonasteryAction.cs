using System;
using System.Collections.Generic;
using Vagabondo.DataModel;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.Actions
{
    public class MonasteryAction : GameAction
    {
        public MonasteryAction(Town townData) : base(GameActionType.Church, townData)
        {
            this.title = "Visit the monastery";
            this.description = "";
        }

        public override GameActionResult Perform(TravelManager travelManager)
        {
            var effectTypes = new List<GameActionEffectType>() {
                GameActionEffectType.Learn,
                GameActionEffectType.Pray,
                GameActionEffectType.Trade,
                GameActionEffectType.MakeEnemies,
            };

            //TODO: influence result by Religion stat, Exoterism stat, and other factors
            var effectType = RandomUtils.RandomChoose(effectTypes);
            switch (effectType)
            {
                case GameActionEffectType.Learn:
                    return performLearn(travelManager);
                case GameActionEffectType.Pray:
                    return performPray(travelManager);
                case GameActionEffectType.Trade:
                    return performTrade(travelManager);
                case GameActionEffectType.MakeEnemies:
                    return performMakeEnemies(travelManager);
            }

            throw new Exception($"Invalid effectType: {DataUtils.EnumToStr(effectType)}");
        }


        private GameActionResult performLearn(TravelManager travelManager)
        {
            travelManager.IncrementStat(StatId.Languages);

            return new GameActionResult($"You spend some time in the monastery library, consulting venerable old texts");
        }

        private GameActionResult performPray(TravelManager travelManager)
        {
            travelManager.IncrementStat(StatId.Religion);

            return new GameActionResult($"You spend some time in the cloister, praying and meditating");
        }

        private GameActionResult performTrade(TravelManager travelManager)
        {
            var donationAmount = 20;
            travelManager.AddMoney(-donationAmount);
            travelManager.IncrementStat(StatId.Religion);

            return new GameActionResult($"You donate some money for charitable works");
        }

        private GameActionResult performMakeEnemies(TravelManager travelManager)
        {
            travelManager.DecrementStat(StatId.Religion);
            travelManager.DecrementStat(StatId.Religion);

            return new GameActionResult($"You get involved in a theological dispute, and are finally kicked out of the monastery"); //FUTURE: make this a Choice Tree
        }
    }
}
