using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Vagabondo.DataModel;

namespace Vagabondo.Behaviours
{
    public class MemoryItemBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public static int maxTitleLength = 20;
        public static int maxDescriptionLength = 240;

        [SerializeField]
        private TextMeshProUGUI titleLabel;
        [SerializeField]
        private GameObject tooltipObj;
        [SerializeField]
        private TextMeshProUGUI descriptionLabel;


        private Memory _memory;
        public Memory Memory
        {
            set
            {
                _memory = value;

                if (_memory.title.Length > maxTitleLength)
                    Debug.LogWarning($"Action title too long for memory {_memory.title}");

                if (_memory.description.Length > maxDescriptionLength)
                    Debug.LogWarning($"Action description too long for memory {_memory.title}");

                titleLabel.text = _memory.title;
                descriptionLabel.text = _memory.description;
            }
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            //Debug.Log("MemoryItemBehaviour.OnPointerEnter");

            tooltipObj.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltipObj.SetActive(false);
        }

    }
}