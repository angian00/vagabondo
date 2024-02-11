using TMPro;
using UnityEngine;
using Vagabondo.DataModel;
using Vagabondo.Utils;

namespace Vagabondo.Behaviours
{
    public class DominionUIBehaviour : MonoBehaviour
    {
        [Header("UI Fields")]
        [SerializeField]
        private TextMeshProUGUI descriptionLabel;

        private Dominion _dominion;
        public Dominion Dominion
        {
            set
            {
                _dominion = value;
                updateView();
            }
        }

        public void OnCloseClicked()
        {
            UnityUtils.HideUIView(gameObject);
        }

        private void updateView()
        {
            descriptionLabel.text = _dominion.name;
        }
    }
}