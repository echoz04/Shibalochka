using Sources.Runtime.Bootstrap;
using Sources.Runtime.Gameplay.Camera;
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
        }
    }
}
