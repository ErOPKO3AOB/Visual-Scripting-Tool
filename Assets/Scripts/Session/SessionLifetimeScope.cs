using GlobalServices.ProjectLifetime;
using Session.Execution;
using Session.Scheme;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;
using User;
using User.Input;

namespace Session
{
    public class SessionLifetimeScope : LifetimeScope
    {
        [SerializeField] private ProjectConfig _projectConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            // Регистрируем сервисы сессии
            builder.Register<ExecutionService>(Lifetime.Scoped)
                .AsImplementedInterfaces();

            builder.Register<SchemeBuilderService>(Lifetime.Scoped)
                .AsImplementedInterfaces()
                .AsSelf();

            // Player Input Service
            ConfigurePlayerInput(builder);

            // Регистрируем камеру сессии
            RegisterCameraSystem(builder);
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
        }

        private void RegisterCameraSystem(IContainerBuilder builder)
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
    }
}