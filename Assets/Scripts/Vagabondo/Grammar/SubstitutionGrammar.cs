using System.Collections.Generic;

namespace Vagabondo.Grammar
{
    public interface SubstitutionGrammar
    {
        public string GenerateText(string startRule = null, HashSet<string> tags = null);

    }
}