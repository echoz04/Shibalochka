using Sources.UI.Services;
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
        }

        private void RegisterUIScreens(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<MenuScreen>().AsSelf().AsImplementedInterfaces();
            builder.RegisterComponentInHierarchy<MainScreen>().AsSelf().AsImplementedInterfaces();
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
    }
}
