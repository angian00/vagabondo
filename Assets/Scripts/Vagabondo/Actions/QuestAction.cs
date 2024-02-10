using Vagabondo.DataModel;
using Vagabondo.Managers;

namespace Vagabondo.Actions
{
    public class QuestAction : GameAction
    {
        private QuestState questState;

        public QuestAction(QuestState questState, Town townData) : base(GameActionType.Quest, townData)
        {
            this.questState = questState;
            this.title = questState.actionTitle;
            this.description = questState.actionDescription;
        }

        public override bool isQuestAction() => true;

        public override GameActionResult Perform(TravelManager travelManager)
        {
            Memory memory;
            if (questState.isFinal)
            {
                memory = new QuestMemory();
                travelManager.RemoveQuestFragments(questState.parent.id);
                travelManager.NewQuest();
            }
            else
            {
                memory = new QuestFragmentMemory(questState.parent.id);
                questState.parent.AdvanceState();
            }

            memory.title = questState.memoryTitle;
            memory.description = questState.memoryDescription;
            travelManager.AddMemory(memory);

            return new GameActionResult(questState.actionResultText);
        }
    }
}
