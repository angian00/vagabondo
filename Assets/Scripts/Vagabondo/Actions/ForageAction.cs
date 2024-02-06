using System;
using Vagabondo.DataModel;
using Vagabondo.Managers;

namespace Vagabondo.Actions
{
    public class ForageAction : GameAction
    {
        public Biome biome;

        public ForageAction(Biome biome) : base(GameActionType.Forage)
        {
            this.biome = biome;
            this.title = "Forage";
            this.description = "Walk around the countryside looking for useful herbs";
        }

        public override GameActionResult Perform(TravelManager travelManager)
        {
            //var merchItem = MerchandiseGenerator.GenerateHerb(biome);
            //travelManager.AddMerchandiseItem(merchItem);

            //return new TextActionResult($"You gather some <style=C1>{merchItem.text}</style>");
            throw new NotImplementedException();
        }
    }
}
