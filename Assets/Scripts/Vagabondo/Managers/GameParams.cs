using Newtonsoft.Json;
using UnityEngine;

namespace Vagabondo.Managers
{
    public class GameParams
    {
        public static GameParams Instance { get; private set; }

        static GameParams()
        {
            var fileObj = Resources.Load<TextAsset>($"Config/gameParams");
            Instance = JsonConvert.DeserializeObject<GameParams>(fileObj.text);
            Debug.Log($"startMoney: {Instance.startMoney}");
        }

        [JsonProperty]
        public readonly int startMoney;
        [JsonProperty]
        public readonly int startHealth;
        [JsonProperty]
        public readonly int startNutrition;

        [JsonProperty]
        public readonly int nMaxHints;

        [JsonProperty]
        public readonly float questActionProbability;
        [JsonProperty]
        public readonly float buildingActionProbability;
        [JsonProperty]
        public readonly float eventActionProbability;

        [JsonProperty]
        public readonly float preparationQualityWeight;
        [JsonProperty]
        public readonly float preparationValueMultiplier;

        [JsonProperty]
        public readonly int travelNutritionCost;
    }
}