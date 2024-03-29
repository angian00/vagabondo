using System.Collections.Generic;
using UnityEngine;
using Vagabondo.DataModel;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.Behaviours
{
    public class DestinationUIBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Transform destinationsPanel;

        [SerializeField]
        private GameObject destinationObjTemplate;


        private void OnEnable()
        {
            EventManager.onDestinationsChanged += updateView;
        }

        private void OnDisable()
        {
            EventManager.onDestinationsChanged -= updateView;

        }

        private void updateView(List<Town> destinationsData)
        {
            UnityUtils.RemoveAllChildren(destinationsPanel);
            foreach (var destData in destinationsData)
            {
                var newDestPanel = Instantiate(destinationObjTemplate, destinationsPanel, false);
                newDestPanel.GetComponent<DestinationItemBehaviour>().ParentView = gameObject;
                newDestPanel.GetComponent<DestinationItemBehaviour>().TownData = destData;
            }
        }

    }
}