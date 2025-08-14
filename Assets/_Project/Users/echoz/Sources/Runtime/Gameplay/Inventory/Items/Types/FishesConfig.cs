using System.Collections.Generic;
using UnityEngine;

namespace Sources.Runtime.Gameplay.Inventory.Items.Types
{
    [CreateAssetMenu(fileName = "FishesConfig", menuName = "Configs/Items/Fish/New Fishes Config")]
    public class FishesConfig : ScriptableObject
    {
        public IReadOnlyList<FishConfig> AllConfigs => _allConfigs;

        [SerializeField] private List<FishConfig> _allConfigs;
    }
}