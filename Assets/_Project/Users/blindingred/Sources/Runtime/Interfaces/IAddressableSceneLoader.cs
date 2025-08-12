using System;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Sources._Project.Users.blindingred.Sources.Runtime.Interfaces
{
    public interface IAddressableSceneLoader
    {
        public UniTask LoadScenes(AssetReference[] sceneReferences,LoadSceneMode loadSceneMode, Action onComplete = null);
        public UniTask LoadScene(AssetReference sceneReference,LoadSceneMode loadSceneMode, Action onComplete = null);
        public UniTaskVoid ActivateScene(string sceneName, bool unloadPreviousScene);
        public UniTaskVoid ActivateAllScenes(bool unloadPreviousScene);
    }
}