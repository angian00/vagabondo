using TMPro;
using UnityEngine;
using Vagabondo.DataModel;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.Behaviours
{
    public class TravelerUIBehaviour : MonoBehaviour
    {
        [Header("UI Fields")]
        [SerializeField]
        private Transform trinketsPanel;
        [SerializeField]
        private Transform knowledgePanel;
        [SerializeField]
        private Transform memoriesPanel;

        [Header("Prefabs")]
        [SerializeField]
        private GameObject trinketTemplate;
        [SerializeField]
        private GameObject knowledgeItemTemplate;
        [SerializeField]
        private GameObject memoryTemplate;


        private void OnEnable()
        {
            EventManager.onTravelerChanged += updateView;
        }

        private void OnDisable()
        {
            EventManager.onTravelerChanged -= updateView;
        }

        public void OnSwitchViewClicked()
        {
            UnityUtils.HideUIView(gameObject);
        }


        private void updateView(Traveler travelerData)
        {
            //Debug.Log("TravelerUIBehaviour.updateView()");

            UnityUtils.RemoveAllChildren(trinketsPanel);
            foreach (var trinket in travelerData.trinkets)
            {
                var newTrinketObj = Instantiate(trinketTemplate, trinketsPanel, false);
                newTrinketObj.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = trinket.text;
            }

            UnityUtils.RemoveAllChildren(knowledgePanel);
            foreach (var knowledgeKey in travelerData.knowledge.Keys)
            {
                var knowledgeValue = travelerData.knowledge[knowledgeKey];
                var newKnowledgeObj = Instantiate(knowledgeItemTemplate, knowledgePanel, false);

                newKnowledgeObj.transform.Find("Label").GetComponent<TextMeshProUGUI>().text = DataUtils.EnumToStr(knowledgeKey);
                newKnowledgeObj.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = knowledgeValue.ToString();
            }

            UnityUtils.RemoveAllChildren(memoriesPanel);
            foreach (var memory in travelerData.memories)
            {
                var newMemoryObj = Instantiate(memoryTemplate, memoriesPanel, false);

                newMemoryObj.GetComponent<MemoryItemBehaviour>().Memory = memory;
                //FUTURE: color varies with memory type
            }

        }


    }
}