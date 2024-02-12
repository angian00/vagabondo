using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vagabondo.Actions;
using Vagabondo.DataModel;
using Vagabondo.Managers;
using Vagabondo.ScriptableObjects;
using Button = UnityEngine.UI.Button;

namespace Vagabondo.Behaviours
{
    public class ActionButtonBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public static int maxTitleLength = 35;
        public static int maxDescriptionLength = 125;

        [Header("UI Fields")]
        [SerializeField]
        private Image backgroundImage;
        [SerializeField]
        private TextMeshProUGUI titleLabel;
        [SerializeField]
        private TextMeshProUGUI descriptionLabel;
        [SerializeField]
        private GameObject tooltipObj;
        [SerializeField]
        private TextMeshProUGUI tooltipLabel;


        [Header("Scriptable Objects")]
        [SerializeField]
        private GameColorConfig colorConfig;


        private GameAction _action;
        public GameAction Action
        {
            set
            {
                _action = value;

                if (_action.title.Length > maxTitleLength)
                    Debug.LogWarning($"Action title too long for action {_action.title}");

                if (_action.description.Length > maxDescriptionLength)
                    Debug.LogWarning($"Action description too long for action {_action.title}");

                if (_action.isShopAction())
                    backgroundImage.color = colorConfig.shopActionColor;
                if (_action.isBuildingAction())
                    backgroundImage.color = colorConfig.buildingActionColor;
                else if (_action.isEventAction())
                    backgroundImage.color = colorConfig.eventActionColor;
                else if (_action.isQuestAction())
                    backgroundImage.color = colorConfig.questActionColor;

                titleLabel.text = _action.title;
                descriptionLabel.text = _action.description;
            }
        }

        private bool _actionSpent = false;
        private bool _canPerform;


        public void OnPointerEnter(PointerEventData eventData)
        {
            //Debug.Log("ActionButtonBehaviour.OnPointerEnter");

            if (_canPerform || _actionSpent)
                return;

            tooltipLabel.text = _action.GetCantPerformMessage();
            tooltipObj.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltipObj.SetActive(false);
        }


        public void ComputeInteractable(Traveler travelerData)
        {
            _canPerform = _action.CanPerform(travelerData);
            gameObject.GetComponent<Button>().interactable = (_canPerform && !_actionSpent);
        }


        public void OnActionClicked()
        {
            _actionSpent = true;
            gameObject.GetComponent<Button>().interactable = false;
            TravelManager.Instance.PerformAction(_action);
        }

    }
}