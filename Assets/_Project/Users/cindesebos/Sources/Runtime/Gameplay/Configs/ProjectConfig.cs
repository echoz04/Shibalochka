using System.Collections.Generic;
using Sources.Runtime.Gameplay.Configs.Fish;
using UnityEngine;

namespace Sources.Runtime.Gameplay.Configs
{
    [CreateAssetMenu(fileName = "Global Project Config", menuName = "Configs/New Global Project Config")]
    public class ProjectConfig : ScriptableObject
    {
        public List<RarityConfig> RarityConfigs = new();
        public List<FishConfig> FishConfigs = new();

        [field: SerializeField] public CameraConfig CameraConfig { get; private set; } = new CameraConfig();
        [field: Space(17.5f)]
        [field: SerializeField] public UIConfig UIConfig { get; private set; } = new UIConfig();
    }
}
