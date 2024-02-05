using TMPro;
using UnityEngine;
using Vagabondo.DataModel;
using Vagabondo.Managers;

namespace Vagabondo.Behaviours
{
    public class ShopItemBehaviour : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI itemLabel;
        [SerializeField]
        private TextMeshProUGUI buttonLabel;

        private TradableItem _data;
        public TradableItem Data { set { _data = value; updateView(); } }

        private bool _isTravelerSelling;
        public bool IsTravelerSelling { set { _isTravelerSelling = value; updateView(); } }

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
            itemLabel.text = _data.name;
            buttonLabel.text = $"{(_isTravelerSelling ? "Sell" : "Buy")} for {_data.price} $";
        }


    }
}