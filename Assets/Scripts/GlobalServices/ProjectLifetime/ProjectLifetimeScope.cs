using GlobalServices.SceneManagement;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GlobalServices.ProjectLifetime
{
    public class ProjectLifetimeScope : LifetimeScope
    {
        [Header("Configs")]
        [SerializeField] private ProjectConfig _projectConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            // Project parameters config
            ConfigureProjectConfigs(builder);

            // Scene management service
            ConfigureSceneManagement(builder);
        }

        #region Implementations
        private void ConfigureProjectConfigs(IContainerBuilder builder)
        {
            // Регистрируем весь конфиг проекта
            builder.RegisterInstance(_projectConfig);

            builder.RegisterInstance(_projectConfig.MainScenes);

            builder.RegisterInstance(_projectConfig.CameraSettings);

            builder.RegisterInstance(_projectConfig.InputSettings);

            builder.RegisterInstance(_projectConfig.BlockConfigs);
        }

        private void ConfigureSceneManagement(IContainerBuilder builder)
        {
            builder.Register<SceneLoaderService>(Lifetime.Singleton).AsSelf();
        }
        #endregion
    }
}