using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using Vagabondo.DataModel;
using Vagabondo.Utils;

namespace Vagabondo.Generators
{
    public class TownGenerator
    {
        private static Dictionary<Biome, Dictionary<Biome, int>> biomeTransitions;

        private DominionGenerator dominionGenerator;
        private HashSet<Dominion> dominions; //TODO: count explored towns by dominion

        static TownGenerator()
        {
            TextAsset fileObj;

            fileObj = Resources.Load<TextAsset>($"Data/Generators/biomeTransitions");
            biomeTransitions = JsonConvert.DeserializeObject<Dictionary<Biome, Dictionary<Biome, int>>>(fileObj.text);

        }

        public TownGenerator()
        {
            dominionGenerator = new DominionGenerator();
        }

        public Town GenerateTown(Town lastTown)
        {
            var townName = FileStringGenerator.Sites.GenerateString();
            var town = new Town(townName);

            TownSize size;
            Biome biome;
            Dominion dominion;
            if (lastTown == null)
            {
                size = RandomUtils.RandomEnum<TownSize>();
                biome = RandomUtils.RandomEnum<Biome>();
                dominion = dominionGenerator.GenerateDominion();
            }
            else
            {
                size = randomSizeTransition(lastTown.size);
                biome = randomBiomeTransition(lastTown.biome);
                dominion = randomDominionTransition(lastTown.dominion);
            }

            town.size = size;
            town.biome = biome;
            town.dominion = dominion;

            //town.description = "";
            //for (int i = 0; i < 10; i++)
            //    town.description += $"TODO: {townName} description  ";
            town.buildings = randomBuildings(biome, size);

            town.description = TownDescriptionGenerator.GenerateTownDescription(town);

            return town;
        }

        private TownSize randomSizeTransition(TownSize lastSize)
        {
            while (true)
            {
                var newSize = EnumGenerator.TownSize.GenerateValue();
                if (!(lastSize == TownSize.City && (newSize == TownSize.City)))
                    return newSize;
            }
        }

        private Biome randomBiomeTransition(Biome lastBiome)
        {
            var transitionWeights = biomeTransitions[lastBiome];
            var values = new List<Biome>();
            var weights = new List<int>();

            foreach (var transitionWeight in transitionWeights)
            {
                values.Add(transitionWeight.Key);
                weights.Add(transitionWeight.Value);
            }

            return RandomUtils.RandomChooseWeighted(values, weights);
        }

        private Dominion randomDominionTransition(Dominion lastDominion)
        {
            if (UnityEngine.Random.value <= lastDominion.type.permanence)
                return lastDominion;
            else
                return dominionGenerator.GenerateDominion();
        }


        private static HashSet<TownBuilding> randomBuildings(Biome biome, TownSize size)
        {
            int maxNBuildings = -1;

            switch (size)
            {
                case TownSize.Hamlet:
                    maxNBuildings = 1;
                    break;
                case TownSize.Village:
                    maxNBuildings = 2;
                    break;
                case TownSize.Town:
                    maxNBuildings = 3;
                    break;
                case TownSize.City:
                    maxNBuildings = 5;
                    break;
            }

            var buildingTypes = DataUtils.EnumToList<TownBuilding>();
            RandomUtils.Shuffle(buildingTypes);

            var buildings = new HashSet<TownBuilding>();
            foreach (var buildingType in buildingTypes)
            {
                if (buildings.Count >= maxNBuildings)
                    break;

                buildings.Add(buildingType);
            }

            return buildings;
        }
    }
}