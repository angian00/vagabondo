using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Button = UnityEngine.UI.Button;

namespace Vagabondo.Behaviours
{
    public class ActionButtonBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public static int maxTitleLength = 20;
        public static int maxDescriptionLength = 60;

        [SerializeField]
        private TextMeshProUGUI titleLabel;
        [SerializeField]
        private TextMeshProUGUI descriptionLabel;
        [SerializeField]
        private GameObject tooltipObj;
        [SerializeField]
        private TextMeshProUGUI tooltipLabel;


        private GameAction _action;
        public GameAction Action
        {
            set
            {
                _action = value;

                Debug.Assert(_action.title.Length <= maxTitleLength);
                Debug.Assert(_action.description.Length <= maxDescriptionLength);

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


        public void ComputeInteractable(TravelerData travelerData)
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