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


        public static SubstitutionGrammar CreateGrammar(GrammarId grammarId)
        {
            if (!cache.ContainsKey(grammarId))
                cache.Add(grammarId, SubstitutionGrammar.Load(filenames[grammarId]));

            return cache[grammarId];
        }
    }
}