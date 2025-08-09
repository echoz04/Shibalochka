using System.Collections.Generic;
using Sources.Runtime.Gameplay.Configs.Fish;
using UnityEngine;

namespace Sources.Runtime.Gameplay.Configs.Items
{
        [System.Serializable]
        public class ItemsConfig
        {
#if UNITY_EDITOR
                public List<ItemConfig> Configs => _configs;
                public List<RaritiesConfig> RaritiesConfig => _raritiesConfig;
#else
                public IEnumerable<ItemConfig> Configs => _configs;
                public IEnumerable<RaritiesConfig> RaritiesConfig => _raritiesConfig;
#endif

                [SerializeField] private List<ItemConfig> _configs = new();
                [SerializeField] private List<RaritiesConfig> _raritiesConfig = new(3);
        }
}