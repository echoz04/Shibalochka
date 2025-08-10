using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

namespace Sources.Runtime.Services.SceneLoader
{
    public class SceneLoader : ISceneLoader
    {
        public event Action OnLoadingStarted;

        public event Action OnLoadingEnded;

        public void LoadScene(Scene scene) => LoadSceneAsync(scene).Forget();

        public async UniTask LoadSceneAsync(Scene scene)
        {
            try
            {
                OnLoadingStarted?.Invoke();

                string sceneName = scene.ToString();

                AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

                while (!loadSceneOperation.isDone) await UniTask.Yield();

                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            }
            finally
            {
                OnLoadingEnded?.Invoke();
            }
        }
    }
}