using System.Collections.Generic;

namespace Vagabondo
{
    public class EventManager
    {
        public delegate void OnTownChanged(TownData newTown);
        public static event OnTownChanged onTownChanged;

        public delegate void OnDestinationsChanged(List<TownData> newDestinations);
        public static event OnDestinationsChanged onDestinationsChanged;

        public delegate void OnTravelerChanged(TravelerData travelerData);
        public static event OnTravelerChanged onTravelerChanged;

        public delegate void OnActionPerformed(string actionResult);
        public static event OnActionPerformed onActionPerformed;


        public static void PublishTownChanged(TownData newTown)
        {
            if (onTownChanged != null)
                onTownChanged(newTown);
        }

        public static void PublishDestinationsChanged(List<TownData> newDestinations)
        {
            if (onDestinationsChanged != null)
                onDestinationsChanged(newDestinations);
        }

        public static void PublishTravelerChanged(TravelerData travelerData)
        {
            if (onTravelerChanged != null)
                onTravelerChanged(travelerData);
        }

        public static void PublishActionPerformed(string actionResult)
        {
            if (onActionPerformed != null)
                onActionPerformed(actionResult);
        }
    }
}