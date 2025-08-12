using VContainer;
using VContainer.Unity;

namespace Sources
{
    public class MenuLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<MenuUI>().AsSelf().AsImplementedInterfaces();
        }
    }
}
