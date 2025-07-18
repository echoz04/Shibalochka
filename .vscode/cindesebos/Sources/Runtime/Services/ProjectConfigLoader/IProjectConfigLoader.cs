using System;
using Cysharp.Threading.Tasks;
using Sources.Runtime.Gameplay.Configs;

namespace Sources.Runtime.Services.ProjectConfigLoader
{
    public interface IProjectConfigLoader
    {
        ProjectConfig ProjectConfig { get; }

        UniTask LoadProjectConfigAsync();
    }
}