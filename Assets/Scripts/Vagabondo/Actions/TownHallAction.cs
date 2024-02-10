using System;
using System.Collections.Generic;
using Vagabondo.DataModel;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.Actions
{
    public class TownHallAction : GameAction
    {
        public TownHallAction(Town townData) : base(GameActionType.TownHall, townData)
        {
            this.title = "Go to the Town Hall";
            this.description = "";
        }

        public override GameActionResult Perform(TravelManager travelManager)
        {
            var effectTypes = new List<GameActionEffectType>() {
                GameActionEffectType.GiveItem,
                GameActionEffectType.MakeFriends,
                //GameActionEffectType.MakeEnemies,
                GameActionEffectType.LoseMoney,
            };

            //TODO: check if you can spend money to make friends

            //DEBUG
            var effectType = RandomUtils.RandomChoose(effectTypes);
            switch (effectType)
            {
                case GameActionEffectType.GiveItem:
                    return performGiveItem(travelManager);
                case GameActionEffectType.MakeFriends:
                    return performMakeFriends(travelManager);
                case GameActionEffectType.LoseMoney:
                    return performLoseMoney(travelManager);
            }

            throw new Exception($"Invalid effectType: {DataUtils.EnumToStr(effectType)}");
        }


        private GameActionResult performGiveItem(TravelManager travelManager)
        {
            travelManager.IncrementStat(StatId.Reputation);
            travelManager.RemoveAnyItem();
            return new GameActionResult($"You have the opportunity to gift something to the town mayor as a sign of good will");
            //return new GiftActionResult($"You have the opportunity to gift something to the town mayor as a sign of good will");
        }

        private GameActionResult performMakeFriends(TravelManager travelManager)
        {
            travelManager.IncrementStat(StatId.Reputation);
            return new GameActionResult($"You spend some time discussing local politics with the town bosses");
        }

        private GameActionResult performLoseMoney(TravelManager travelManager)
        {
            var taxAmount = 20; //TODO: check current traveler money

            travelManager.AddMoney(-taxAmount);
            return new GameActionResult($"You are politely invited to pay your customs tax");
        }
    }
}
