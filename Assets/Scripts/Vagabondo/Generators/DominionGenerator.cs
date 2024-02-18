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

        private static List<DominionTemplate> dominionTemplates;
        private static List<int> dominionTemplateWeights;

        private HashSet<string> usedRegionNames = new();


        static DominionGenerator()
        {
            var fileObj = Resources.Load<TextAsset>($"Data/Generators/dominionTemplates");
            dominionTemplates = JsonConvert.DeserializeObject<List<DominionTemplate>>(fileObj.text);
            dominionTemplateWeights = new();
            foreach (var template in dominionTemplates)
                dominionTemplateWeights.Add(template.frequency);
        }



        public Dominion GenerateDominion()
        {
            var dominionTemplate = RandomUtils.RandomChooseWeighted(dominionTemplates, dominionTemplateWeights);
            var regionName = chooseRegionName();

            var dominion = new Dominion(dominionTemplate, regionName);
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


        private static HashSet<TownTrait> randomTraits(Dominion dominion)
        {
            var binaryTraitDistribution = new List<int> { 6, 2, 2 };
            var unaryTraitProbability = 0.2f;

            var traits = new HashSet<TownTrait>();

            var richPoorValues = new List<TownTrait>() {
                    TownTrait.Default,
                    TownTrait.Rich,
                    TownTrait.Poor
                };
            var richPoor = RandomUtils.RandomChooseWeighted(richPoorValues, binaryTraitDistribution);
            if (richPoor != TownTrait.Default)
                traits.Add(richPoor);

            var ruralIndustrialValues = new List<TownTrait>() {
                    TownTrait.Default,
                    TownTrait.Rural,
                    TownTrait.Industrial
                };
            var ruralIndustrial = RandomUtils.RandomChooseWeighted(ruralIndustrialValues, binaryTraitDistribution);
            if (ruralIndustrial != TownTrait.Default)
                traits.Add(ruralIndustrial);

            if (Random.value <= unaryTraitProbability)
                traits.Add(TownTrait.Fanatic);

            if (Random.value <= unaryTraitProbability)
                traits.Add(TownTrait.HighCrime);

            return traits;
        }
    }
}