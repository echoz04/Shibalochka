using Sources.Runtime.Gameplay.Inventory;
using Sources.Runtime.Services.SceneLoader;
using UnityEngine;
using Zenject;

namespace Sources.Runtime.Bootstrap
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private Scene _sceneToLoad;

        public override void InstallBindings()
        {
            Container.Bind<Scene>()
                .FromInstance(_sceneToLoad)
                .AsSingle();

            Container.BindInterfacesAndSelfTo<BootstrapService>()
                .AsSingle();
        }
    }
}
