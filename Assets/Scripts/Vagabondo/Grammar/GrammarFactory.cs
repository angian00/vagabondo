using System.Collections.Generic;

namespace Vagabondo.Grammar
{
    public class GrammarFactory
    {
        public enum GrammarId
        {
            SketchyDeal,
        }

        private static Dictionary<GrammarId, string> filenames = new()
        {
            { GrammarId.SketchyDeal, "sketchyDeal" },
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