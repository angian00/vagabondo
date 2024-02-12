using System.Collections.Generic;

namespace Vagabondo.Grammar
{
    public class GrammarFactory
    {
        public enum GrammarId
        {
            SketchyDeal,
            Food,
            TownDescription,
        }

        private static Dictionary<GrammarId, List<string>> grammarFiles = new()
        {
            { GrammarId.SketchyDeal, new List<string>() { "sketchyDeal" }  },
            { GrammarId.Food, new List<string>() {"food" } },
            { GrammarId.TownDescription, new List<string>() {
                "townRoot",
                "townSentenceChildren",
                "townSentenceNature",
                "townSentenceChurch",
                "townSentenceFields",
                "townSentencePond",
                "townStructures",
                "townNouns",
                "townVerbs",
                "townAdjectives",
                }
            },
        };


        private static Dictionary<GrammarId, RichGrammar> cache = new();


        public static RichGrammar GetGrammar(GrammarId grammarId)
        {
            if (!cache.ContainsKey(grammarId))
                cache.Add(grammarId, new RichGrammar(grammarFiles[grammarId]));

            return cache[grammarId];
        }
    }
}