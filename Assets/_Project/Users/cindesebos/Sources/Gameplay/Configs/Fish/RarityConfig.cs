using UnityEngine;

namespace Sources.Gameplay.Configs.Fish
{
    [CreateAssetMenu(fileName = "Rarity Config", menuName = "Configs/New Rarity Config")]
    public class RarityConfig : ScriptableObject
    {
        public Rarity Rarity;
        public int Chance;
    }

    public enum Rarity
    {
        Common = 0,
        Rare = 1,
        Legendary = 2
    }
}
