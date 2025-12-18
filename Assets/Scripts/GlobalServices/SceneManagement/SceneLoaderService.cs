using UnityEngine.SceneManagement;

namespace GlobalServices.SceneManagement
{
    public class SceneLoaderService
    {
        private string _sceneName;

        public void SetScene(string name)
        {
            _sceneName = name;
        }

        public void LoadScene()
        {
            SceneManager.LoadScene(_sceneName);
        }
    }
}