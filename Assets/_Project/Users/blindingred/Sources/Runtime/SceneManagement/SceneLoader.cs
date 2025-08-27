using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Sources.SceneManagement
{
    public class SceneLoader : IAddressableSceneLoader
    {
        private readonly Dictionary<object, SceneInstance> _loadedScenes = new();

        public async UniTask LoadScenes(AssetReference[] sceneReferences, LoadSceneMode loadSceneMode, Action onComplete = null)
        {
            foreach (var sceneReference in sceneReferences)
            {
                await LoadScene(sceneReference, loadSceneMode);
            }

            onComplete?.Invoke();
        }

        public async UniTask LoadScene(AssetReference sceneReference, LoadSceneMode loadSceneMode, Action onComplete = null)
        {
            var async = Addressables.LoadSceneAsync(sceneReference.RuntimeKey, loadSceneMode, false);
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

        public async UniTaskVoid ActivateScene(AssetReference sceneReference)
        {
            var sceneInstance = _loadedScenes[sceneReference.RuntimeKey];
            
            await sceneInstance.ActivateAsync();
            SceneManager.SetActiveScene(sceneInstance.Scene);
        }

        public async UniTask UnloadScene(AssetReference sceneReference)
        {
            var sceneInstance = _loadedScenes[sceneReference.RuntimeKey];
            var unloadAsync = SceneManager.UnloadSceneAsync(sceneInstance.Scene);
            if (unloadAsync != null)
            {
                Debug.Log($"Unloading scene \"{sceneInstance.Scene.name}\".");
                await UniTask.WaitUntil(() => unloadAsync.isDone);
                
            }
        }

        public async UniTask UnloadScene(Scene scene)
        {
            var unloadAsync = SceneManager.UnloadSceneAsync(scene);
            if (unloadAsync != null)
            {
                Debug.Log($"Unloading scene \"{scene.name}\".");
                await UniTask.WaitUntil(() => unloadAsync.isDone);
            }
        }
    }
}