using UnityEngine.AddressableAssets;

namespace Sources.Signals
{
    public struct LoadingSceneSignal
    {
        public AssetReference SceneToLoad { get; }

        public LoadingSceneSignal(AssetReference sceneToLoad)
        {
            SceneToLoad = sceneToLoad;
        }
    }
}