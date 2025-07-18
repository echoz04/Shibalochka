using Sources.Runtime.Gameplay.Camera;
using Sources.Runtime.Gameplay.Inventory;
using Sources.Runtime.Gameplay.MiniGames;
using Sources.Runtime.Gameplay.MiniGames.Fishing;
using UnityEngine;
using Zenject;

namespace Sources.Runtime.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private FishingMiniGameBootstrapper _fishingMiniGameBootstrapper;
        [SerializeField] private StaminaHandler _staminaHandler;
        [SerializeField] private InventoryRoot _inventoryRoot;
        [SerializeField] private CameraRotator _cameraRotator;

        public override void InstallBindings()
        {
            BindFishingMiniGameBootstrapper();
            BindCameraRotator();
            BindInventory();
            BindStaminaHandler();
        }

        private void BindFishingMiniGameBootstrapper()
        {
            Container.Bind<FishingMiniGameBootstrapper>()
                .FromInstance(_fishingMiniGameBootstrapper)
                .AsSingle();
        }

        private void BindCameraRotator()
        {
            Container.Bind<CameraRotator>()
                .FromInstance(_cameraRotator)
                .AsSingle();
        }

        private void BindInventory()
        {
            Container.Bind<InventoryRoot>()
                .FromInstance(_inventoryRoot)
                .AsSingle();
        }

        private void BindStaminaHandler()
        {
            Container.Bind<StaminaHandler>()
                .FromInstance(_staminaHandler)
                .AsSingle();
        }
    }
}