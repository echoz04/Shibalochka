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
        private readonly InventoryService _inventoryService;

        public BootstrapService(IAssetLoader assetLoader, ISceneLoader sceneLoader, IProjectConfigLoader projectConfigLoader, Scene sceneToLoad,
        InventoryService inventoryService)
        {
            _assetLoader = assetLoader;
            _sceneLoader = sceneLoader;
            _projectConfigLoader = projectConfigLoader;
            _sceneToLoad = sceneToLoad;
            _inventoryService = inventoryService;
        }

        public async void Initialize()
        {
            using (var loadingCanvas = await _assetLoader.LoadDisposable<GameObject>(AssetsConstants.LoadingCanvas))
            {
                await _projectConfigLoader.LoadProjectConfigAsync();
                _inventoryService.SetConfig(_projectConfigLoader);
                _inventoryService.BuildGrid();
                await _sceneLoader.LoadSceneAsync(_sceneToLoad);
            }
        }
    }
}
