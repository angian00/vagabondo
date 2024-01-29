namespace Vagabondo.Quests
{
    public class QuestState
    {
        public readonly Quest parent;

        public string actionTitle;
        public string actionDescription;
        public string actionResultText;
        public string memoryTitle;
        public string memoryDescription;
        public bool isFinal = false;

        public QuestState(Quest parent)
        {
            this.parent = parent;
        }
    }

    public class Quest
    {
        public readonly Guid id = Guid.NewGuid();

        public List<QuestState> states = new();
        public int currentStateIndex = 0;

        public void AdvanceState()
        {
            currentStateIndex++;
        }

        public QuestState GetCurrentState()
        {
            return states[currentStateIndex];
        }
    }
}
