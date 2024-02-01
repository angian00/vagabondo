using Vagabondo.DataModel;
using Vagabondo.Generators;
using Vagabondo.Managers;

namespace Vagabondo.Actions
{
    public class FoodGiftAction : GameAction
    {
        private Town townData;
        public FoodGiftAction(Town townData) : base(GameActionType.SketchyDeal)
        {
            this.townData = townData;
            this.title = "Chat with the local folk";
            this.description = "Look for a bargain deal in the seediest part of the town";
        }

        public override bool CanPerform(Traveler travelerData)
        {
            return true;
        }

        public override string GetCantPerformMessage()
        {
            return "";
        }

        public override GameActionResult Perform(TravelManager travelManager)
        {
            var foodItem = MerchandiseGenerator.GenerateFood(townData);
            travelManager.AddMerchandiseItem(foodItem);

            //TODO: item discovery UI
            var resultText = $"You socialize with the locals and they gift you with <style=C1>{foodItem.text}</style>";

            return new ItemAcquiredActionResult(resultText, foodItem);
        }
    }

}