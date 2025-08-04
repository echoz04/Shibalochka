using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sources._Project.Users.blindingred.Sources.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace blindingred
{
    public class SceneLoader : IAddressableSceneLoader, IInitializable
    {
        private Dictionary<object, SceneInstance> _loadedScenes;

        public void Initialize()
        {
            _loadedScenes = new Dictionary<object, SceneInstance>();
        }

        public async UniTask LoadScenes(AssetReference[] sceneReferences, Action onComplete = null)
        {
            foreach (var sceneReference in sceneReferences)
            {
                await LoadScene(sceneReference);
            }

            onComplete?.Invoke();
        }

        public async UniTask LoadScene(AssetReference sceneReference, Action onComplete = null)
        {
            var async = Addressables.LoadSceneAsync(sceneReference.RuntimeKey, LoadSceneMode.Additive, false);
            await async.Task;

            if (async.Status == AsyncOperationStatus.Succeeded)
            {
                if (_loadedScenes.ContainsKey(sceneReference.RuntimeKey))
                {
                    Debug.LogWarning($"Scene \"{sceneReference.editorAsset.name}\" has already loaded. RuntimeKey <{sceneReference.RuntimeKey}>.");
                    onComplete?.Invoke();
                    return;
                }

                _loadedScenes.Add(sceneReference.RuntimeKey, async.Result);
                
                Debug.Log($"Scene \"{sceneReference.editorAsset.name}\" has been loaded. RuntimeKey <{sceneReference.RuntimeKey}>.");
            }
            else
            {
                Debug.LogError($"Couldn't load scene \"{sceneReference.editorAsset.name}\". RuntimeKey <{sceneReference.RuntimeKey}>.");
            }

            onComplete?.Invoke();
        }

        public async UniTaskVoid ActivateScene(object sceneRuntimeKey)
        {
            var sceneInstance = _loadedScenes[sceneRuntimeKey];
            await sceneInstance.ActivateAsync();
        }

        public void ActivateAllScenes()
        {
            foreach (var sceneInstance in _loadedScenes)
            {
                sceneInstance.Value.ActivateAsync();
            }
        }
    }
}