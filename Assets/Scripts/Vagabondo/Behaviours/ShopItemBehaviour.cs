using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vagabondo.DataModel;
using Vagabondo.Managers;

namespace Vagabondo.Behaviours
{
    public class ShopItemBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public static int maxLabelLength = 20;

        [SerializeField]
        private TextMeshProUGUI itemLabel;
        [SerializeField]
        private Button tradeButton;
        [SerializeField]
        private TextMeshProUGUI buttonLabel;
        [SerializeField]
        private GameObject tooltipObj;
        [SerializeField]
        private TextMeshProUGUI tooltipLabel;


        private GameItem _data;
        public GameItem Data { get => _data; set { _data = value; updateView(); } }

        private bool _isTravelerSelling;
        public bool IsTravelerSelling { set { _isTravelerSelling = value; updateView(); } }

        private bool _interactable;
        public bool Interactable { set { _interactable = value; updateView(); } }

        private ShopUIBehaviour _parent;
        public ShopUIBehaviour Parent { set { _parent = value; } }


        public void OnTradeItem()
        {
            TravelManager.Instance.TradeItem(_data, _isTravelerSelling);
            if (_isTravelerSelling)
                _parent.AddToShopInventory(_data);
            else
                _parent.RemoveFromShopInventory(_data);
        }

        private void updateView()
        {
            itemLabel.text = _data.extendedName;
            tooltipLabel.text = _data.extendedName;

            buttonLabel.text = $"{(_isTravelerSelling ? "Sell" : "Buy")} for {_data.currentPrice} $";
            tradeButton.interactable = _interactable;
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