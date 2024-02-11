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
        private static List<TownTemplate> townTemplates;
        private static List<int> townTemplateWeights;

        private DominionGenerator dominionGenerator;
        private HashSet<string> usedTownNames = new();


        static TownGenerator()
        {
            TextAsset fileObj;

            fileObj = Resources.Load<TextAsset>($"Data/Generators/biomeTransitions");
            biomeTransitions = JsonConvert.DeserializeObject<Dictionary<Biome, Dictionary<Biome, int>>>(fileObj.text);

            fileObj = Resources.Load<TextAsset>($"Data/Generators/townTemplates");
            townTemplates = JsonConvert.DeserializeObject<List<TownTemplate>>(fileObj.text);
            townTemplateWeights = new();
            foreach (var template in townTemplates)
                townTemplateWeights.Add(template.frequency);
        }


        public TownGenerator()
        {
            dominionGenerator = new DominionGenerator();
        }

        public Town GenerateTown(Town lastTown)
        {
            Biome biome;
            Dominion dominion;

            if (lastTown == null)
            {
                biome = RandomUtils.RandomEnum<Biome>();
                dominion = dominionGenerator.GenerateDominion();
            }
            else
            {
                biome = randomBiomeTransition(lastTown.biome);
                var dominionCanPersist = Random.value <= lastTown.dominion.persistence;
                if (dominionCanPersist && lastTown.dominion.nTowns < lastTown.dominion.maxNTowns)
                    dominion = lastTown.dominion;
                else
                    dominion = dominionGenerator.GenerateDominion();
            }

            TownTemplate townTemplate = null;
            while (!isValidTownTemplate(townTemplate, dominion))
                townTemplate = RandomUtils.RandomChooseWeighted(townTemplates, townTemplateWeights);

            string townName;
            do
                townName = FileStringGenerator.Sites.GenerateString();
            while (usedTownNames.Contains(townName));
            usedTownNames.Add(townName);
            var town = new Town(townName);

            town.size = townTemplate.size;
            town.nDestinations = townTemplate.nDestinations;
            town.biome = biome;
            town.dominion = dominion;
            town.dominion.nTowns++;

            assignTownTraits(town, dominion.traits);
            assignBuildings(town, townTemplate);

            town.description = TownDescriptionGenerator.GenerateTownDescription(town);

            return town;
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


        private void assignTownTraits(Town town, HashSet<DominionTrait> traits)
        {
            town.traits = new();

            foreach (var trait in traits)
            {
                if (isValidConstraint(trait, town))
                    town.traits.Add(trait);
            }
        }

        private bool isValidTownTemplate(TownTemplate townTemplate, Dominion dominion)
        {
            if (townTemplate == null)
                return false;

            if (townTemplate.size > dominion.maxTownSize)
                return false;

            return true;
        }

        private bool isValidConstraint(DominionTrait trait, Town town)
        {
            if (trait == DominionTrait.Industrial && town.size == TownSize.Hamlet)
                return false;

            if (trait == DominionTrait.HighCrime && town.size <= TownSize.Town && town.traits.Contains(DominionTrait.Rural))
                return false;

            return true;
        }

        private void assignBuildings(Town town, TownTemplate townTemplate)
        {
            var shopTypes = new List<TownBuilding>() {
                TownBuilding.Bakery,
                TownBuilding.Butchery,
                //TownBuilding.Emporium,
                //TownBuilding.Clothier,
                //TownBuilding.Smith,
                //TownBuilding.Carpenter,
                //TownBuilding.Shoemaker,
            };

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
            //buildings.Add(TownBuilding.Farm);
            //buildings.Add(TownBuilding.Butchery);

            //buildings.Add(TownBuilding.Church);
            //buildings.Add(TownBuilding.Monastery);
            //buildings.Add(TownBuilding.TownHall);
            buildings.Add(TownBuilding.Tavern);
            //buildings.Add(TownBuilding.Library);
            //
            town.buildings = buildings;
        }
    }
}