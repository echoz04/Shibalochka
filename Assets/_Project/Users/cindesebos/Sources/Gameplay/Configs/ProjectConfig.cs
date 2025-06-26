using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sources.Gameplay.Configs
{
    [CreateAssetMenu(fileName = "Global Project Config", menuName = "Configs/New Global Project Config")]
    public class ProjectConfig : ScriptableObject
    {
        public List<RarityConfig> RarityConfigs = new();
        public List<FishConfig> FishConfigs = new();
    }
}
