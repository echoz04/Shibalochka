using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Sources.SceneManagement
{
    [Serializable]
    public class SceneBinding
    {
        [SerializeField] private string _sceneKey;
        [SerializeField] private AssetReference _scene;
        
        public string SceneKey => _sceneKey;
        public AssetReference Scene => _scene;
    }
}