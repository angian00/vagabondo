using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vagabondo.DataModel;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.Behaviours
{
    public class TownUIBehaviour : MonoBehaviour
    {
        [Header("UI Fields")]
        [SerializeField]
        private TextMeshProUGUI townNameLabel;
        [SerializeField]
        private TextMeshProUGUI townDescriptionLabel;
        [SerializeField]
        private Transform townActionPanel;

        [Header("Prefabs")]
        [SerializeField]
        private GameObject actionButtonTemplate;

        [Header("Other UI views")]
        [SerializeField]
        private GameObject travelerUI;
        [SerializeField]
        private GameObject merchandiseUI;
        [SerializeField]
        private GameObject destinationUI;


        private List<ActionButtonBehaviour> actionObjs = new();

        private void OnEnable()
        {
            EventManager.onTownChanged += updateView;
            EventManager.onTravelerChanged += updateInteractableActions;
        }

        private void OnDisable()
        {
            EventManager.onTownChanged -= updateView;
            EventManager.onTravelerChanged -= updateInteractableActions;
        }

        private void Start()
        {
            TravelManager.Instance.Init();
        }


        public void OnSwitchViewClicked()
        {
            UnityUtils.ShowUIView(travelerUI);
        }

        public void OnMerchandiseClicked()
        {
            UnityUtils.ShowUIView(merchandiseUI);
        }

        public void OnTravelClicked()
        {
            UnityUtils.ShowUIView(destinationUI);
        }

        public void updateView(Town townData)
        {
            townNameLabel.text = townData.name;
            //townDescriptionLabel.text = townData.description;
            townDescriptionLabel.text = townData.dominion.name + "\n" + DataUtils.EnumToStr(townData.biome);

            actionObjs.Clear();
            UnityUtils.RemoveAllChildren(townActionPanel);
            foreach (var action in townData.actions)
            {
                var newActionButton = Instantiate(actionButtonTemplate, townActionPanel, false);
                newActionButton.GetComponent<ActionButtonBehaviour>().Action = action;

                actionObjs.Add(newActionButton.GetComponent<ActionButtonBehaviour>());
            }

            //updateInteractableActions(TravelManager.Instance.Traveler); //FIXME
        }

        public void updateInteractableActions(Traveler travelerData)
        {
            foreach (var actionObj in actionObjs)
            {
                actionObj.ComputeInteractable(travelerData);
            }
        }

    }
}
