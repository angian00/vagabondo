using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using Vagabondo.DataModel;
using Vagabondo.Utils;

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

            return new Dominion(dominionType, $"{dominionType.name} of {regionName}");
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
    }
}