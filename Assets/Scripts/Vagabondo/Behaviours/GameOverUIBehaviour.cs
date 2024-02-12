using UnityEngine;
using UnityEngine.SceneManagement;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.Behaviours
{
    public class GameOverUIBehaviour : MonoBehaviour
    {

        private void OnEnable()
        {
            EventManager.onGameOver += showView;
        }

        private void OnDisable()
        {
            EventManager.onGameOver -= showView;
        }


        private void showView()
        {
            UnityUtils.ShowUIView(gameObject);
        }

        public void OnRestartClicked()
        {
            //UnityUtils.HideUIView(gameObject);
            var currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }
}