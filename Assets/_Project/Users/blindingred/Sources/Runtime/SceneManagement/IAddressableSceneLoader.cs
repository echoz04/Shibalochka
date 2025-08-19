using System;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Sources.SceneManagement
{
    public interface IAddressableSceneLoader
    {
        public UniTask LoadScenes(AssetReference[] sceneReferences,LoadSceneMode loadSceneMode, Action onComplete = null);
        public UniTask LoadScene(AssetReference sceneReference,LoadSceneMode loadSceneMode, Action onComplete = null);
        public UniTaskVoid ActivateScene(string sceneName, bool unloadPreviousScene);
        public UniTaskVoid ActivateAllScenes(bool unloadPreviousScene);
    }
}