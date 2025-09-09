using Sources.Runtime.Gameplay.Camera;
using Sources.Runtime.Gameplay.Configs;
using Sources.Runtime.Gameplay.MiniGames.Fishing;
using Sources.Runtime.Services.AssetLoader;
using Sources.SceneManagement;
using Sources.Signals;
using Sources.UI.Services.Fishing;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Sources
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private ProjectConfig _projectConfig;
        [SerializeField] private ScenesData _scenesData;
        [SerializeField] private FishingConfig _fishingConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            BindSceneLoader(builder);
            RegisterScenesData(builder);
            
            BindInput(builder);
            RegisterCursorHandler(builder);
            
            RegisterProjectConfig(builder);
           
            RegisterSignalBus(builder);

            RegisterFishingGame(builder);
        }

        private void RegisterFishingGame(IContainerBuilder builder)
        {
            builder.Register<FishingGame>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_fishingConfig).AsSelf().AsImplementedInterfaces();
        }

        private void RegisterSignalBus(IContainerBuilder builder)
        {
            builder.Register<ISignalBus, SignalBus>(Lifetime.Singleton);
        }

        private void RegisterScenesData(IContainerBuilder builder)
        {
            builder.RegisterInstance(_scenesData).AsSelf().AsImplementedInterfaces();
        }

        private void RegisterProjectConfig(IContainerBuilder builder)
        {
            builder.RegisterInstance(_projectConfig);
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
        
        private void RegisterCursorHandler(IContainerBuilder builder)
        {
            Debug.Log($"Registering cursor view");
            builder.Register<CursorHandler>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}
