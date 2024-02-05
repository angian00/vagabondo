using System;
using System.Collections.Generic;

namespace Vagabondo.DataModel
{
    public class ChoiceTreeNode
    {
        public string id;
        public string title;
        public string description;

        //TODO: side effects somewhere

        public bool isFinal;
        public string choiceA;
        public string choiceB;
    }

    public class ChoiceTree
    {
        public Dictionary<string, ChoiceTreeNode> nodes = new();
        public string startNodeId;

        private ChoiceTree() { }

        public static ChoiceTree Load(string filename)
        {
            //TODO: ChoiceTree Load
            throw new NotImplementedException();
        }

    }
}
