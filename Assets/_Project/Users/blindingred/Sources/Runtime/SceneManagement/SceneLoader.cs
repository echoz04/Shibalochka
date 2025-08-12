using System;
using System.Collections.Generic;
using System.Linq;
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

        public async UniTaskVoid ActivateScene(string sceneName, bool unloadPreviousScene)
        {
            var sceneToUnload = SceneManager.GetActiveScene();
            
            var sceneInstance = _loadedScenes.First(s => s.Value.Scene.name == sceneName).Value;
            
            await sceneInstance.ActivateAsync();
            
            if (SceneManager.GetActiveScene().name == sceneToUnload.name)
            {
                SceneManager.SetActiveScene(sceneInstance.Scene);
            }
            
            if (unloadPreviousScene)
            {
                await UnloadScene(sceneToUnload);
            }
        }

        public async UniTaskVoid ActivateAllScenes(bool unloadPreviousScene)
        {
            var sceneToUnload = SceneManager.GetActiveScene();
            
            foreach (var sceneInstance in _loadedScenes)
            {
                await sceneInstance.Value.ActivateAsync();
                
                if (SceneManager.GetActiveScene().name == sceneToUnload.name)
                {
                    SceneManager.SetActiveScene(sceneInstance.Value.Scene);
                }
            }
            
            if (unloadPreviousScene)
            {
                await UnloadScene(sceneToUnload);
            }
        }

        private async UniTask UnloadScene(Scene sceneToUnload)
        {
            var unloadAsync = SceneManager.UnloadSceneAsync(sceneToUnload);
            if (unloadAsync != null)
            {
                Debug.Log($"Unloading scene \"{sceneToUnload.name}\".");
                await UniTask.WaitUntil(() => unloadAsync.isDone);
                
            }
        } 
    }
}