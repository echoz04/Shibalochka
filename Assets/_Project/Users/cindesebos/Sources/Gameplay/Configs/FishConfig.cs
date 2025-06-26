using UnityEngine;

namespace Sources.Gameplay.Configs
{
    [CreateAssetMenu(fileName = "Fish Config", menuName = "Configs/New Fish Config")]
    public class FishConfig : ScriptableObject
    {
        public string TypeId;
        public string Title;
        public Sprite Icon;
        public Rarity Rarity;
    }
}
