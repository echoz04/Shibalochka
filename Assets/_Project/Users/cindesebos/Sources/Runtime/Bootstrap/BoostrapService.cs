using Zenject;
using Cysharp.Threading.Tasks;
using Sources.Runtime.Services.AssetLoader;
using Sources.Runtime.Services.SceneLoader;
using UnityEngine;
using Sources.Runtime.Services.ProjectConfigLoader;
using Sources.Runtime.Gameplay.Inventory;

namespace Sources.Runtime.Bootstrap
{
    public class BootstrapService : IInitializable
    {
        private readonly IAssetLoader _assetLoader;
        private readonly ISceneLoader _sceneLoader;
        private readonly IProjectConfigLoader _projectConfigLoader;
        private readonly Scene _sceneToLoad;
        private DiscordOverlayDisplayer _discordOverlayDisplayer;

        public BootstrapService(IAssetLoader assetLoader, ISceneLoader sceneLoader, IProjectConfigLoader projectConfigLoader, Scene sceneToLoad,
        DiscordOverlayDisplayer discordOverlayDisplayer)
        {
            _assetLoader = assetLoader;
            _sceneLoader = sceneLoader;
            _projectConfigLoader = projectConfigLoader;
            _sceneToLoad = sceneToLoad;
            _discordOverlayDisplayer = discordOverlayDisplayer;
        }

        public async void Initialize()
        {
            using (await _assetLoader.LoadDisposable<GameObject>(AssetsConstants.LoadingCanvas))
            {
                await _projectConfigLoader.LoadProjectConfigAsync();
                _discordOverlayDisplayer.Initialize();
                await _sceneLoader.LoadSceneAsync(_sceneToLoad);
            }
        }
    }
}
