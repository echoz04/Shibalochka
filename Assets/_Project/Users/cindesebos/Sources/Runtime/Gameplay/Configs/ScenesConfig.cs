using System;
using Sirenix.OdinInspector;
using Sources.SceneManagement;
using UnityEngine;

namespace Sources.Runtime.Gameplay.Configs
{
    [Serializable]
    public class ScenesConfig
    {
        [Title("Scenes configuration"), LabelText("Scene type bindings")] [SerializeField]
        private SceneBinding[] _sceneBindings;
        
        public SceneBinding[] SceneBindings => _sceneBindings;
    }
}