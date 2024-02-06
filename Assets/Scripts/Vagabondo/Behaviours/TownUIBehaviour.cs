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
        private Town townData;
        private Traveler travelerData;


        private void OnEnable()
        {
            EventManager.onTownChanged += onTownChanged;
            EventManager.onTravelerChanged += onTravelerChanged;
        }

        private void OnDisable()
        {
            EventManager.onTownChanged -= onTownChanged;
            EventManager.onTravelerChanged -= onTravelerChanged;
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


        public void onTownChanged(Town townData)
        {
            Debug.Log("TownUIBehaviour.onTownChanged()");
            this.townData = townData;
            updateView();
            if (travelerData != null)
                updateInteractableActions();
        }

        private void updateView()
        {
            townNameLabel.text = townData.name;
            //townDescriptionLabel.text = townData.description;
            townDescriptionLabel.text = townData.Dump();

            actionObjs.Clear();
            UnityUtils.RemoveAllChildren(townActionPanel);
            foreach (var action in townData.actions)
            {
                var newActionButton = Instantiate(actionButtonTemplate, townActionPanel, false);
                newActionButton.GetComponent<ActionButtonBehaviour>().Action = action;

                actionObjs.Add(newActionButton.GetComponent<ActionButtonBehaviour>());
            }
        }

        public void onTravelerChanged(Traveler travelerData)
        {
            Debug.Log("TownUIBehaviour.onTravelerChanged()");
            this.travelerData = travelerData;
            updateInteractableActions();

            foreach (var actionObj in actionObjs)
            {
                actionObj.ComputeInteractable(travelerData);
            }
        }

        private void updateInteractableActions()
        {
            foreach (var actionObj in actionObjs)
            {
                actionObj.ComputeInteractable(travelerData);
            }
        }

    }
}
