using System;
using UnityEngine;

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


        public static void ShowUIView(GameObject viewObj)
        {
            ToggleUIView(viewObj, true);
        }

        public static void HideUIView(GameObject viewObj)
        {
            ToggleUIView(viewObj, false);
        }

        public static void ToggleUIView(GameObject viewObj, bool? proposedVisibilityStatus = null)
        {
            var targetCanvasGroup = viewObj.GetComponent<CanvasGroup>();
            if (targetCanvasGroup == null)
                throw new Exception($"viewObj {viewObj.name} does not have a CanvasGroup");

            bool newVisibilityStatus;
            if (proposedVisibilityStatus == null)
                newVisibilityStatus = !(targetCanvasGroup.interactable);
            else
                newVisibilityStatus = (bool)proposedVisibilityStatus;

            targetCanvasGroup.alpha = newVisibilityStatus ? 1 : 0;
            targetCanvasGroup.interactable = newVisibilityStatus;
            targetCanvasGroup.blocksRaycasts = newVisibilityStatus;
        }

    }
}
