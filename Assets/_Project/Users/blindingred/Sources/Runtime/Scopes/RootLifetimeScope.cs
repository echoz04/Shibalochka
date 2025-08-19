using Sources.Runtime.Gameplay.Camera;
using Sources.Runtime.Gameplay.Configs;
using Sources.Runtime.Gameplay.MiniGames.Fishing;
using Sources.Runtime.Services.AssetLoader;
using Sources.SceneManagement;
using Sources.Signals;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;

namespace Sources
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private ProjectConfig _projectConfig;
        [SerializeField] private ScenesData _scenesData;
        
        protected override void Configure(IContainerBuilder builder)
        {
            BindSceneLoader(builder);
            BindInput(builder);
            
            BindDiscordOverlayDisplayer(builder);
            BindMiniGameRewardService(builder);
            
            RegisterProjectConfig(builder);
            RegisterScenes(builder);
            RegisterSignalBus(builder);
            RegisterCursorHandler(builder);
        }

        private void RegisterSignalBus(IContainerBuilder builder)
        {
            builder.Register<ISignalBus, SignalBus>(Lifetime.Singleton);
        }

        private void RegisterScenes(IContainerBuilder builder)
        {
            foreach (var sceneBinding in _scenesData.Scenes)
            {
                builder.RegisterInstance(sceneBinding.Scene).As<AssetReference>().Keyed(sceneBinding.SceneKey);
            }
        }

        private void RegisterProjectConfig(IContainerBuilder builder)
        {
            builder.RegisterInstance(_projectConfig);
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
        
        private void RegisterCursorHandler(IContainerBuilder builder)
        {
            Debug.Log($"Registering cursor view");
            builder.Register<CursorHandler>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}
