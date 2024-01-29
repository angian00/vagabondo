using Vagabondo.DataModel;
using Vagabondo.Managers;

namespace Vagabondo.Actions
{
    public class QuestAction : GameAction
    {
        private QuestState questState;

        public QuestAction(QuestState questState) : base(GameActionType.Quest)
        {
            this.questState = questState;
            this.title = questState.actionTitle;
            this.description = questState.actionDescription;
        }

        public override bool CanPerform(Traveler travelerData)
        {
            return true;
        }

        public override string GetCantPerformMessage()
        {
            return "";
        }

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
                memory = new QuestFragmentMemory();
                questState.parent.AdvanceState();
            }

            memory.title = questState.memoryTitle;
            memory.description = questState.memoryDescription;
            travelManager.AddMemory(memory);

            return new TextActionResult(questState.actionResultText);
        }
    }
}
