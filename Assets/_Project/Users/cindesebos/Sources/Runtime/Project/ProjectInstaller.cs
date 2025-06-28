using Zenject;
using Sources.Runtime.Services.SceneLoader;
using Sources.Runtime.Services.AssetLoader;
using Sources.Runtime.Services.ProjectConfigLoader;

namespace Sources.Runtime.Project
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindSceneLoader();
            BindProjectConfigLoader();
            BindAssetLoader();
            BindInput();
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
    }
}
