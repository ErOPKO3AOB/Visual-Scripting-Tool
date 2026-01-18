using GlobalServices.ProjectLifetime;
using System;
using UnityEngine;
using VContainer.Unity;
using System.Collections.Generic;

namespace Session.Scheme.Windows
{
    public class WindowService : IInitializable
    {
        public WindowService(Func<string, Transform, BaseWindowUI> windowFactory, BlockConfigs blockConfigs)
        {
            _windowFactory = windowFactory;
            _blockConfigs = blockConfigs;
        }

        private readonly Func<string, Transform, BaseWindowUI> _windowFactory;
        private readonly BlockConfigs _blockConfigs;

        private List<BaseWindowUI> _activeWindows = new List<BaseWindowUI>();

        public BaseWindowUI OpenWindow(string windowName, Transform spawnParent = null)
        {
            //Debug.Log($"OPENED WINDOW {windowName}");

            BaseWindowUI window = _windowFactory.Invoke(windowName, spawnParent);
            _activeWindows.Add(window);
            return window;
        }

        public void CloseWindow(string windowName)
        {
            GameObject window = null;

            // Searching for last opened
            for (int i = _activeWindows.Count - 1; i >= 0; i--)
            {
                if (_activeWindows[i].WindowName == windowName)
                {
                    window = _activeWindows[i].gameObject;
                    _activeWindows.RemoveAt(i);
                    GameObject.Destroy(window);
                    break;
                }
            }

        }

        public void Initialize()
        {
            OpenWindow(_blockConfigs.WindowPrefabsUI[2].WindowName); 
        }
    }
}