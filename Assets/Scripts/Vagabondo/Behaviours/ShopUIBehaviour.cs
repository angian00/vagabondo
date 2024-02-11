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
        private TextMeshProUGUI moneyValueLabel;
        [SerializeField]
        private Transform travelerItemsPanel;
        [SerializeField]
        private Transform shopItemsPanel;
        [SerializeField]
        private Scrollbar travelerItemsScrollbar;
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

        public void RemoveFromShopInventory(GameItem item)
        {
            _shopInfo.inventory.Remove(item);
            updateShopInfo();
        }

        public void AddToShopInventory(GameItem item)
        {
            _shopInfo.inventory.Add(item);
            updateShopInfo();
        }

        private void updateTravelerView()
        {
            moneyValueLabel.text = _travelerData.money.ToString();

            UnityUtils.RemoveAllChildren(travelerItemsPanel);
            foreach (var item in _travelerData.merchandise)
            {
                if (_shopInfo != null && _shopInfo.canBuy != null && !_shopInfo.canBuy(item))
                    continue;

                var newItemObj = Instantiate(tradableItemTemplate, travelerItemsPanel, false);
                newItemObj.GetComponent<ShopItemBehaviour>().Parent = this;
                newItemObj.GetComponent<ShopItemBehaviour>().Data = item;
                newItemObj.GetComponent<ShopItemBehaviour>().IsTravelerSelling = true;
                newItemObj.GetComponent<ShopItemBehaviour>().Interactable = true;
            }

            //if (_shopInfo != null)
            //    updateShopItemsInteractable();
        }

        private void updateShopInfo()
        {
            title.text = $"Trading: {_shopInfo.name}";

            UnityUtils.RemoveAllChildren(shopItemsPanel);
            foreach (var item in _shopInfo.inventory)
            {
                var newItemObj = Instantiate(tradableItemTemplate, shopItemsPanel, false);
                newItemObj.GetComponent<ShopItemBehaviour>().Parent = this;
                newItemObj.GetComponent<ShopItemBehaviour>().Data = item;
                newItemObj.GetComponent<ShopItemBehaviour>().IsTravelerSelling = false;
                if (_travelerData != null)
                    newItemObj.GetComponent<ShopItemBehaviour>().Interactable = (item.currentPrice <= _travelerData.money);
            }
        }

        private void updateShopItemsInteractable()
        {
            if (_shopInfo == null)
                return;

            for (int i = 0; i < _shopInfo.inventory.Count; i++)
            {
                //works because _shopInventory and shopItemsPanel children are in the same order
                var itemObj = shopItemsPanel.GetChild(i);
                //var price = itemObj.GetComponent<ShopItemBehaviour>().Data.currentPrice;
                var price = _shopInfo.inventory[i].currentPrice;
                itemObj.GetComponent<ShopItemBehaviour>().Interactable = (price <= _travelerData.money);
            }
        }

        private void resetScrollbars()
        {
            travelerItemsScrollbar.value = 1;
            shopItemsScrollbar.value = 1;

            //shopItemsPanel.Find("Scrollbar Vertical").GetComponent<Scrollbar>().value = 1;
        }
    }
}