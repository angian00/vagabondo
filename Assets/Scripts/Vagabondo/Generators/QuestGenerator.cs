using Vagabondo.DataModel;

namespace Vagabondo.Generators
{
    public class QuestGenerator
    {
        public static Quest GenerateQuest()
        {
            var quest = new Quest();
            var questPlot = new string[] { "dungeonIntro", "artifactIntro", "dungeonGuardian", "getArtifact" };

            foreach (var questPlotItem in questPlot)
            {
                var questState = new QuestState(quest);
                generateState(questState, questPlotItem);

                quest.states.Add(questState);
            }

            quest.states[quest.states.Count - 1].isFinal = true;

            return quest;
        }


        private static void generateState(QuestState questState, string plotItemId)
        {
            //TODO: compile and use quest grammar
            if (plotItemId == "dungeonIntro")
            {
                questState.actionTitle = "Chat with the locals";
                questState.actionDescription = "Spend some time socializing with the villagers";
                questState.memoryTitle = "The Abandoned Church";
                questState.memoryDescription = "A local farmer told you that somewhere in a neighbouring valley "
                    + "lay the ruins an old church, abandoned a century ago after a plague.";
                questState.actionResultText = questState.memoryDescription;
            }
            else if (plotItemId == "artifactIntro")
            {
                questState.actionTitle = "Chat with the locals";
                questState.actionDescription = "Spend some time socializing with the villagers";
                questState.memoryTitle = "An Artifact of Yore";
                questState.memoryDescription = "I racconti popolari della zona menzionano un antico artefatto ormai smarrito."
                    + " L'artefatto e' di pietra verde ricoperta di muschio. secondo alcuni porta bene, secondo altri male.";
                questState.actionResultText = questState.memoryDescription;
            }
            else if (plotItemId == "dungeonGuardian")
            {
                questState.actionTitle = "Dealing in antiquities";
                questState.actionDescription = "Make some cheap purchases at the local second-hand dealer";
                questState.memoryTitle = "The Church Warden Diary";
                questState.memoryDescription = "Hai trovato il diario del guardiano della chiesa. "
                    + "vi si dice che la chiesa gode di una protezione dagli influssi maligni che allignano nel bosco";
                questState.actionResultText = questState.memoryDescription;
            }
            else if (plotItemId == "getArtifact")
            {
                questState.actionTitle = "Explore Green Pasture church";
                questState.actionDescription = "You have finally found that legendary old church, it's time to explore it";
                questState.actionResultText = @"You rifle through the ruins, in search of something valuable or interesting. "
                    + "You feel a cold round shape under your fingers. You triumphantly grasp it and take it out of the rubble: "
                    + "it's the mossy artifact! Finally! Once it is exposed to the air, it crumbles with a sizzle into a handful of dust. Too bad.";
                questState.memoryTitle = "The Mossy Artifact";
                questState.memoryDescription = "You managed to put your hands on an old stone artifact fuzzy with moss and moisture, "
                    + "only to see it crumble in your hands.";
            }
        }

    }
}
