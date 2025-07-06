using System.Collections.Generic;
using Sources.Runtime.Gameplay.Configs.Fish;
using UnityEngine;

namespace Sources.Runtime.Gameplay.Configs
{
    [CreateAssetMenu(fileName = "Global Project Config", menuName = "Configs/New Global Project Config")]
    public class ProjectConfig : ScriptableObject
    {
        public List<RarityConfig> RarityConfigs = new();
        public List<ItemConfig> ItemConfigs = new();

        [field: Space(17.5f)]

        [field: SerializeField] public CameraConfig CameraConfig { get; private set; } = new CameraConfig();
        [field: Space(17.5f)]
        [field: SerializeField] public UIConfig UIConfig { get; private set; } = new UIConfig();
        [field: Space(17.5f)]
        [field: SerializeField] public InventoryConfig InventoryConfig { get; private set; } = new InventoryConfig();
    }
}
