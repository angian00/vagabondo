using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vagabondo.DataModel;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.Behaviours
{
    public class InventoryItemBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public static int maxLabelLength = 20;

        [SerializeField]
        private TextMeshProUGUI nonusableItemLabel;
        [SerializeField]
        private TextMeshProUGUI usableItemLabel;
        [SerializeField]
        private GameObject useButton;
        [SerializeField]
        private TextMeshProUGUI useButtonLabel;
        [SerializeField]
        private GameObject tooltipObj;
        [SerializeField]
        private TextMeshProUGUI tooltipLabel;


        private GameItem _data;
        public GameItem Data { get => _data; set { _data = value; updateView(); } }

        private bool _isTravelerSelling;
        public bool IsTravelerSelling { set { _isTravelerSelling = value; updateView(); } }

        private bool _interactable = true;
        public bool Interactable { set { _interactable = value; updateView(); } }


        private ShopUIBehaviour _shopUI;
        public ShopUIBehaviour ShopUI { set { _shopUI = value; } }



        public void OnButtonClicked()
        {
            if (_shopUI)
                tradeItem();
            else
                useItem();
        }

        private void tradeItem()
        {
            TravelManager.Instance.TradeItem(_data, _isTravelerSelling);
            if (_isTravelerSelling)
                _shopUI.AddToShopInventory(_data);
            else
                _shopUI.RemoveFromShopInventory(_data);
        }

        private void useItem()
        {
            TravelManager.Instance.UseItem(_data);
        }


        private void updateView()
        {
            if ((_data.useVerb == UseVerb.None) && (_shopUI == null))
            {
                nonusableItemLabel.gameObject.SetActive(true);
                usableItemLabel.gameObject.SetActive(false);
                useButton.SetActive(false);

                nonusableItemLabel.text = _data.extendedName;
            }
            else
            {
                nonusableItemLabel.gameObject.SetActive(false);
                usableItemLabel.gameObject.SetActive(true);
                useButton.SetActive(true);

                usableItemLabel.text = _data.extendedName;

                string buttonlabelText;
                if (_shopUI)
                    buttonlabelText = $"{(_isTravelerSelling ? "Sell" : "Buy")} for {_data.currentPrice} $";
                else
                    buttonlabelText = DataUtils.EnumToStr(_data.useVerb);

                useButtonLabel.text = buttonlabelText;
                useButton.GetComponent<Button>().interactable = _interactable;
            }

            tooltipLabel.text = _data.extendedName;
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            if ((usableItemLabel.IsActive() && usableItemLabel.isTextOverflowing) ||
                    (nonusableItemLabel.IsActive() && nonusableItemLabel.isTextOverflowing))
                tooltipObj.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltipObj.SetActive(false);
        }


    }
}