using UnityEngine;
using Zenject;

namespace Sources.Project
{
    public class ProjectInstaller : MonoInstaller<ProjectInstaller>
    {
        public override void InstallBindings()
        {
            BindInput();
        }

        private void BindInput()
        {
            Container.BindInterfacesAndSelfTo<CharacterInput>()
                .AsSingle();
        }
    }
}
