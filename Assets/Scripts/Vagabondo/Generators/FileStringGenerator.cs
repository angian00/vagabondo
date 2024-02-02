using System.Collections.Generic;
using UnityEngine;
using Vagabondo.Utils;

namespace Vagabondo.Generators
{
    public class FileStringGenerator : StringGenerator
    {
        protected List<string> _names = new();
        protected List<int> _frequencies = new();


        //public static FileStringGenerator Sites = new FileStringGenerator("namesSitesInternational");
        public static FileStringGenerator Sites = new FileStringGenerator("namesSitesItalia");
        public static FileStringGenerator Regions = new FileStringGenerator("namesRegionsMilano");
        public static FileStringGenerator Herbs = new FileStringGenerator("namesHerbs");


        protected FileStringGenerator(string sourceFile) : this(new string[] { sourceFile }) { }

        protected FileStringGenerator(string[] sourceFiles)
        {
            foreach (var file in sourceFiles)
                loadFile(file);
        }


        protected void loadFile(string filename)
        {
            var fileObj = Resources.Load<TextAsset>($"Data/Generators/{filename}");
            var fileLines = fileObj.text.Split("\n");


            foreach (var line in fileLines)
            {
                if (line.Length == 0 || line[0] == '#' || line.Trim() == "")
                    //skip comments and empty lines
                    continue;

                var tokens = line.Trim().Split("|");
                Debug.Assert(tokens.Length <= 2);

                var name = tokens[0];
                var freq = (tokens.Length == 1 ? 1 : int.Parse(tokens[1]));

                _names.Add(name);
                _frequencies.Add(freq);
            }
        }


        public string GenerateString()
        {
            return RandomUtils.RandomChooseWeighted(_names, _frequencies);
        }
    }

}