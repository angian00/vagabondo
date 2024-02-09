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
        private static Dictionary<TownSize, TownTemplate> townTemplates;

        private DominionGenerator dominionGenerator;
        private HashSet<Dominion> dominions; //TODO: count explored towns by dominion

        static TownGenerator()
        {
            TextAsset fileObj;

            fileObj = Resources.Load<TextAsset>($"Data/Generators/biomeTransitions");
            biomeTransitions = JsonConvert.DeserializeObject<Dictionary<Biome, Dictionary<Biome, int>>>(fileObj.text);

            fileObj = Resources.Load<TextAsset>($"Data/Generators/townTemplates");
            townTemplates = JsonConvert.DeserializeObject<Dictionary<TownSize, TownTemplate>>(fileObj.text);
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

            assignTownTraits(town, dominion.traits);

            assignRandomBuildings(town);

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

        private void assignTownTraits(Town town, HashSet<DominionTrait> traits)
        {
            town.traits = new();

            foreach (var trait in traits)
            {
                //if (trait == DominionTrait.Industrial && town.size == TownSize.Hamlet)
                //    continue;

                //if (trait == DominionTrait.HighCrime && town.size <= TownSize.Town && traits.Contains(DominionTrait.Rural))
                //    continue;

                town.traits.Add(trait);
            }
        }

        private static void assignRandomBuildings(Town town)
        {
            var shopTypes = new List<TownBuilding>() {
                TownBuilding.Bakery,
                TownBuilding.Butchery,
                //TownBuilding.Clothier,
                //TownBuilding.Smith,
                //TownBuilding.Carpenter,
                //TownBuilding.Shoemaker,
            };

            var townTemplate = townTemplates[town.size];

            var buildingInfos = townTemplate.buildings;
            RandomUtils.Shuffle(buildingInfos);

            var buildings = new HashSet<TownBuilding>();
            foreach (var buildingInfo in buildingInfos)
            {
                if (buildings.Count >= townTemplate.nMaxBuildings)
                    break;

                if (buildingInfo.traitNeeded != DominionTrait.Default && !town.traits.Contains(buildingInfo.traitNeeded))
                    continue;

                if (buildingInfo.traitExcluded != DominionTrait.Default && town.traits.Contains(buildingInfo.traitExcluded))
                    continue;

                if (Random.value > buildingInfo.probability)
                    continue;

                var buildingType = buildingInfo.buildingType;
                if (buildingType == TownBuilding.ShopGeneric)
                    buildingType = RandomUtils.RandomChoose(shopTypes);

                buildings.Add(buildingType);
            }

            //DEBUG
            //buildings.Add(TownBuilding.Tavern);
            //
            town.buildings = buildings;
        }
    }
}