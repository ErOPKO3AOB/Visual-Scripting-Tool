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
            ConfigureProjectConfigs(builder);

            ConfigureSceneManagement(builder);
        }

        private void ConfigureSceneManagement(IContainerBuilder builder)
        {
            builder.Register<SceneLoaderService>(Lifetime.Singleton).AsSelf();
        }

        private void ConfigureProjectConfigs(IContainerBuilder builder)
        {
            builder.RegisterInstance(_projectConfig.MainScenes);
        }
    }
}