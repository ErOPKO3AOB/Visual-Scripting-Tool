using Session.Execution;
using Session.Scheme;
using VContainer;
using VContainer.Unity;

namespace Session
{
    public class SessionLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ExecutionService>(Lifetime.Scoped)
                .AsImplementedInterfaces();

            // Registering builder service with interfaces and itself
            builder.Register<SchemeBuilderService>(Lifetime.Scoped)
                .AsImplementedInterfaces()
                .AsSelf();
        }
    }
}