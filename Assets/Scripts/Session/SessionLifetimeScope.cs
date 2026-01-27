using GlobalServices.ProjectLifetime;
using Session.Scheme;
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
            // Service for scheme building
            builder.Register<SchemeBuilderService>(Lifetime.Scoped)
                .AsImplementedInterfaces()
                .AsSelf();

            // Service for variable building
            builder.Register<VariableService>(Lifetime.Scoped)
                .AsSelf();

            // Service for opening windows
            builder.RegisterEntryPoint<WindowService>(Lifetime.Scoped)
                .AsSelf();

            // Register windows
            //builder.RegisterComponent(_projectConfig.BlockConfigs.VariablesListWindowPrefab.GetComponent<VariableListUI>()).As<SettingsBaseWindowUI>();
            builder.RegisterFactory<string, Transform, BaseWindowUI>((IObjectResolver resolver) =>
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

                    return window.GetComponent<BaseWindowUI>();
                };
            }, Lifetime.Scoped);
        }
    }
}