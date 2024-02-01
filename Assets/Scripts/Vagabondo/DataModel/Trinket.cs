namespace Vagabondo.DataModel
{
    public struct Trinket : GameItem
    {
        public string text;

        public Trinket(string text)
        {
            this.text = text;
        }
    }
}