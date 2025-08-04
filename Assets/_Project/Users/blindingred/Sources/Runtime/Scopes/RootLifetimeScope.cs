using blindingred;
using Sources._Project.Users.blindingred.Sources.Runtime.Interfaces;
using Sources.Runtime.Gameplay.Camera;
using Sources.Runtime.Gameplay.MiniGames.Fishing;
using Sources.Runtime.Services.AssetLoader;
using Sources.Runtime.Services.ProjectConfigLoader;
using VContainer;
using VContainer.Unity;

namespace Sources
{
    public class RootLifetimeScope : LifetimeScope
    {
        
        protected override void Configure(IContainerBuilder builder)
        {
            BindSceneLoader(builder);
            BindProjectConfigLoader(builder);
            BindAssetLoader(builder);
            BindInput(builder);
            BindCursorView(builder);
            BindDiscordOverlayDisplayer(builder);
            BindMiniGameRewardService(builder);
        }

        private void BindMiniGameRewardService(IContainerBuilder builder)
        {
            builder.Register<MiniGameRewardService>(Lifetime.Singleton).As<IMiniGameRewardService>();
        }

        private void BindDiscordOverlayDisplayer(IContainerBuilder builder)
        {
            builder.Register<DiscordOverlayDisplayer>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }

        private void BindSceneLoader(IContainerBuilder builder)
        {
            builder.Register<SceneLoader>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }

        private void BindInput(IContainerBuilder builder)
        {
            builder.Register<CharacterInput>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        }

        private void BindAssetLoader(IContainerBuilder builder)
        {
            builder.Register<AssetLoader>(Lifetime.Singleton).As<IAssetLoader>();
        }

        private void BindProjectConfigLoader(IContainerBuilder builder)
        {
            builder.Register<ProjectConfigLoader>(Lifetime.Singleton).As<IProjectConfigLoader>();
        }

        private void BindCursorView(IContainerBuilder builder)
        {
            builder.Register<CursorView>(Lifetime.Singleton).AsSelf();
        }
    }
}
