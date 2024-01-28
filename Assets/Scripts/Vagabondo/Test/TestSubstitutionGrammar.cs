using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using Vagabondo.Grammar;

namespace Vagabondo.Test
{
    public class TestSubstitutionGrammar : MonoBehaviour
    {
        private static string grammarFilename = "poem";
        private static string rootRuleName = "origin";

        [SerializeField]
        private TMP_Dropdown grammarDropdown;
        [SerializeField]
        private TextMeshProUGUI outputField;

        private SubstitutionGrammar grammar;


        private void Start()
        {
            populateGrammarDropdown();
            //TestJsonLoader.LoadTestJson("exampleGrammarTracerySyntax");
        }

        public void OnRun()
        {
            var generatedText = grammar.GenerateText(rootRuleName);

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
            var filename = grammarDropdown.options[grammarDropdown.value].text;
            grammar = SubstitutionGrammar.Load(filename);

        }
    }
}
