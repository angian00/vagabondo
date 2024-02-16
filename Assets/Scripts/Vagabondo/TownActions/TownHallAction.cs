using System;
using System.Collections.Generic;
using Vagabondo.DataModel;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.TownActions
{
    public class TownHallAction : TownAction
    {
        public TownHallAction(Town townData) : base(GameActionType.TownHall, townData)
        {
            this.title = "Go to the Town Hall";
            this.description = "";
        }

        public override bool isBuildingAction() => true;

        public override TownActionResult Perform(TravelManager travelManager)
        {
            var effectTypes = new List<TownActionEffectType>() {
                TownActionEffectType.GiveItem,
                TownActionEffectType.MakeFriends,
                //GameActionEffectType.MakeEnemies,
                TownActionEffectType.LoseMoney,
            };

            var effectType = RandomUtils.RandomChoose(effectTypes);
            switch (effectType)
            {
                case TownActionEffectType.GiveItem:
                    return performGiveItem(travelManager);
                case TownActionEffectType.MakeFriends:
                    return performMakeFriends(travelManager);
                case TownActionEffectType.LoseMoney:
                    return performLoseMoney(travelManager);
            }

            throw new Exception($"Invalid effectType: {DataUtils.EnumToStr(effectType)}");
        }


        private TownActionResult performGiveItem(TravelManager travelManager)
        {
            var item = travelManager.RemoveAnyItem();
            travelManager.IncrementStat(StatId.Reputation);

            var description = "You have the opportunity to gift something to the town mayor as a sign of good will";
            var resultText = StringUtils.BuildResultTextItem(item, false)
                + "\n\n" + StringUtils.BuildResultTextStat(StatId.Reputation, 1);

            return new TownActionResult(description, resultText);
        }

        private TownActionResult performMakeFriends(TravelManager travelManager)
        {
            travelManager.IncrementStat(StatId.Reputation);

            var description = "You spend some time discussing local politics with the town bosses";
            var resultText = StringUtils.BuildResultTextStat(StatId.Reputation, 1);

            return new TownActionResult(description, resultText);
        }

        private TownActionResult performLoseMoney(TravelManager travelManager)
        {
            const int maxTaxAmount = 30;
            string description;
            string resultText;

            var taxAmount = UnityEngine.Random.Range(1, maxTaxAmount);
            if (travelManager.travelerData.money < taxAmount)
            {
                taxAmount = travelManager.travelerData.money;
                travelManager.AddMoney(-taxAmount);
                travelManager.DecrementStat(StatId.Reputation);
                travelManager.DecrementStat(StatId.Reputation);

                description = "You are politely invited to pay your customs tax" +
                    " Since you don't have enough money, they don't look so polite anymore";
                resultText = StringUtils.BuildResultTextMoney(-taxAmount)
                    + "\n\n" + StringUtils.BuildResultTextStat(StatId.Reputation, -2);

                return new TownActionResult(description, resultText);
            }

            travelManager.AddMoney(-taxAmount);

            description = "You are politely invited to pay your customs tax";
            resultText = StringUtils.BuildResultTextMoney(-taxAmount);

            return new TownActionResult(description, resultText);
        }
    }
}
