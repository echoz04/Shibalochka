using UnityEngine.AddressableAssets;

namespace Sources.Signals
{
    public class LoadingSceneSignal
    {
        public AssetReference SceneToLoad { get; }

        public LoadingSceneSignal(AssetReference sceneToLoad)
        {
            SceneToLoad = sceneToLoad;
        }
    }
}