using System.Collections.Generic;
using Vagabondo.Actions;
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

        public delegate void OnActionPerformed(GameActionResult actionResult);
        public static event OnActionPerformed onActionPerformed;


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

        public static void PublishActionPerformed(GameActionResult actionResult)
        {
            if (onActionPerformed != null)
                onActionPerformed(actionResult);
        }
    }
}