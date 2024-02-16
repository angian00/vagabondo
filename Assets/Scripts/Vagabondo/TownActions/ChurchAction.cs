using System;
using System.Collections.Generic;
using Vagabondo.DataModel;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.TownActions
{
    public class ChurchAction : TownAction
    {
        public ChurchAction(Town townData) : base(GameActionType.Church, townData)
        {
            this.title = "Visit the church";
            this.description = "";
        }

        public override bool isBuildingAction() => true;

        public override TownActionResult Perform(TravelManager travelManager)
        {
            var effectTypes = new List<TownActionEffectType>() {
                TownActionEffectType.Learn,
                TownActionEffectType.Trade,
            };

            //TODO: influence result by Religion stat
            var effectType = RandomUtils.RandomChoose(effectTypes);
            switch (effectType)
            {
                case TownActionEffectType.Learn:
                    return performLearn(travelManager);
                case TownActionEffectType.Trade:
                    return performTrade(travelManager);
            }

            throw new Exception($"Invalid effectType: {DataUtils.EnumToStr(effectType)}");
        }


        private TownActionResult performLearn(TravelManager travelManager)
        {
            travelManager.IncrementStat(StatId.Religion);

            var description = "You spend some time praying and meditating";
            var resultText = StringUtils.BuildResultTextStat(StatId.Religion, 1);

            return new TownActionResult(description, resultText);
        }

        private TownActionResult performTrade(TravelManager travelManager)
        {
            var donationAmount = 20;
            string description;
            string resultText;

            if (travelManager.travelerData.money < donationAmount)
            {
                travelManager.DecrementStat(StatId.Religion);

                description = "The priests ask you for a substantial donation, but you don't have enough money for their liking";
                resultText = StringUtils.BuildResultTextStat(StatId.Religion, -1);

                return new TownActionResult(description, resultText);
            }

            travelManager.AddMoney(-donationAmount);
            travelManager.IncrementStat(StatId.Religion);

            description = "You donate some money for charitable works";
            resultText = StringUtils.BuildResultTextMoney(-donationAmount)
                            + "\n\n" + StringUtils.BuildResultTextStat(StatId.Religion, 1);

            return new TownActionResult(description, resultText);
        }
    }
}
