using VContainer;
using VContainer.Unity;

public class ProjectileLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<GameManagerModel>(Lifetime.Singleton);
    }
}
