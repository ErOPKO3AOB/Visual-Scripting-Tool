using GlobalServices.ProjectLifetime;
using Session.Scheme;
using Session.Scheme.Block;
using Session.Scheme.Variables;
using Session.Scheme.Windows;
using UnityEngine;
using UnityEngine.InputSystem;
using User;
using VContainer;
using VContainer.Unity;

namespace Session
{
    public class SessionLifetimeScope : LifetimeScope
    {
        [SerializeField] private ProjectConfig _projectConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<SessionInitializer>(Lifetime.Singleton);

            ConfigurePlayerInput(builder);
            ConfigureCameraSystem(builder);
            ConfigureSchemeEssentials(builder);
        }

        private void ConfigurePlayerInput(IContainerBuilder builder)
        {
            builder.RegisterComponentInNewPrefab(
                _projectConfig
                .InputSettings
                .PlayerInputPrefab
                .GetComponent<PlayerInput>(),
                Lifetime.Scoped)
                .AsSelf();
            builder.RegisterEntryPoint<InputService>(
                Lifetime
                .Scoped)
                .AsSelf();

            // Service for object dragging
            builder.RegisterEntryPoint<WorldUIControllerService>(Lifetime.Scoped)
                .AsSelf();
        }

        private void ConfigureCameraSystem(IContainerBuilder builder)
        {
            builder.RegisterComponentInNewPrefab(
                _projectConfig
                .CameraSettings
                .CamPrefab
                .GetComponent<CameraControllerFacade>(),
                Lifetime.Scoped)
                .AsSelf();
            builder.RegisterEntryPoint<CameraController>(
                Lifetime.Scoped)
                .AsSelf();
        }

        private void ConfigureSchemeEssentials(IContainerBuilder builder)
        {
            // Service for scheme building
            builder.Register<SchemeBlockFactory>(Lifetime.Scoped)
                .AsImplementedInterfaces()
                .AsSelf();

            // Service for variable building
            builder.Register<VariableService>(Lifetime.Scoped)
                .AsSelf();

            // Service for opening windows
            builder.RegisterEntryPoint<WindowFactory>(Lifetime.Scoped)
                .AsSelf();
        }
    }
}