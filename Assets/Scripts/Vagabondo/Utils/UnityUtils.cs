using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vagabondo.Utils
{
    public class UnityUtils
    {
        public static void RemoveAllChildren(Transform parent)
        {
            foreach (Transform child in parent)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        public static Color? ColorFromHex(string hexStr)
        {
            if (hexStr == null)
                return null;

            Color c;
            if (ColorUtility.TryParseHtmlString(hexStr, out c))
                return c;

            return null;
        }

        public static Color ColorFromHash(int objHash)
        {
            float r = Mathf.Abs(Mathf.Sin(objHash * 0.123f));
            float g = Mathf.Abs(Mathf.Cos(objHash * 0.456f));
            float b = Mathf.Abs(Mathf.Sin(objHash * 0.789f));
            return new Color(r, g, b);
        }

        public static void InitUIViews()
        {
            var rootObjs = SceneManager.GetActiveScene().GetRootGameObjects();
            var canvas = rootObjs.First(obj => obj.name == "Canvas").transform;

            foreach (Transform uiView in canvas.transform)
            {
                if (uiView.name == "Town UI" || uiView.name == "Game Start UI")
                    ShowUIView(uiView.gameObject);
                else
                    HideUIView(uiView.gameObject);
            }
        }


        public static void ShowUIView(GameObject viewObj)
        {
            toggleUIView(viewObj, true);
        }

        public static void HideUIView(GameObject viewObj)
        {
            toggleUIView(viewObj, false);
        }

        private static void toggleUIView(GameObject viewObj, bool newStatus)
        {
            var targetCanvasGroup = viewObj.GetComponent<CanvasGroup>();
            if (targetCanvasGroup == null)
                throw new Exception($"viewObj {viewObj.name} does not have a CanvasGroup");

            targetCanvasGroup.alpha = newStatus ? 1 : 0;
            targetCanvasGroup.interactable = newStatus;
            targetCanvasGroup.blocksRaycasts = newStatus;
        }

    }
}
