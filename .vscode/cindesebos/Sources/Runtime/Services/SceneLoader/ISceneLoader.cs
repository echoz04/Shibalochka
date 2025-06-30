using System;
using Cysharp.Threading.Tasks;

namespace Sources.Runtime.Services.SceneLoader
{
    public interface ISceneLoader
    {
        event Action OnLoadingStarted;

        event Action OnLoadingEnded;

        void LoadScene(Scene scene);

        UniTask LoadSceneAsync(Scene scene);
    }
}