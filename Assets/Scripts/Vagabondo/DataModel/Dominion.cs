namespace Vagabondo.DataModel
{
    public class DominionType
    {
        public string name;
        public int size;
        public float permanence; //probability to transition to itself
    }

    public class Dominion
    {
        public string name;
        public DominionType type;
        //TODO: archetype(religion, nomadness, ...)

        public Dominion(DominionType type, string name)
        {
            this.type = type;
            this.name = name;
        }
    }
}