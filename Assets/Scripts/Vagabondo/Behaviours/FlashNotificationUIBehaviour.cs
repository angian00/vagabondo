using TMPro;
using UnityEngine;
using Vagabondo.Managers;

namespace Vagabondo.Behaviours
{
    public class FlashNotificationUIBehaviour : MonoBehaviour
    {
        [Header("Behaviour Params")]
        [SerializeField]
        private float showTime = 0.5f;
        [SerializeField]
        private float fadeOutTime = 0.5f;

        [Header("UI Fields")]
        [SerializeField]
        private TextMeshProUGUI notificationLabel;

        private float cumTime = 0.0f;

        private CanvasGroup _cg;


        private void OnEnable()
        {
            EventManager.onTextNotification += onTextNotification;
        }

        private void OnDisable()
        {
            EventManager.onTextNotification -= onTextNotification;
        }

        void Start()
        {
            _cg = gameObject.GetComponent<CanvasGroup>();
        }


        void Update()
        {
            if (_cg.alpha <= 0)
                return;

            cumTime += Time.deltaTime;
            if (cumTime < showTime)
                return;

            var alpha = _cg.alpha - (Time.deltaTime / fadeOutTime);
            if (alpha >= 0)
                _cg.alpha = alpha;
        }

        public void onTextNotification(string message)
        {
            notificationLabel.text = message;

            cumTime = 0;
            _cg.alpha = 1;
        }
    }
}