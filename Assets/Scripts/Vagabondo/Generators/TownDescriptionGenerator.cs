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
            var variables = new Dictionary<string, string>();

            var sentences = new List<string>();

            var sizeStr = DataUtils.EnumToStr(town.size).ToLower();
            inputTags.Add(sizeStr);
            variables.Add("townSize", sizeStr);

            //return string.Join("\n", sentences.ToArray());

            return grammar.GenerateText(inputTags: inputTags, variables: variables);
        }
    }
}