using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Vagabondo.DataModel;

namespace Vagabondo.Behaviours
{
    public class InventoryItemBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public static int maxLabelLength = 20;

        [SerializeField]
        private TextMeshProUGUI itemLabel;
        [SerializeField]
        private GameObject tooltipObj;
        [SerializeField]
        private TextMeshProUGUI tooltipLabel;


        private GameItem _data;
        public GameItem Data { get => _data; set { _data = value; updateView(); } }


        private void updateView()
        {
            itemLabel.text = _data.extendedName;
            tooltipLabel.text = _data.extendedName;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (itemLabel.isTextOverflowing)
                tooltipObj.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltipObj.SetActive(false);
        }


    }
}