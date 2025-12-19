using GlobalServices.DataSave;
using GlobalServices.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
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

            // Scne management service
            ConfigureSceneManagement(builder);

            // Data Save service
            ConfigureDataSaveService(builder);
        }

        #region Implementations
        private void ConfigureProjectConfigs(IContainerBuilder builder)
        {
            builder.RegisterInstance(_projectConfig.MainScenes);
        }

        private void ConfigureSceneManagement(IContainerBuilder builder)
        {
            builder.Register<SceneLoaderService>(Lifetime.Singleton).AsSelf();
        }

        private void ConfigureDataSaveService(IContainerBuilder builder)
        {
            builder.Register<DataSaveService>(Lifetime.Singleton).AsSelf();
        }
        #endregion
    }
}