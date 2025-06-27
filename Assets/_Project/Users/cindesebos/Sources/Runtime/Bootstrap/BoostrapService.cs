using Zenject;
using Cysharp.Threading.Tasks;
using Sources.Runtime.Services.AssetLoader;
using Sources.Runtime.Services.SceneLoader;
using UnityEngine;

namespace Sources.Runtime.Bootstrap
{
    public class BootstrapService : IInitializable
    {
        private readonly IAssetLoader _assetLoader;
        private readonly ISceneLoader _sceneLoader;
        private readonly Scene _sceneToLoad;

        public BootstrapService(IAssetLoader assetLoader, ISceneLoader sceneLoader, Scene sceneToLoad)
        {
            _assetLoader = assetLoader;
            _sceneLoader = sceneLoader;
            _sceneToLoad = sceneToLoad;
        }

        public async void Initialize()
        {
            using (var loadingCanvas = await _assetLoader.LoadDisposable<GameObject>(AssetsConstants.LoadingCanvas))
            {
                await _sceneLoader.LoadSceneAsync(_sceneToLoad);
            }
        }
    }
}
