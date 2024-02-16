using System;
using System.Collections.Generic;
using Vagabondo.DataModel;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.TownActions
{
    public class MonasteryAction : TownAction
    {
        public MonasteryAction(Town townData) : base(GameActionType.Church, townData)
        {
            this.title = "Visit the monastery";
            this.description = "";
        }

        public override bool isBuildingAction() => true;

        public override TownActionResult Perform(TravelManager travelManager)
        {
            var effectTypes = new List<TownActionEffectType>() {
                TownActionEffectType.Learn,
                TownActionEffectType.Pray,
                TownActionEffectType.Trade,
                TownActionEffectType.MakeEnemies,
            };

            //TODO: influence result by Religion stat, Exoterism stat, and other factors
            var effectType = RandomUtils.RandomChoose(effectTypes);
            switch (effectType)
            {
                case TownActionEffectType.Learn:
                    return performLearn(travelManager);
                case TownActionEffectType.Pray:
                    return performPray(travelManager);
                case TownActionEffectType.Trade:
                    return performTrade(travelManager);
                case TownActionEffectType.MakeEnemies:
                    return performMakeEnemies(travelManager);
            }

            throw new Exception($"Invalid effectType: {DataUtils.EnumToStr(effectType)}");
        }


        private TownActionResult performLearn(TravelManager travelManager)
        {
            travelManager.IncrementStat(StatId.Languages);

            var description = "You spend some time in the monastery library, consulting venerable old texts";
            var resultText = StringUtils.BuildResultTextStat(StatId.Languages, 1);

            return new TownActionResult(description, resultText);
        }

        private TownActionResult performPray(TravelManager travelManager)
        {
            travelManager.IncrementStat(StatId.Religion);

            var description = "You spend some time in the cloister, praying and meditating";
            var resultText = StringUtils.BuildResultTextStat(StatId.Religion, 1);

            return new TownActionResult(description, resultText);
        }

        private TownActionResult performTrade(TravelManager travelManager)
        {
            var donationAmount = 40;
            string description;
            string resultText;

            if (travelManager.travelerData.money < donationAmount)
            {
                travelManager.DecrementStat(StatId.Religion);

                description = "The monks ask you for a substantial donation, but you don't have enough money for their liking";
                resultText = StringUtils.BuildResultTextStat(StatId.Religion, -1);

                return new TownActionResult(description, resultText);
            }

            travelManager.AddMoney(-donationAmount);
            travelManager.IncrementStat(StatId.Religion);
            travelManager.IncrementStat(StatId.Religion);

            description = "You donate some money for charitable works";
            resultText = StringUtils.BuildResultTextMoney(-donationAmount)
                + "\n\n" + StringUtils.BuildResultTextStat(StatId.Religion, 2);

            return new TownActionResult(description, resultText);
        }

        private TownActionResult performMakeEnemies(TravelManager travelManager)
        {
            travelManager.DecrementStat(StatId.Religion);
            travelManager.DecrementStat(StatId.Religion);

            //FUTURE: make this a Choice Tree
            var description = "You get involved in a theological dispute, and are kicked out of the monastery!";
            var resultText = StringUtils.BuildResultTextStat(StatId.Religion, -2);

            return new TownActionResult(description, resultText);
        }
    }
}
