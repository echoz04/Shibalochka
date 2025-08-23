using Cysharp.Threading.Tasks;
using Sources.Runtime.Gameplay.MiniGames.Fishing;
using Sources.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Sources.Runtime.Bootstrap
{
    public class BootstrapService : IInitializable
    {
        private readonly IAddressableSceneLoader _sceneLoader;
        private readonly ScenesData _scenesData;
        private readonly DiscordOverlayDisplayer _discordOverlayDisplayer;
        private readonly IMiniGameRewardService _miniGameRewardService;
        
        [Inject]
        public BootstrapService(
            IAddressableSceneLoader sceneLoader, 
            ScenesData scenesData,
            DiscordOverlayDisplayer discordOverlayDisplayer, 
            IMiniGameRewardService miniGameRewardService)
        {
            _sceneLoader = sceneLoader;
            _scenesData = scenesData;
            _discordOverlayDisplayer = discordOverlayDisplayer;
            _miniGameRewardService = miniGameRewardService;
        }

        async void IInitializable.Initialize()
        {
                // _discordOverlayDisplayer.Initialize();
                _miniGameRewardService.Initialize();
                await BootstrapScenes();
        }
        
        private async UniTask BootstrapScenes()
        {
            var uiScene = _scenesData.GetSceneBindingByKey(SceneKey.UI).Scene;
            var menuScene = _scenesData.GetSceneBindingByKey(SceneKey.Menu).Scene;
            var bootsTrapScene = _scenesData.GetSceneBindingByKey(SceneKey.Bootstrap).Scene;
            
            var scenesToLoad = new [] {
                uiScene,
                menuScene
            };
            await _sceneLoader.LoadScenes(
                scenesToLoad, 
                LoadSceneMode.Additive,
                () =>
                {
                    _sceneLoader.ActivateScene(menuScene);
                    _sceneLoader.UnloadScene(bootsTrapScene);
                });
        }
    }
}
