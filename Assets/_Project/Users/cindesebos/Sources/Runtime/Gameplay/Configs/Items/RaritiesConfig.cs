using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Sources.Runtime.Gameplay.Configs.Fish
{
    [System.Serializable]
    public class RaritiesConfig
    {
        [Title("Rarity Settings")]

        [field: SerializeField, LabelText("Rarity")] private Rarity _rarity;
        public Rarity Rarity => _rarity;
        [field: Space(4f)]

        [field: SerializeField, MinValue(1), MaxValue(100), LabelText("Chance")] public int _chance;
        public int Chance => _chance;
        [field: Space(4f)]

        [field: SerializeField, MinValue(1), MaxValue(100), LabelText("Mutant Version Chance")] public int _mutantChance;
        public int MutantChance => _mutantChance;
    }

    public class RarityConfig
    {
        public Rarity Rarity;
        public int Chance;
    }

    public enum Rarity
    {
        Common = 0,
        Mythical = 1,
        Leviathan = 2
    }
}
