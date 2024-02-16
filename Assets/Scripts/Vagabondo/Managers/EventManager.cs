using System.Collections.Generic;
using Vagabondo.TownActions;
using Vagabondo.DataModel;

namespace Vagabondo.Managers
{
    public class EventManager
    {
        public delegate void OnTownChanged(Town newTown);
        public static event OnTownChanged onTownChanged;

        public delegate void OnDestinationsChanged(List<Town> newDestinations);
        public static event OnDestinationsChanged onDestinationsChanged;

        public delegate void OnTravelerChanged(Traveler travelerData);
        public static event OnTravelerChanged onTravelerChanged;

        public delegate void OnActionPerformed(TownActionResult actionResult);
        public static event OnActionPerformed onActionPerformed;

        public delegate void OnGameOver();
        public static event OnGameOver onGameOver;


        public static void PublishTownChanged(Town newTown)
        {
            if (onTownChanged != null)
                onTownChanged(newTown);
        }

        public static void PublishDestinationsChanged(List<Town> newDestinations)
        {
            if (onDestinationsChanged != null)
                onDestinationsChanged(newDestinations);
        }

        public static void PublishTravelerChanged(Traveler travelerData)
        {
            if (onTravelerChanged != null)
                onTravelerChanged(travelerData);
        }

        public static void PublishActionPerformed(TownActionResult actionResult)
        {
            if (onActionPerformed != null)
                onActionPerformed(actionResult);
        }

        public static void PublishGameOver()
        {
            if (onGameOver != null)
                onGameOver();
        }
    }
}