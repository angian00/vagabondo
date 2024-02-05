using System.Collections.Generic;

namespace Vagabondo.DataModel
{
    public class DominionType
    {
        public string name;
        public int size;
        public float permanence; //probability to transition to itself
    }

    public enum DominionTrait
    {
        Default,

        Rich,
        Poor,

        Rural,
        Industrial,

        HighCrime,

        Fanatic,
    }


    public class Dominion
    {
        public string name;
        public DominionType type;
        public HashSet<DominionTrait> traits = new();


        public Dominion(DominionType type, string name)
        {
            this.type = type;
            this.name = name;
        }
    }
}
