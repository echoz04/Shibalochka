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
        private readonly AssetReference[] _scenesToLoad;
        private readonly DiscordOverlayDisplayer _discordOverlayDisplayer;
        private readonly IMiniGameRewardService _miniGameRewardService;
        
        [Inject]
        public BootstrapService(
            IAddressableSceneLoader sceneLoader, 
            [Key(SceneKey.UI)] AssetReference uiScene,
            [Key(SceneKey.Menu)] AssetReference menuScene,
            DiscordOverlayDisplayer discordOverlayDisplayer, 
            IMiniGameRewardService miniGameRewardService)
        {
            _sceneLoader = sceneLoader;
            _discordOverlayDisplayer = discordOverlayDisplayer;
            _miniGameRewardService = miniGameRewardService;

            _scenesToLoad = new[]
            {
                uiScene,
                menuScene,
            };
        }

        async void IInitializable.Initialize()
        {
                _discordOverlayDisplayer.Initialize();
                _miniGameRewardService.Initialize();
                await BootstrapScenes();
        }
        
        private async UniTask BootstrapScenes()
        {
            await _sceneLoader.LoadScenes(
                _scenesToLoad, 
                LoadSceneMode.Additive,
                () => _sceneLoader.ActivateAllScenes(true));
        }
    }
}
