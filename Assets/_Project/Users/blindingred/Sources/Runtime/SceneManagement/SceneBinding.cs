using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Sources.SceneManagement
{
    [Serializable]
    public class SceneBinding
    {
        [SerializeField] private SceneKey _sceneKey;
        [SerializeField] private AssetReference _scene;
        
        public SceneKey SceneKey => _sceneKey;
        public AssetReference Scene => _scene;
    }
}