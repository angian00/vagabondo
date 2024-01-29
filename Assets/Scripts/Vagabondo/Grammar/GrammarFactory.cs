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


        private static Dictionary<GrammarId, SubstitutionGrammar> cache = new();


        public static SubstitutionGrammar GetGrammar(GrammarId grammarId)
        {
            if (!cache.ContainsKey(grammarId))
                cache.Add(grammarId, SubstitutionGrammar.FromFile(filenames[grammarId]));

            return cache[grammarId];
        }
    }
}