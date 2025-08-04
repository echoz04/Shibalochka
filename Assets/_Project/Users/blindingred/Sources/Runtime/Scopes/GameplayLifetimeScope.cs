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
            BindFishingMiniGameBootstrapper(builder);
            BindCameraRotator(builder);
            BindInventory(builder);
            BindStaminaHandler(builder);
            BindItemBuilder(builder);
            BindWallet(builder); 
        }

        private void BindItemBuilder(IContainerBuilder builder)
        {
           builder.Register<ItemBuilder>(Lifetime.Singleton).As<IItemBuilder>().WithParameter("prefab", _itemRootPrefab);
        }

        private void BindWallet(IContainerBuilder builder)
        {
            builder.Register<WalletRoot>(Lifetime.Singleton).AsSelf();
            builder.RegisterComponentInHierarchy<WalletView>().AsSelf();
        }

        private void BindStaminaHandler(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<StaminaHandler>().AsSelf();
        }

        private void BindInventory(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<InventoryRoot>().AsSelf();
            builder.RegisterComponentInHierarchy<InventoryView>().AsSelf();
        }

        private void BindCameraRotator(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<CameraRotator>().AsSelf();
        }

        private void BindFishingMiniGameBootstrapper(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<FishingMiniGameBootstrapper>().AsSelf();
        }
    }
}
