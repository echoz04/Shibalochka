using System.Collections.Generic;
using Sources.Runtime.Gameplay.Configs.Fish;
using UnityEngine;

namespace Sources.Runtime.Gameplay.Configs
{
    [CreateAssetMenu(fileName = "Global Project Config", menuName = "Configs/New Global Project Config")]
    public class ProjectConfig : ScriptableObject
    {
        [field: SerializeField] public CameraConfig CameraConfig { get; private set; } = new CameraConfig();
        [field: Space(17.5f)]
        [field: SerializeField] public UIConfig UIConfig { get; private set; } = new UIConfig();
        [field: Space(17.5f)]
        [field: SerializeField] public InventoryConfig InventoryConfig { get; private set; } = new InventoryConfig();
        [field: Space(17.5f)]
        [field: SerializeField] public DiscordConfig DiscordConfig { get; private set; } = new DiscordConfig();
    }
}
