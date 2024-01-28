using TMPro;
using UnityEngine;
using Vagabondo.Utils;

namespace Vagabondo.Behaviours
{
    public class ActionResultUIBehaviour : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI descriptionLabel;


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

        private void updateView(string actionResult)
        {
            Debug.Log("ActionResultUIBehaviour.updateMerchandisePanel()");
            UnityUtils.ShowUIView(gameObject);

            descriptionLabel.text = actionResult;
        }


    }
}