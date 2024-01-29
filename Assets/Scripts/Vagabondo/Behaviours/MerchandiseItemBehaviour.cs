using TMPro;
using UnityEngine;
using Vagabondo.DataModel;
using Vagabondo.Managers;

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