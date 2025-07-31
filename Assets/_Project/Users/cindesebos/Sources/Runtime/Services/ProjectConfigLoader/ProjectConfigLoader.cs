using Cysharp.Threading.Tasks;
using Sources.Runtime.Gameplay.Configs;
using UnityEngine;

namespace Sources.Runtime.Services.ProjectConfigLoader
{
    public class ProjectConfigLoader : IProjectConfigLoader
    {
        public ProjectConfig ProjectConfig { get; private set; }

        public async UniTask LoadProjectConfigAsync()
        {
            ProjectConfig = (ProjectConfig)await Resources.LoadAsync<ProjectConfig>("Global Project Config");
        }
    }
}