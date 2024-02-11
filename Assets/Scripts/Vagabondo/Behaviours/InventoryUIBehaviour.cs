using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vagabondo.DataModel;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.Behaviours
{
    public class InventoryUIBehaviour : MonoBehaviour
    {
        [Header("UI Fields")]
        [SerializeField]
        private TextMeshProUGUI moneyValueLabel;
        [SerializeField]
        private Transform travelerItemsPanel;
        [SerializeField]
        private Scrollbar travelerItemsScrollbar;

        [Header("Prefabs")]
        [SerializeField]
        private GameObject itemTemplate;

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
            resetScrollbar();
        }

        public void OnCloseClicked()
        {
            UnityUtils.HideUIView(gameObject);
        }

        private void updateTravelerView()
        {
            moneyValueLabel.text = _travelerData.money.ToString();

            UnityUtils.RemoveAllChildren(travelerItemsPanel);
            foreach (var item in _travelerData.merchandise)
            {
                var newItemObj = Instantiate(itemTemplate, travelerItemsPanel, false);
                newItemObj.GetComponent<InventoryItemBehaviour>().Data = item;
            }
        }

        private void resetScrollbar()
        {
            travelerItemsScrollbar.value = 1;
        }
    }
}