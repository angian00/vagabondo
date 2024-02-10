using System;
using System.Collections.Generic;
using Vagabondo.DataModel;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.Actions
{
    public class ChurchAction : GameAction
    {
        public ChurchAction(Town townData) : base(GameActionType.Church, townData)
        {
            this.title = "Visit the church";
            this.description = "";
        }

        public override GameActionResult Perform(TravelManager travelManager)
        {
            var effectTypes = new List<GameActionEffectType>() {
                GameActionEffectType.Learn,
                GameActionEffectType.Trade,
            };

            //TODO: influence result by Religion stat
            var effectType = RandomUtils.RandomChoose(effectTypes);
            switch (effectType)
            {
                case GameActionEffectType.Learn:
                    return performLearn(travelManager);
                case GameActionEffectType.Trade:
                    return performTrade(travelManager);
            }

            throw new Exception($"Invalid effectType: {DataUtils.EnumToStr(effectType)}");
        }


        private GameActionResult performLearn(TravelManager travelManager)
        {
            travelManager.IncrementStat(StatId.Religion);

            return new GameActionResult($"You spend some time praying and meditating");
        }

        private GameActionResult performTrade(TravelManager travelManager)
        {
            var donationAmount = 20;
            travelManager.AddMoney(-donationAmount);
            travelManager.IncrementStat(StatId.Religion);

            return new GameActionResult($"You donate some money for charitable works");
        }
    }
}
