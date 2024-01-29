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

        private void updateView(GameActionResult actionResult)
        {
            UnityUtils.ShowUIView(gameObject);

            if (actionResult is TextActionResult)
            {
                descriptionLabel.text = actionResult;
            }
            else if (actionResult is ItemAcquiredActionResult)
            {
                descriptionLabel.text = actionResult;
                //TODO: schedule item reveal
            }
        }
    }
}