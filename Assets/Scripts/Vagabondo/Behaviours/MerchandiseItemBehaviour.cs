using TMPro;
using UnityEngine;

namespace Vagabondo.Behaviours
{
    public class MerchandiseItemBehaviour : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI itemLabel;
        [SerializeField]
        private TextMeshProUGUI buttonLabel;

        private MerchandiseItem _data;
        public MerchandiseItem Data { set { _data = value; updateView(); } }


        public void OnSellItem()
        {
            TravelManager.Instance.SellMerchandiseItem(_data);
        }

        private void updateView()
        {
            itemLabel.text = _data.text;
            buttonLabel.text = $"Sell for {_data.price} $";
        }


    }
}