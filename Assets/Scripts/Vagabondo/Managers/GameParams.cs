namespace Vagabondo.Managers
{
    public class GameParams
    {
        // traveler data initialization
        public const int startMoney = 100;
        public const int startHealth = 10;

        // town generation stats
        public const int nMaxHints = 3;

        // action generation probabilities
        //public const float questActionProbability = 0.2f;
        public const float questActionProbability = 0.8f;
        public const float buildingActionProbability = 0.7f;
        public const float eventActionProbability = 0.7f;

        //food calculations
        public const float preparationQualityWeight = 2.0f;
        public const float preparationValueMultiplier = 1.2f;

    }
}