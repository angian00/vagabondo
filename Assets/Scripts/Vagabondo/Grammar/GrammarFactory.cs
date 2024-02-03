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

        private static Dictionary<GrammarId, string> filenames = new()
        {
            { GrammarId.SketchyDeal, "sketchyDeal" },
            { GrammarId.Food, "food" },
            { GrammarId.TownDescription, "townDescription" },
        };


        private static Dictionary<GrammarId, RichGrammar> cache = new();


        public static RichGrammar GetGrammar(GrammarId grammarId)
        {
            if (!cache.ContainsKey(grammarId))
                cache.Add(grammarId, new RichGrammar(filenames[grammarId]));

            return cache[grammarId];
        }
    }
}