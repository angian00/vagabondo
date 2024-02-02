using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;


namespace Vagabondo.Utils
{
    public class RandomUtils
    {
        public static void Shuffle<T>(List<T> values)
        {
            // Fisher-Yates shuffle algorithm
            int n = values.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                var value = values[k];
                values[k] = values[n];
                values[n] = value;
            }
        }

        public static T RandomChoose<T>(List<T> values)
        {
            return values[Random.Range(0, values.Count)];
        }

        public static T RandomChooseWeighted<T>(Dictionary<T, int> weightedValues)
        {
            var values = new List<T>(weightedValues.Keys);
            var weights = new List<int>();
            foreach (var value in values)
                weights.Add(weightedValues[value]);

            return RandomChooseWeighted(values, weights);
        }

        public static T RandomChooseWeighted<T>(List<T> values, List<int> weights)
        {
            var cumWeights = new List<int>();
            int currCum = 0;
            foreach (var currWeight in weights)
            {
                currCum += currWeight;
                cumWeights.Add(currCum);
            }

            int chosenCumWeight = Random.Range(0, currCum + 1);

            var chosenIndex = cumWeights.BinarySearch(chosenCumWeight);
            if (chosenIndex < 0)
            {
                //as per List.BinarySearch docs: the complementof the next index is returned
                chosenIndex = ~chosenIndex;
            }

            return values[chosenIndex];
        }


        public static bool RandomBool()
        {
            if (Random.value >= 0.5)
                return true;
            return false;
        }

        public static T RandomEnum<T>() where T : Enum
        {
            var enumValues = Enum.GetValues(typeof(T));
            return (T)enumValues.GetValue(Random.Range(0, enumValues.Length));
        }
    }

}
