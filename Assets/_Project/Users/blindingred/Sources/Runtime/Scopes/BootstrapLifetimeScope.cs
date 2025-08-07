using Sources.Runtime.Bootstrap;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;

namespace Sources
{
    public class BootstrapLifetimeScope : LifetimeScope
    {
        [SerializeField] private AssetReference[] _scenesToLoad;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<BootstrapService>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            RegisterScenes(builder);
        }

        private void RegisterScenes(IContainerBuilder builder)
        {
            foreach (var scene in _scenesToLoad)
            {
                builder.RegisterInstance(scene).As<AssetReference>().Keyed(scene.editorAsset.name);
            }
        }
    }
}
