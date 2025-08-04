using System;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Sources._Project.Users.blindingred.Sources.Runtime.Interfaces
{
    public interface IAddressableSceneLoader
    {
        public UniTask LoadScenes(AssetReference[] sceneReferences, Action onComplete = null);
        public UniTask LoadScene(AssetReference sceneReference, Action onComplete = null);
        public UniTaskVoid ActivateScene(object sceneRuntimeKey);
        public void ActivateAllScenes();
    }
}