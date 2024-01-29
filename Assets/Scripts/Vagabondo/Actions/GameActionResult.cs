using Vagabondo.DataModel;

namespace Vagabondo.Actions
{
    public abstract class GameActionResult { }

    public class TextActionResult : GameActionResult
    {
        public readonly string text;
        public TextActionResult(string text)
        {
            this.text = text;
        }
    }

    public class ItemAcquiredActionResult : GameActionResult
    {
        public readonly string text;
        public Trinket item;

        public ItemAcquiredActionResult(string text, Trinket item)
        {
            this.text = text;
            this.item = item;
        }
    }

}
