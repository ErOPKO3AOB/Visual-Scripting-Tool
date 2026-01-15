using GlobalServices.ProjectLifetime;
using System;
using UnityEngine;
using VContainer.Unity;

namespace Session.Scheme.Windows
{
    public class WindowService : IInitializable
    {
        public WindowService(Func<SettingsBaseWindowUI> windowFactory, BlockConfigs blockConfigs)
        {
            _windowFactory = windowFactory;
            _blockConfigs = blockConfigs;

        }

        private readonly Func<SettingsBaseWindowUI> _windowFactory;
        private readonly BlockConfigs _blockConfigs;

        public void OpenWindow(string windowName)
        {
            Debug.Log("OPENED WINDOW");

            _windowFactory.Invoke();
        }

        public void CloseWindow(string windowName)
        {
            //GameObject.Destroy(SearchWindow(windowName));
        }

        public void Initialize()
        {
            OpenWindow(null);
        }

        //private GameObject SearchWindow(string windowName)
        //{

        //}
    }
}