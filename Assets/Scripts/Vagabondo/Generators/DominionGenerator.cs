using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using Vagabondo.DataModel;
using Vagabondo.Utils;
using Random = UnityEngine.Random;

namespace Vagabondo.Generators
{
    public class DominionGenerator
    {
        private static int nMaxRegions = 80;

        private static List<DominionType> dominionTypes;

        private HashSet<string> usedRegionNames = new();


        static DominionGenerator()
        {
            var fileObj = Resources.Load<TextAsset>($"Data/Generators/dominionTypes");
            dominionTypes = JsonConvert.DeserializeObject<List<DominionType>>(fileObj.text);
        }


        public Dominion GenerateDominion()
        {
            var dominionType = RandomUtils.RandomChoose(dominionTypes);
            var regionName = chooseRegionName();

            var dominion = new Dominion(dominionType, $"{dominionType.name} of {regionName}");
            dominion.traits = randomTraits(dominion);

            return dominion;
        }


        private string chooseRegionName()
        {
            if (usedRegionNames.Count > nMaxRegions)
                throw new Exception("No more region names available");

            string regionName;
            do
            {
                regionName = FileStringGenerator.Regions.GenerateString();
            } while (usedRegionNames.Contains(regionName));

            usedRegionNames.Add(regionName);
            return regionName;
        }


        private static HashSet<DominionTrait> randomTraits(Dominion dominion)
        {
            var binaryTraitDistribution = new List<int> { 6, 2, 2 };
            var unaryTraitProbability = 0.2f;

            var traits = new HashSet<DominionTrait>();

            var richPoorValues = new List<DominionTrait>() {
                    DominionTrait.Default,
                    DominionTrait.Rich,
                    DominionTrait.Poor
                };
            var richPoor = RandomUtils.RandomChooseWeighted(richPoorValues, binaryTraitDistribution);
            if (richPoor != DominionTrait.Default)
                traits.Add(richPoor);

            var ruralIndustrialValues = new List<DominionTrait>() {
                    DominionTrait.Default,
                    DominionTrait.Rural,
                    DominionTrait.Industrial
                };
            var ruralIndustrial = RandomUtils.RandomChooseWeighted(ruralIndustrialValues, binaryTraitDistribution);
            if (ruralIndustrial != DominionTrait.Default)
                traits.Add(ruralIndustrial);

            if (Random.value <= unaryTraitProbability)
                traits.Add(DominionTrait.Fanatic);

            if (Random.value <= unaryTraitProbability)
                traits.Add(DominionTrait.HighCrime);

            return traits;
        }
    }
}