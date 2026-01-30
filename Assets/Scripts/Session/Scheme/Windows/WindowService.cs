using GlobalServices.ProjectLifetime;
using System;
using UnityEngine;
using VContainer.Unity;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Session.Scheme.Windows
{
    public class WindowService
    {
        public WindowService(Func<string, Transform, BaseWindowUI> windowFactory, BlockConfigs blockConfigs)
        {
            _windowFactory = windowFactory;
            _blockConfigs = blockConfigs;
        }

        private readonly Func<string, Transform, BaseWindowUI> _windowFactory;
        private readonly BlockConfigs _blockConfigs;

        private List<BaseWindowUI> _activeWindows = new List<BaseWindowUI>();

        public UnityAction<BaseWindowUI> OnOpenWindow;
        public UnityAction<BaseWindowUI> OnCloseWindow;

        public BaseWindowUI OpenWindow(string windowName, Transform spawnParent = null)
        {
            BaseWindowUI window = _windowFactory.Invoke(windowName, spawnParent);
            _activeWindows.Add(window);
            OnOpenWindow?.Invoke(window);
            return window;
        }

        public void CloseWindow(string windowName)
        {
            BaseWindowUI window = null;

            // Searching for last opened
            for (int i = _activeWindows.Count - 1; i >= 0; i--)
            {
                if (_activeWindows[i].WindowName == windowName)
                {
                    window = _activeWindows[i];
                    OnCloseWindow?.Invoke(window);
                    _activeWindows.RemoveAt(i);
                    GameObject.Destroy(window.gameObject);
                    break;
                }
            }

        }
    }
}