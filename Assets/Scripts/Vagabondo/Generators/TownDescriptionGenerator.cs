using System.Collections.Generic;
using Vagabondo.DataModel;
using Vagabondo.Grammar;
using Vagabondo.Utils;

namespace Vagabondo.Generators
{
    public class TownDescriptionGenerator
    {
        public static string GenerateTownDescription(Town town)
        {
            var grammar = GrammarFactory.GetGrammar(GrammarFactory.GrammarId.TownDescription);
            var inputTags = new HashSet<string>();

            var sentences = new List<string>();

            var sizeStr = DataUtils.EnumToStr(town.size).ToLower();
            inputTags.Add(sizeStr);
            //sentences.Add(grammar.GenerateText(sizeRuleId));

            //return string.Join("\n", sentences.ToArray());

            return grammar.GenerateText(inputTags: inputTags);
        }
    }
}