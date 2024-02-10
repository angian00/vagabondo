using UnityEngine;

namespace Vagabondo.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Ventura/Add ScriptableObjects Asset/GameColorConfig")]
    public class GameColorConfig : ScriptableObject
    {
        public Color buildingActionColor;
        public Color eventActionColor;
        public Color questActionColor;
    }
}