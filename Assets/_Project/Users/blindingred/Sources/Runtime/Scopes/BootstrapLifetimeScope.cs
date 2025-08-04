using Sources._Project.Users.blindingred.Sources.Runtime.Interfaces;
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
            builder.Register<BootstrapService>(Lifetime.Singleton).AsSelf();
            builder.RegisterBuildCallback(BootstrapScenes);
        }

        private async void BootstrapScenes(IObjectResolver resolver)
        {
           var sceneLoader = resolver.Resolve<IAddressableSceneLoader>(); 
           await sceneLoader.LoadScenes(_scenesToLoad, sceneLoader.ActivateAllScenes);
        }
    }
}
