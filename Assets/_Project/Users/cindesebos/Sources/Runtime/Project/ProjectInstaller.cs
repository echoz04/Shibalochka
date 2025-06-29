using Zenject;
using Sources.Runtime.Services.SceneLoader;
using Sources.Runtime.Services.AssetLoader;
using Sources.Runtime.Services.ProjectConfigLoader;
using UnityEngine;
using Sources.Runtime.Gameplay.Camera;

namespace Sources.Runtime.Project
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private CursorHandler _cursorHandler;

        public override void InstallBindings()
        {
            BindSceneLoader();
            BindProjectConfigLoader();
            BindAssetLoader();
            BindInput();
            BindCursorHandler();
        }

        private void BindSceneLoader()
        {
            Container.Bind<ISceneLoader>()
                .To<SceneLoader>()
                .AsSingle();
        }

        private void BindProjectConfigLoader()
        {
            Container.Bind<IProjectConfigLoader>()
                .To<ProjectConfigLoader>()
                .AsSingle();
        }

        private void BindAssetLoader()
        {
            Container.Bind<IAssetLoader>()
                .To<AssetLoader>()
                .AsSingle();
        }

        private void BindInput()
        {
            Container.BindInterfacesAndSelfTo<CharacterInput>()
                .AsSingle();
        }

        private void BindCursorHandler()
        {
            Container.Bind<CursorHandler>()
                .FromInstance(_cursorHandler)
                .AsSingle();
        }
    }
}
