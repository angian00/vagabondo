using TMPro;
using UnityEngine;
using Vagabondo.DataModel;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.Behaviours
{
    public class DestinationItemBehaviour : MonoBehaviour
    {
        public GameObject ParentView { get; set; }

        private Town _data;
        public Town Data { set { _data = value; updateView(); } }


        public void OnDestinationChosen()
        {
            TravelManager.Instance.TravelTo(_data.name);
            UnityUtils.HideUIView(ParentView);
        }

        private void updateView()
        {
            var townNameLabel = gameObject.transform.Find("Town Name").GetComponent<TextMeshProUGUI>();
            townNameLabel.text = _data.name;

            //var hintsPanel = gameObject.transform.Find("Hints"); //FUTURE: visualize hints
        }


    }
}