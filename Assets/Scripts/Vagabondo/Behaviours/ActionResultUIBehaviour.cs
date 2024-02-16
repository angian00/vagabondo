using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vagabondo.TownActions;
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
        private TextMeshProUGUI resultLabel;
        [SerializeField]
        private GameObject actionButton;

        [Header("Other UI views")]
        [SerializeField]
        private GameObject shopUI;

        private TownActionResult _actionResult;

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

        private void updateView(TownActionResult actionResult)
        {
            this._actionResult = actionResult;
            UnityUtils.ShowUIView(gameObject);

            descriptionLabel.text = actionResult.descriptionText;
            if (actionResult.resultText != null)
                resultLabel.text = actionResult.resultText;
            else
                resultLabel.text = "";


            if (actionResult is ShopActionResult)
            {
                if (((ShopActionResult)_actionResult).skipTextResult)
                {
                    onActionShop();
                }
                else
                {

                    actionButton.transform.Find("Action Label").GetComponent<TextMeshProUGUI>().text = "Trade";
                    actionButton.GetComponent<Button>().onClick.RemoveAllListeners();
                    actionButton.GetComponent<Button>().onClick.AddListener(onActionShop);
                    actionButton.SetActive(true);
                }
            }

            else
                actionButton.SetActive(false);
        }

        private void onActionShop()
        {
            UnityUtils.HideUIView(gameObject);

            var shopActionResult = (ShopActionResult)_actionResult;
            shopUI.GetComponent<ShopUIBehaviour>().ShopInfo = shopActionResult.shopInfo;
            UnityUtils.ShowUIView(shopUI);
        }
    }
}