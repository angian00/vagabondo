using TMPro;
using UnityEngine;
using Vagabondo.DataModel;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.Behaviours
{
    public class DestinationItemBehaviour : MonoBehaviour
    {
        [Header("UI Fields")]
        [SerializeField]
        private TextMeshProUGUI townNameLabel;
        [SerializeField]
        private TextMeshProUGUI dominionLabel;
        [SerializeField]
        private Transform hintsPanel;

        [Header("Prefabs")]
        [SerializeField]
        private GameObject hintTemplate;


        public GameObject ParentView { get; set; }

        private Town _townData;
        public Town TownData
        {
            set
            {
                _townData = value;
                updateView();
            }
        }


        public void OnDestinationChosen()
        {
            TravelManager.Instance.TravelTo(_townData);
            UnityUtils.HideUIView(ParentView);
        }

        private void updateView()
        {
            townNameLabel.text = _townData.name;
            dominionLabel.text = _townData.dominion.name;

            UnityUtils.RemoveAllChildren(hintsPanel);
            foreach (var hint in _townData.hints)
            {
                var newHintObj = Instantiate(hintTemplate, hintsPanel, false);
                newHintObj.transform.Find("Label").GetComponent<TextMeshProUGUI>().text = hint;
            }
        }
    }
}