
using Sources.Runtime.Services.AssetLoader;
using Sources.Runtime.Services.SceneLoader;
using UnityEngine;
using Sources.Runtime.Services.ProjectConfigLoader;
using Sources.Runtime.Gameplay.MiniGames.Fishing;
using Sources.Runtime.Project;
using VContainer;
using VContainer.Unity;

namespace Sources.Runtime.Bootstrap
{
    public class BootstrapService : IInitializable
    {
        private readonly IAssetLoader _assetLoader;
        private readonly ISceneLoader _sceneLoader;
        private readonly IProjectConfigLoader _projectConfigLoader;
        private readonly Scene _sceneToLoad;
        private readonly DiscordOverlayDisplayer _discordOverlayDisplayer;
        private readonly IMiniGameRewardService _miniGameRewardService;
        
        [Inject]
        public BootstrapService(IAssetLoader assetLoader, ISceneLoader sceneLoader, IProjectConfigLoader projectConfigLoader, Scene sceneToLoad,
        DiscordOverlayDisplayer discordOverlayDisplayer, IMiniGameRewardService miniGameRewardService)
        {
            _assetLoader = assetLoader;
            _sceneLoader = sceneLoader;
            _projectConfigLoader = projectConfigLoader;
            _sceneToLoad = sceneToLoad;
            _discordOverlayDisplayer = discordOverlayDisplayer;
            _miniGameRewardService = miniGameRewardService;
        }

        public async void Initialize()
        {
            using (await _assetLoader.LoadDisposable<GameObject>(AssetsConstants.LoadingCanvas))
            {
                await _projectConfigLoader.LoadProjectConfigAsync();
                _discordOverlayDisplayer.Initialize();
                _miniGameRewardService.Initialize();
#if UNITY_EDITOR
                ContentManagementSystem.Instance.ProjectConfigLoader = _projectConfigLoader;

                Debug.Log($"Loading scene: {ContentManagementSystem.Instance.SceneToLoad}");

                await _sceneLoader.LoadSceneAsync(ContentManagementSystem.Instance.SceneToLoad);
#else
                await _sceneLoader.LoadSceneAsync(_sceneToLoad);
#endif
            }
        }
    }
}
