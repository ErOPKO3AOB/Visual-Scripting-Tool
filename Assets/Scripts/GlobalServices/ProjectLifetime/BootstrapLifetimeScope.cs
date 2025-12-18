using GlobalServices.ProjectLifetime;
using VContainer;
using VContainer.Unity;

public class BootstrapLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<BootstrapEntryPoint>(Lifetime.Scoped);
    }
}
