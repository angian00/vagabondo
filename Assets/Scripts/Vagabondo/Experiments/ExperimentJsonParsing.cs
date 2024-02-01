using Newtonsoft.Json;
using UnityEngine;

namespace Vagabondo.Experiment
{
    class DataClass
    {
        public string Field1 { get; set; }
        public string Field2 { get; set; }
    }

    class VariantDataClass
    {
        public string Field1 { get; set; }
        public object Field2 { get; set; }
    }

    public class ExperimentJsonParsing : MonoBehaviour
    {
        private const string filename = "experimentJsonParsing";

        public void OnRun()
        {
            runExperimentNewtonsoft();
        }

        private void runExperimentNewtonsoft()
        {
            Debug.Log("ExperimentJsonParsing.Run()");
            var fileObj = Resources.Load<TextAsset>($"Data/Grammars/Experiments/{filename}");
            Debug.Log("fileObj.text:");
            Debug.Log(fileObj.text);

            Debug.Log("before Deserialize<DataClass>");
            var parsedDataClass = JsonConvert.DeserializeObject<DataClass>(fileObj.text);
            Debug.Log("parsedDataClass:");
            Debug.Log(parsedDataClass);
            Debug.Log("parsedDataClass.Field1:");
            Debug.Log(parsedDataClass.Field1);

            var parsedVariantDataClass = JsonConvert.DeserializeObject<VariantDataClass>(fileObj.text);
            Debug.Log("parsedVariantDataClass:");
            Debug.Log(parsedVariantDataClass);
            Debug.Log("parsedVariantDataClass.Field1:");
            Debug.Log(parsedVariantDataClass.Field1);
            Debug.Log("parsedVariantDataClass.Field2:");
            Debug.Log(parsedVariantDataClass.Field2);
            Debug.Log("parsedVariantDataClass.Field2.GetType():");
            Debug.Log(parsedVariantDataClass.Field2.GetType());


            //fileObj = Resources.Load<TextAsset>($"Data/Grammars/Experiments/{filename}");
            //Debug.Log("before DeserializeObject");
            //dynamic parsedGrammar = JsonConvert.DeserializeObject(fileObj.text);
            //Debug.Log("parsedGrammar:");
            //Debug.Log(parsedGrammar);
            //Debug.Log("parsedGrammar[\"field1\"]:");
            //Debug.Log(parsedGrammar["field1"]);
            //Debug.Log("parsedGrammar.field1:");
            //Debug.Log(parsedGrammar.field1);

        }
    }
}
