using TMPro;
using UnityEngine;
using Vagabondo.Utils;

namespace Vagabondo.Behaviours
{
    public class MerchandiseUIBehaviour : MonoBehaviour
    {
        [Header("UI Fields")]
        [SerializeField]
        private TextMeshProUGUI moneyValueLabel;
        [SerializeField]
        private Transform merchandisePanel;

        [Header("Prefabs")]
        [SerializeField]
        private GameObject merchandiseItemTemplate;


        private void OnEnable()
        {
            EventManager.onTravelerChanged += updateView;
        }

        private void OnDisable()
        {
            EventManager.onTravelerChanged -= updateView;
        }

        public void OnSwitchViewClicked()
        {
            UnityUtils.HideUIView(gameObject);
        }


        private void updateView(TravelerData travelerData)
        {
            moneyValueLabel.text = travelerData.money.ToString();

            UnityUtils.RemoveAllChildren(merchandisePanel);
            foreach (var merchItem in travelerData.merchandise)
            {
                var newMerchandiseItemObj = Instantiate(merchandiseItemTemplate, merchandisePanel, false);
                newMerchandiseItemObj.GetComponent<MerchandiseItemBehaviour>().Data = merchItem;
            }
        }
    }
}