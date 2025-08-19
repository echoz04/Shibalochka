using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Sources.SceneManagement
{
    [CreateAssetMenu(fileName = "ScenesData", menuName = "Data/ScenesData")]
    public class ScenesData : ScriptableObject
    {
        [SerializeField] private SceneBinding[] _sceneBindings;
        [SerializeField] private AssetReference[] _scenes;
        public SceneBinding[] Scenes => _sceneBindings;
    }
}