using System.Collections.Generic;
using Vagabondo.Utils;

namespace Vagabondo.DataModel
{
    public enum DominionType
    {
        FreeState,
        Barony,
        County,
        Marquisdom,
        Duchy,
        Archduchy,
        Principate,
    }

    public enum DominionTrait
    {
        Default,

        Rich,
        Poor,

        Wild,
        Rural,
        Industrial,

        HighCrime,

        Fanatic,

    }

    public class DominionTemplate
    {
        public DominionType type;
        public int maxNTowns;
        public TownSize maxTownSize;
        public float persistence;
        public int frequency;
    }


    public class Dominion
    {
        public readonly string name;
        public readonly int maxNTowns;
        public readonly TownSize maxTownSize;
        public readonly float persistence;

        public int nTowns = 0;
        public HashSet<DominionTrait> traits = new();


        public Dominion(DominionTemplate template, string regionName)
        {
            this.maxNTowns = template.maxNTowns;
            this.maxTownSize = template.maxTownSize;
            this.persistence = template.persistence;
            this.name = DataUtils.EnumToStr(template.type) + " of " + regionName;
        }
    }
}
