using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vagabondo.DataModel;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.Behaviours
{
    public class ShopUIBehaviour : MonoBehaviour
    {
        [Header("UI Fields")]
        [SerializeField]
        private TextMeshProUGUI moneyValueLabel;
        [SerializeField]
        private Transform travelerItemsPanel;
        [SerializeField]
        private Transform shopItemsPanel;

        [Header("Prefabs")]
        [SerializeField]
        private GameObject tradableItemTemplate;

        private List<TradableItem> _shopInventory;
        public List<TradableItem> ShopInventory { set { _shopInventory = value; updateShopInventory(); } }

        private Traveler _travelerData;


        private void OnEnable()
        {
            EventManager.onTravelerChanged += onTravelerChanged;
        }

        private void OnDisable()
        {
            EventManager.onTravelerChanged -= onTravelerChanged;
        }

        public void OnCloseClicked()
        {
            UnityUtils.HideUIView(gameObject);
        }

        public void RemoveFromShopInventory(TradableItem item)
        {
            _shopInventory.Remove(item);
            updateShopInventory();
        }

        public void AddToShopInventory(TradableItem item)
        {
            _shopInventory.Add(item);
            updateShopInventory();
        }

        private void onTravelerChanged(Traveler travelerData)
        {
            this._travelerData = travelerData;
            updateTravelerView();
        }

        private void updateTravelerView()
        {
            moneyValueLabel.text = _travelerData.money.ToString();

            UnityUtils.RemoveAllChildren(travelerItemsPanel);
            foreach (var item in _travelerData.merchandise)
            {
                var newItemObj = Instantiate(tradableItemTemplate, travelerItemsPanel, false);
                newItemObj.GetComponent<ShopItemBehaviour>().Parent = this;
                newItemObj.GetComponent<ShopItemBehaviour>().Data = item;
                newItemObj.GetComponent<ShopItemBehaviour>().IsTravelerSelling = true;
            }

            if (_shopInventory != null)
                updateInteractableInventory();
        }

        private void updateShopInventory()
        {
            UnityUtils.RemoveAllChildren(shopItemsPanel);
            foreach (var item in _shopInventory)
            {
                var newItemObj = Instantiate(tradableItemTemplate, shopItemsPanel, false);
                newItemObj.GetComponent<ShopItemBehaviour>().Parent = this;
                newItemObj.GetComponent<ShopItemBehaviour>().Data = item;
                newItemObj.GetComponent<ShopItemBehaviour>().IsTravelerSelling = false;
                if (_travelerData != null)
                    newItemObj.GetComponent<ShopItemBehaviour>().Interactable = (item.currentPrice <= _travelerData.money);
            }
        }

        private void updateInteractableInventory()
        {
            for (int i = 0; i < _shopInventory.Count; i++)
            {
                //works because _shopInventory and shopItemsPanel children are in the same order
                var itemObj = shopItemsPanel.GetChild(i);
                //var price = itemObj.GetComponent<ShopItemBehaviour>().Data.currentPrice;
                var price = _shopInventory[i].currentPrice;
                itemObj.GetComponent<ShopItemBehaviour>().Interactable = (price <= _travelerData.money);
            }

        }
    }
}