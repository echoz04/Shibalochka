
using Cysharp.Threading.Tasks;
using Sources._Project.Users.blindingred.Sources.Runtime.Interfaces;
using Sources.Runtime.Gameplay.Configs;
using Sources.Runtime.Services.AssetLoader;
using UnityEngine;
using Sources.Runtime.Gameplay.MiniGames.Fishing;
using Sources.Runtime.Project;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Sources.Runtime.Bootstrap
{
    public class BootstrapService : IInitializable
    {
        private readonly IAssetLoader _assetLoader;
        private readonly IAddressableSceneLoader _sceneLoader;
        private readonly ProjectConfig _projectConfig;
        private readonly AssetReference _sceneToLoad;
        private readonly DiscordOverlayDisplayer _discordOverlayDisplayer;
        private readonly IMiniGameRewardService _miniGameRewardService;
        
        [Inject]
        public BootstrapService(
            IAssetLoader assetLoader, 
            IAddressableSceneLoader sceneLoader, 
            ProjectConfig projectConfig, 
            [Key("scene_menu")] AssetReference sceneToLoad,
            DiscordOverlayDisplayer discordOverlayDisplayer, 
            IMiniGameRewardService miniGameRewardService)
        {
            _assetLoader = assetLoader;
            _sceneLoader = sceneLoader;
            _projectConfig = projectConfig;
            _sceneToLoad = sceneToLoad;
            _discordOverlayDisplayer = discordOverlayDisplayer;
            _miniGameRewardService = miniGameRewardService;
        }

        async void IInitializable.Initialize()
        {
            // using (await _assetLoader.LoadDisposable<GameObject>(AssetsConstants.LoadingCanvas))
            // {
                _discordOverlayDisplayer.Initialize();
                _miniGameRewardService.Initialize();
                
#if UNITY_EDITOR
                 ContentManagementSystem.Instance.ProjectConfig = _projectConfig;

                // Debug.Log($"Loading scene: {ContentManagementSystem.Instance.SceneToLoad}");
                //
                // await _sceneLoader.LoadSceneAsync(ContentManagementSystem.Instance.SceneToLoad);
#else
                // await _sceneLoader.LoadSceneAsync(_sceneToLoad);
           
#endif
            await BootstrapScenes();
            // }
        }
        
        private async UniTask BootstrapScenes()
        {
            await _sceneLoader.LoadScene(
                _sceneToLoad, 
                LoadSceneMode.Additive,
                () => _sceneLoader.ActivateAllScenes(true));
        }
    }
}
