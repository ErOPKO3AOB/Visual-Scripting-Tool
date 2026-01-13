using GlobalServices.ProjectLifetime;
using Session.Execution;
using Session.Scheme;
using Session.Scheme.Variables;
using UnityEngine;
using UnityEngine.InputSystem;
using User;
using User.Input;
using VContainer;
using VContainer.Unity;

namespace Session
{
    public class SessionLifetimeScope : LifetimeScope
    {
        [SerializeField] private ProjectConfig _projectConfig;

        protected override void Configure(IContainerBuilder builder)
        {
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
            builder.RegisterEntryPoint<DraggableObjectController>(Lifetime.Scoped).AsSelf();
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
            // Service for execution
            builder.Register<ExecutionService>(Lifetime.Scoped)
                .AsImplementedInterfaces();

            // Service for scheme building
            builder.Register<SchemeBuilderService>(Lifetime.Scoped)
                .AsImplementedInterfaces()
                .AsSelf();

            // Service for variable building
            builder.Register<VariableService>(Lifetime.Scoped)
                .AsSelf();

            foreach (var item in _projectConfig.BlockConfigs.BlockScreensPrefabsUI)
            {
                //builder.RegisterEntryPoint<SettingsBaseUI>();
            }
        }
    }
}