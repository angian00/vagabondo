using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Vagabondo.Grammar
{
    public class RichGrammarModifiers
    {
        private static Dictionary<string, string> irregularNouns;
        private static Dictionary<string, (string, string)> irregularVerbs;

        static RichGrammarModifiers()
        {
            loadIrregularNouns();
            loadIrregularVerbs();
        }

        private static void loadIrregularNouns()
        {
            var fileObj = Resources.Load<TextAsset>($"Data/Grammars/irregularNouns");
            var lines = fileObj.text.Split("\n");

            irregularNouns = new();

            foreach (var line in lines)
            {
                var tokens = line.Split("|");
                irregularNouns.Add(tokens[0], tokens[1]);
            }
        }

        private static void loadIrregularVerbs()
        {
            var fileObj = Resources.Load<TextAsset>($"Data/Grammars/irregularVerbs");
            var lines = fileObj.text.Split("\n");

            irregularVerbs = new();

            foreach (var line in lines)
            {
                var tokens = line.Split("|");
                irregularVerbs.Add(tokens[0], (tokens[1], tokens[2]));
            }
        }

        public static string applyModifier(string originalText, string modifier)
        {
            if (modifier == "capitalize")
                return applyCapitalize(originalText);
            if (modifier == "capitalizeAll")
                return applyCapitalizeAll(originalText);
            if (modifier == "a")
                return applyA(originalText);
            if (modifier == "s")
                return applyPlural(originalText);
            if (modifier == "ed")
                return applyPastTense(originalText);

            return originalText;
        }

        private static string applyCapitalize(string originalText)
        {
            return originalText.Substring(0, 1).ToUpper() + originalText.Substring(1);
        }

        private static string applyCapitalizeAll(string originalText)
        {
            var tokens = originalText.Split();
            var result = new StringBuilder();
            foreach (var token in tokens)
            {
                if (result.Length > 0)
                    result.Append(" ");
                result.Append(applyCapitalize(token));
            }

            return result.ToString();
        }

        private static string applyA(string originalText)
        {
            if (isVowel(originalText[0], false))
                return "an " + originalText;

            return "a " + originalText;
        }

        private static string applyPlural(string originalText)
        {
            if (irregularNouns.ContainsKey(originalText))
                return irregularNouns[originalText];

            if (originalText.EndsWith("f"))
                return originalText.Substring(0, originalText.Length - 1) + "ves";
            if (originalText.EndsWith("fe"))
                return originalText.Substring(0, originalText.Length - 2) + "ves";

            if (originalText.EndsWith("y") && !isVowel(originalText[originalText.Length - 2]))
                return originalText.Substring(0, originalText.Length - 1) + "ies";

            if (originalText.EndsWith("o") && !isVowel(originalText[originalText.Length - 2]))
                return originalText + "es";

            if (originalText.EndsWith("ss") || originalText.EndsWith("x") ||
                originalText.EndsWith("ch") || originalText.EndsWith("sh"))
                return originalText + "es";

            return originalText + "s";
        }

        private static string applyPastTense(string originalText)
        {
            if (irregularVerbs.ContainsKey(originalText))
                return irregularVerbs[originalText].Item1;

            if (originalText.EndsWith("e"))
                return originalText + "d";

            //doubling final consonant as per britannica.com
            //https://www.britannica.com/dictionary/eb/qa/Doubling-the-final-consonant-before-adding-ed-or-ing#:~:text=To%20know%20when%20to%20double,consonant%2C%20follow%20the%20rules%20below.&text=In%20a%20word%20with%201,the%20final%20syllable%20is%20stressed.
            if ((!isVowel(originalText[originalText.Length - 3])) && isVowel(originalText[originalText.Length - 2]) && !isVowel(originalText[originalText.Length - 1], false)
                && originalText[originalText.Length - 1] != 'w' && originalText[originalText.Length - 1] != 'x')
                //ignoring accent-related subrule
                return originalText + originalText[originalText.Length - 1] + "ed";

            //If a verb ends in consonant and -y, you take off the y and add - ied.
            if (!isVowel(originalText[originalText.Length - 2]) && originalText[originalText.Length - 1] == 'y')
                return originalText.Substring(0, originalText.Length - 1) + "ied";

            return originalText + "ed";
        }

        private static bool isVowel(char c, bool yIsVowel = true)
        {
            return (c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u' || (yIsVowel && c == 'y'));
        }

    }
}