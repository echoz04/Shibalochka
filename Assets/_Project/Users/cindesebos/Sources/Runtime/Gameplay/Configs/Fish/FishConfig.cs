using UnityEngine;

namespace Sources.Runtime.Gameplay.Configs.Fish
{
    [CreateAssetMenu(fileName = "Fish Config", menuName = "Configs/New Fish Config")]
    public class FishConfig : ScriptableObject
    {
        public string TypeId;
        public string Title;
        public Sprite Icon;
        public Rarity Rarity;
        public Vector2Int Size;
        public GameObject DraggableFishPrefab;
    }
}
