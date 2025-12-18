using GlobalServices.SceneManagement;
using VContainer.Unity;

namespace GlobalServices.ProjectLifetime
{
    public class BootstrapEntryPoint : IPostStartable
    {
        public BootstrapEntryPoint(SceneLoaderService sceneLoader, MainScenesConfig mainScenes)
        {
            _sceneLoader = sceneLoader;
            _mainScenes = mainScenes;
        }

        private readonly SceneLoaderService _sceneLoader;
        private readonly MainScenesConfig _mainScenes;

        public void PostStart()
        {
            _sceneLoader.SetScene(_mainScenes.MenuScene);
            _sceneLoader.LoadScene();
        }
    }
}