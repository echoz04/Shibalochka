using Sources.Runtime.Gameplay;
using Sources.Runtime.Gameplay.Camera;
using Sources.Runtime.Gameplay.Inventory;
using Sources.Runtime.Gameplay.Inventory.Item;
using Sources.Runtime.Gameplay.MiniGames.Fishing;
using Sources.Runtime.Gameplay.Wallet;
using Sources.Runtime.Services.Builders.Item;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Sources
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private ItemRoot _itemRootPrefab;
        
        protected override void Configure(IContainerBuilder builder)
        {
            BindGameplayEntryPoint(builder);
            BindFishingMiniGameBootstrapper(builder);
            BindCameraRotator(builder);
            BindInventory(builder);
            BindStaminaHandler(builder);
            BindItemBuilder(builder);
            BindWallet(builder);
            BindMiniGameRewardService(builder);
        }
        
        private void BindMiniGameRewardService(IContainerBuilder builder)
        {
            builder.Register<MiniGameRewardService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }

        private void BindGameplayEntryPoint(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameplayEntryPoint>();
        }

        private void BindItemBuilder(IContainerBuilder builder)
        {
           builder.Register<ItemBuilder>(Lifetime.Singleton).AsImplementedInterfaces().WithParameter("prefab", _itemRootPrefab);
        }

        private void BindWallet(IContainerBuilder builder)
        {
            builder.Register<WalletRoot>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.RegisterComponentInHierarchy<WalletView>().AsImplementedInterfaces().AsSelf();
        }

        private void BindStaminaHandler(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<StaminaHandler>().AsImplementedInterfaces().AsSelf();
        }

        private void BindInventory(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<InventoryRoot>().AsImplementedInterfaces().AsSelf();
            builder.RegisterComponentInHierarchy<InventoryView>().AsImplementedInterfaces().AsSelf();
        }

        private void BindCameraRotator(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<CameraRotator>().AsImplementedInterfaces().AsSelf();
        }

        private void BindFishingMiniGameBootstrapper(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<FishingMiniGameBootstrapper>().AsImplementedInterfaces().AsSelf();
        }
    }
}
