using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using Vagabondo.Grammar;

namespace Vagabondo.Experiment
{
    public class ExperimentSubstitutionGrammar : MonoBehaviour
    {
        private static string rootRuleName = "origin";

        [SerializeField]
        private TMP_Dropdown grammarDropdown;
        [SerializeField]
        private TextMeshProUGUI outputField;

        private RichGrammar grammar;


        private void Start()
        {
            //populateGrammarDropdown();
            var townDescriptionGrammarFiles = new List<string>() {
                "townRoot",
                "townSentenceChildren",
                "townSentenceNature",
                "townSentenceChurch",
                "townSentenceFields",
                "townSentencePond",
                "townStructures",
                "townNouns",
                "townVerbs",
                "townAdjectives",
            };
            grammar = new RichGrammar(townDescriptionGrammarFiles);
        }

        public void OnRun()
        {
            var variables = new Dictionary<string, string>();
            variables.Add("townType", "village");
            var generatedText = grammar.GenerateText(rootRuleName, variables: variables);

            outputField.text = generatedText;
            Debug.Log("-- generatedText:");
            Debug.Log(generatedText);
        }

        private void populateGrammarDropdown()
        {

            grammarDropdown.onValueChanged.AddListener(delegate { onGrammarChanged(); });
            grammarDropdown.ClearOptions();
            grammarDropdown.AddOptions(new List<string>(listGrammarFiles()));
            grammarDropdown.RefreshShownValue();
            grammarDropdown.value = -1;
        }

        private List<string> listGrammarFiles()
        {
            var result = new List<string>();

            var resourcesPath = Application.dataPath + "/Resources/Data/Grammars";
            var filePaths = Directory.GetFiles(resourcesPath)
                        .Where(x => Path.GetExtension(x) == ".json");

            foreach (var filePath in filePaths)
            {
                var filename = Path.GetFileName(filePath);
                result.Add(filename.Substring(0, filename.Length - 5));
            }

            return result;
        }

        private void onGrammarChanged()
        {
            //var filename = grammarDropdown.options[grammarDropdown.value].text;
            //grammar = new RichGrammar(filename);
        }
    }
}
