using blindingred;
using Sources.Runtime.Gameplay.Camera;
using Sources.Runtime.Gameplay.Configs;
using Sources.Runtime.Gameplay.MiniGames.Fishing;
using Sources.Runtime.Services.AssetLoader;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Sources
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private ProjectConfig projectConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            BindSceneLoader(builder);
            BindAssetLoader(builder);
            BindInput(builder);
            BindCursorView(builder);
            BindDiscordOverlayDisplayer(builder);
            BindMiniGameRewardService(builder);
            RegisterProjectConfig(builder);
        }

        private void RegisterProjectConfig(IContainerBuilder builder)
        {
            builder.RegisterInstance(projectConfig);
        }

        private void BindMiniGameRewardService(IContainerBuilder builder)
        {
            builder.Register<MiniGameRewardService>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        }

        private void BindDiscordOverlayDisplayer(IContainerBuilder builder)
        {
            builder.Register<DiscordOverlayDisplayer>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        }

        private void BindSceneLoader(IContainerBuilder builder)
        {
            builder.Register<SceneLoader>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        }

        private void BindInput(IContainerBuilder builder)
        {
            builder.Register<CharacterInput>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        }

        private void BindAssetLoader(IContainerBuilder builder)
        {
            builder.Register<AssetLoader>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        }

        private void BindCursorView(IContainerBuilder builder)
        {
            builder.Register<CursorView>(Lifetime.Singleton).AsSelf();
        }
    }
}
