using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vagabondo.Actions;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.Behaviours
{
    public class ActionResultUIBehaviour : MonoBehaviour
    {
        [Header("UI Fields")]
        [SerializeField]
        private TextMeshProUGUI descriptionLabel;
        [SerializeField]
        private GameObject actionButton;

        [Header("Other UI views")]
        [SerializeField]
        private GameObject shopUI;

        private GameActionResult _actionResult;

        private void OnEnable()
        {
            EventManager.onActionPerformed += updateView;
        }

        private void OnDisable()
        {
            EventManager.onActionPerformed -= updateView;
        }


        public void OnCloseClicked()
        {
            UnityUtils.HideUIView(gameObject);
        }

        private void updateView(GameActionResult actionResult)
        {
            this._actionResult = actionResult;
            UnityUtils.ShowUIView(gameObject);

            descriptionLabel.text = actionResult.text;
            if (actionResult is ShopActionResult)
            {
                actionButton.transform.Find("Action Label").GetComponent<TextMeshProUGUI>().text = "Trade";
                actionButton.GetComponent<Button>().onClick.RemoveAllListeners();
                actionButton.GetComponent<Button>().onClick.AddListener(onActionShop);
                actionButton.SetActive(true);
            }

            else
                actionButton.SetActive(false);
        }

        private void onActionShop()
        {
            UnityUtils.HideUIView(gameObject);

            var shopInventory = ((ShopActionResult)_actionResult).shopInventory;
            shopUI.GetComponent<ShopUIBehaviour>().ShopInventory = shopInventory;
            UnityUtils.ShowUIView(shopUI);
        }
    }
}