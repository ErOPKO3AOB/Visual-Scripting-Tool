using GlobalServices.ProjectLifetime;
using Session.Scheme;
using Session.Scheme.Block;
using Session.Scheme.Variables;
using Session.Scheme.Windows;
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
            builder.RegisterEntryPoint<WorldUIControllerService>(Lifetime.Scoped).AsSelf();
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
            builder.Register<SchemeBuilderService>(Lifetime.Scoped)
                .AsImplementedInterfaces()
                .AsSelf();

            // Factory for spawning blocks
            builder.RegisterFactory<string, Transform, SchemeBlockFacade>((IObjectResolver resolver) =>
            {
                return (string blockName, Transform spawnPosition) =>
                {
                    GameObject block = null;

                    for (int i = 0; i < _projectConfig.BlockConfigs.BlockFacades.Length; i++)
                    {
                        if (blockName == _projectConfig.BlockConfigs.BlockFacades[i].BlockName)
                        {
                            Vector3 spawnPos = spawnPosition ? spawnPosition.position : Vector3.zero;
                            block = resolver.Instantiate(_projectConfig.BlockConfigs.BlockFacades[i].gameObject, spawnPos, Quaternion.identity);
                            break;
                        }
                    }

                    return block.GetComponent<SchemeBlockFacade>();
                };
            }, Lifetime.Scoped);

            // Service for variable building
            builder.Register<VariableService>(Lifetime.Scoped)
                .AsSelf();

            // Service for opening windows
            builder.RegisterEntryPoint<WindowService>(Lifetime.Scoped)
                .AsSelf();

            // Register windows
            builder.RegisterFactory<string, Transform, BaseWindow>((IObjectResolver resolver) =>
            {
                return (string windowName, Transform spawnParent) =>
                {
                    GameObject window = null;

                    for (int i = 0; i < _projectConfig.BlockConfigs.WindowPrefabsUI.Length; i++)
                    {
                        if (windowName == _projectConfig.BlockConfigs.WindowPrefabsUI[i].WindowName)
                        {
                            window = resolver.Instantiate(_projectConfig.BlockConfigs.WindowPrefabsUI[i].gameObject, spawnParent);
                            break;
                        }
                    }

                    return window.GetComponent<BaseWindow>();
                };
            }, Lifetime.Scoped);
        }
    }
}