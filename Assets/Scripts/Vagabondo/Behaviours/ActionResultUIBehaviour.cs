using TMPro;
using UnityEngine;
using Vagabondo.Actions;
using Vagabondo.Managers;
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
                descriptionLabel.text = ((TextActionResult)actionResult).text;
            }
            else if (actionResult is ItemAcquiredActionResult)
            {
                descriptionLabel.text = ((ItemAcquiredActionResult)actionResult).text;
                //TODO: schedule item reveal
            }
        }
    }
}