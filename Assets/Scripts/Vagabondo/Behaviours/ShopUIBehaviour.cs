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

        private void OnEnable()
        {
            EventManager.onTravelerChanged += updateTravelerView;
        }

        private void OnDisable()
        {
            EventManager.onTravelerChanged -= updateTravelerView;
        }

        public void OnSwitchViewClicked()
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


        private void updateTravelerView(Traveler travelerData)
        {
            moneyValueLabel.text = travelerData.money.ToString();

            UnityUtils.RemoveAllChildren(travelerItemsPanel);
            foreach (var item in travelerData.merchandise)
            {
                var newItemObj = Instantiate(tradableItemTemplate, travelerItemsPanel, false);
                newItemObj.GetComponent<ShopItemBehaviour>().Parent = this;
                newItemObj.GetComponent<ShopItemBehaviour>().Data = item;
                newItemObj.GetComponent<ShopItemBehaviour>().IsTravelerSelling = true;
            }

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
            }
        }
    }
}