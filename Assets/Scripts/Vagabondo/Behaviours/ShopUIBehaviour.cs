using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vagabondo.DataModel;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.Behaviours
{
    public class ShopUIBehaviour : MonoBehaviour
    {
        [Header("UI Fields")]
        [SerializeField]
        private TextMeshProUGUI title;
        [SerializeField]
        private TextMeshProUGUI travelerMoneyValueLabel;
        [SerializeField]
        private Transform travelerItemsPanel;
        [SerializeField]
        private Transform shopItemsPanel;
        [SerializeField]
        private Scrollbar travelerItemsScrollbar;
        [SerializeField]
        private TextMeshProUGUI shopMoneyValueLabel;
        [SerializeField]
        private Scrollbar shopItemsScrollbar;

        [Header("Prefabs")]
        [SerializeField]
        private GameObject tradableItemTemplate;

        private ShopInfo _shopInfo;
        public ShopInfo ShopInfo
        {
            set
            {
                _shopInfo = value;
                updateShopInfo();
                updateTravelerView();
                resetScrollbars();
            }
        }

        private Traveler _travelerData;


        private void OnEnable()
        {
            EventManager.onTravelerChanged += onTravelerChanged;
        }

        private void OnDisable()
        {
            EventManager.onTravelerChanged -= onTravelerChanged;
        }

        private void onTravelerChanged(Traveler travelerData)
        {
            this._travelerData = travelerData;
            updateTravelerView();
            updateShopItemsInteractable();
        }

        public void OnCloseClicked()
        {
            UnityUtils.HideUIView(gameObject);
        }

        public void SellItem(GameItem item)
        {
            _shopInfo.inventory.Remove(item);
            _shopInfo.money += item.currentPrice;
            updateShopInfo();
        }

        public void BuyItem(GameItem item)
        {
            _shopInfo.inventory.Add(item);
            _shopInfo.money -= item.currentPrice;
            updateShopInfo();
        }

        private void updateTravelerView()
        {
            travelerMoneyValueLabel.text = _travelerData.money.ToString();

            UnityUtils.RemoveAllChildren(travelerItemsPanel);
            foreach (var item in _travelerData.merchandise)
            {
                if (_shopInfo != null && _shopInfo.canBuy != null && !_shopInfo.canBuy(item))
                    continue;

                var newItemObj = Instantiate(tradableItemTemplate, travelerItemsPanel, false);
                newItemObj.GetComponent<InventoryItemBehaviour>().ShopUI = this;
                newItemObj.GetComponent<InventoryItemBehaviour>().Data = item;
                newItemObj.GetComponent<InventoryItemBehaviour>().IsTravelerSelling = true;
                newItemObj.GetComponent<InventoryItemBehaviour>().Interactable = true;
            }
        }

        private void updateShopInfo()
        {
            title.text = $"Trading: {_shopInfo.name}";
            shopMoneyValueLabel.text = _shopInfo.money.ToString();

            UnityUtils.RemoveAllChildren(shopItemsPanel);
            foreach (var item in _shopInfo.inventory)
            {
                var newItemObj = Instantiate(tradableItemTemplate, shopItemsPanel, false);
                newItemObj.GetComponent<InventoryItemBehaviour>().ShopUI = this;
                newItemObj.GetComponent<InventoryItemBehaviour>().Data = item;
                newItemObj.GetComponent<InventoryItemBehaviour>().IsTravelerSelling = false;
                if (_travelerData != null)
                    newItemObj.GetComponent<InventoryItemBehaviour>().Interactable = (item.currentPrice <= _travelerData.money);
            }
        }

        private void updateTravelerItemsInteractable()
        {
            if (_travelerData == null)
                return;

            for (int i = 0; i < _travelerData.merchandise.Count; i++)
            {
                //it would be simpler to use the itemData from the InventoryItemBehaviour,
                //but there are race conditions that make it unreliable
                var item = _travelerData.merchandise[i];

                //same filtering logic as in updateTravelerView, otherwise we are not in sync
                if (_shopInfo != null && _shopInfo.canBuy != null && !_shopInfo.canBuy(item))
                    continue;

                var itemObj = travelerItemsPanel.GetChild(i);
                itemObj.GetComponent<InventoryItemBehaviour>().Interactable = (item.currentPrice <= _shopInfo.money);
            }
        }

        private void updateShopItemsInteractable()
        {
            if (_shopInfo == null)
                return;

            for (int i = 0; i < _shopInfo.inventory.Count; i++)
            {
                //it would be simpler to use the itemData from the InventoryItemBehaviour,
                //but there are race conditions that make it unreliable
                var item = _shopInfo.inventory[i];

                //works because _shopInventory and shopItemsPanel children are in the same order
                if (_shopInfo != null && _shopInfo.canBuy != null && !_shopInfo.canBuy(item))
                    continue;

                var itemObj = shopItemsPanel.GetChild(i);
                var price = _shopInfo.inventory[i].currentPrice;
                itemObj.GetComponent<InventoryItemBehaviour>().Interactable = (price <= _travelerData.money);
            }
        }

        private void resetScrollbars()
        {
            travelerItemsScrollbar.value = 1;
            shopItemsScrollbar.value = 1;
        }
    }
}