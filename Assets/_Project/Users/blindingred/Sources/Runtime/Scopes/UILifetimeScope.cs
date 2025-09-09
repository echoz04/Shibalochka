using Sources.UI.Services;
using Sources.UI.Services.Fishing;
using Sources.UI.Services.Loading;
using Sources.UI.Services.PowerBar;
using VContainer;
using VContainer.Unity;

namespace Sources
{
    public class UILifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<UIScreenManager>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.RegisterEntryPoint<UIEntryPoint>();

            RegisterUIScreens(builder);

            RegisterMenu(builder);
            RegisterHUD(builder);
            RegisterWallet(builder);
            RegisterMap(builder);
            RegisterFade(builder);
            RegisterLoading(builder);
            RegisterPowerBar(builder);
            RegisterFishingMinigame(builder);
        }

        private void RegisterUIScreens(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<MenuScreen>().AsSelf().AsImplementedInterfaces();
            builder.RegisterComponentInHierarchy<MainScreen>().AsSelf().AsImplementedInterfaces();
            builder.RegisterComponentInHierarchy<MapScreen>().AsSelf().AsImplementedInterfaces();
            builder.RegisterComponentInHierarchy<FishingScreen>().AsSelf().AsImplementedInterfaces();
            builder.RegisterComponentInHierarchy<LoadingScreen>().AsSelf().AsImplementedInterfaces();
        }

        private void RegisterMenu(IContainerBuilder builder)
        {
            builder.Register<MenuService>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.RegisterComponentInHierarchy<MenuView>().AsSelf().AsImplementedInterfaces();
        }

        private void RegisterHUD(IContainerBuilder builder)
        {
            builder.Register<HUDService>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.RegisterComponentInHierarchy<HUDView>().AsSelf().AsImplementedInterfaces();
        }

        private void RegisterWallet(IContainerBuilder builder)
        {
            builder.Register<WalletService>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.RegisterComponentInHierarchy<WalletView>().AsSelf().AsImplementedInterfaces();
        }

        private void RegisterMap(IContainerBuilder builder)
        {
            builder.Register<MapService>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.RegisterComponentInHierarchy<MapView>().AsSelf().AsImplementedInterfaces();
        }

        private void RegisterLoading(IContainerBuilder builder)
        {
            builder.Register<LoadingService>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.RegisterComponentInHierarchy<LoadingView>().AsSelf().AsImplementedInterfaces();
        }

        private void RegisterFade(IContainerBuilder builder)
        {
            builder.Register<FadeService>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.RegisterComponentInHierarchy<FadeView>().AsSelf().AsImplementedInterfaces();
        }

        private void RegisterPowerBar(IContainerBuilder builder)
        {
            builder.Register<PowerBarService>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.RegisterComponentInHierarchy<PowerBarView>().AsSelf().AsImplementedInterfaces();
        }
        private void RegisterFishingMinigame(IContainerBuilder builder)
        {
            builder.Register<FishingService>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.RegisterComponentInHierarchy<FishingView>().AsSelf().AsImplementedInterfaces();
        }
    }
}
