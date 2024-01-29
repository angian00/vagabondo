using System.Collections.Generic;
using Vagabondo.DataModel;
using Vagabondo.Utils;

namespace Vagabondo.Generators
{
    public class TownGenerator
    {
        private static Dictionary<Biome, Dictionary<Biome, int>> biomeTransitions;

        private DominionGenerator dominionGenerator;
        private HashSet<Dominion> dominions;

        static TownGenerator()
        {
            initBiomeTransitions();
        }

        private static void initBiomeTransitions()
        {
            biomeTransitions = new Dictionary<Biome, Dictionary<Biome, int>>();

            biomeTransitions.Add(Biome.Forest, new Dictionary<Biome, int>() {
                { Biome.Forest, 40 },
                { Biome.Plains, 20 },
                { Biome.Hills, 20 },
                { Biome.Mountains, 10 },
                { Biome.Lake, 10 },
            });

            biomeTransitions.Add(Biome.Plains, new Dictionary<Biome, int>() {
                { Biome.Forest, 30 },
                { Biome.Plains, 40 },
                { Biome.Hills, 20 },
                { Biome.Mountains, 0 },
                { Biome.Lake, 10 },
            });

            biomeTransitions.Add(Biome.Hills, new Dictionary<Biome, int>() {
                { Biome.Forest, 20 },
                { Biome.Plains, 20 },
                { Biome.Hills, 30 },
                { Biome.Mountains, 20 },
                { Biome.Lake, 10 },
            });

            biomeTransitions.Add(Biome.Mountains, new Dictionary<Biome, int>() {
                { Biome.Forest, 20 },
                { Biome.Plains, 0 },
                { Biome.Hills, 30 },
                { Biome.Mountains, 40 },
                { Biome.Lake, 10 },
            });

            biomeTransitions.Add(Biome.Lake, new Dictionary<Biome, int>() {
                { Biome.Forest, 40 },
                { Biome.Plains, 20 },
                { Biome.Hills, 40 },
                { Biome.Mountains, 0 },
                { Biome.Lake, 0 },
            });

        }

        public TownGenerator()
        {
            dominionGenerator = new DominionGenerator();
        }

        public Town GenerateTownData(Town lastTown)
        {
            var townName = FileStringGenerator.Sites.GenerateString();
            var townData = new Town(townName);

            Biome biome;
            if (lastTown == null)
                biome = randomBiome();
            else
                biome = randomBiome(lastTown.biome);
            townData.biome = biome;

            Dominion dominion = dominionGenerator.GenerateDominion(); //FUTURE: dominion persistence logic
            townData.dominion = dominion;

            townData.description = "";
            for (int i = 0; i < 10; i++)
                townData.description += $"TODO: {townName} description  "; //FUTURE: generate description from grammar

            return townData;
        }


        private static Biome randomBiome()
        {
            return RandomUtils.RandomEnum<Biome>();
        }

        private static Biome randomBiome(Biome lastBiome)
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

    }
}