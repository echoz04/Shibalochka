using Sources.Runtime.Gameplay.Camera;
using Sources.Runtime.Gameplay.MiniGames.Fishing;
using UnityEngine;
using Zenject;

namespace Sources.Runtime.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private FishingMiniGameBootstrapper _fishingMiniGameBootstrapper;
        [SerializeField] private CameraRotator _cameraRotator;

        public override void InstallBindings()
        {
            BindFishingMiniGameBootstrapper();
            BindCameraRotator();
        }

        private void BindFishingMiniGameBootstrapper()
        {
            Container.Bind<FishingMiniGameBootstrapper>()
                .FromInstance(_fishingMiniGameBootstrapper)
                .AsSingle();
        }

        private void BindCameraRotator()
        {
            Container.Bind<CameraRotator>()
                .FromInstance(_cameraRotator)
                .AsSingle();
        }
    }
}