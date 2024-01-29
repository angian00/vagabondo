using System;
using System.Collections.Generic;
using Vagabondo.Utils;

namespace Vagabondo.Generators
{
    public class DominionGenerator
    {
        private static int nMaxRegions = 80;

        private static List<string> dominionTypes = new List<string>()
        {
            "Empire",
            "Kingdom",
            "Principate",
            "Archduchy",
            "Duchy",
            "County",
            "Marquisdom",
            "Barony",

            "Free State",
        };


        private HashSet<string> usedRegionNames = new();

        public DominionData GenerateDominion()
        {
            var dominionType = chooseDominionType();
            var regionName = chooseRegionName();

            return new DominionData($"{dominionType} of {regionName}");
        }

        private string chooseDominionType()
        {
            return RandomUtils.RandomChoose<string>(dominionTypes);
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