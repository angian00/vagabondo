using UnityEngine;

namespace Vagabondo.Behaviours
{
    public class GameStartUIBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float showTime = 0.5f;
        [SerializeField]
        private float fadeOutTime = 0.5f;

        private float cumTime = 0.0f;


        void Update()
        {
            cumTime += Time.deltaTime;
            if (cumTime < showTime)
                return;

            var cg = gameObject.GetComponent<CanvasGroup>();

            var alpha = cg.alpha - (Time.deltaTime / fadeOutTime);
            if (alpha <= 0)
                Destroy(gameObject);
            else
                cg.alpha = alpha;
        }
    }
}