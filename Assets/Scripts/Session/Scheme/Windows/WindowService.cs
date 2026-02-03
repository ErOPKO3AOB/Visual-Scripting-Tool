using GlobalServices.ProjectLifetime;
using System;
using UnityEngine;
using VContainer.Unity;
using System.Collections.Generic;
using UnityEngine.Events;
using Session.Scheme.Block;

namespace Session.Scheme.Windows
{
    public class WindowService
    {
        public WindowService(Func<string, Transform, BaseWindow> windowFactory, BlockConfigs blockConfigs)
        {
            _windowFactory = windowFactory;
            _blockConfigs = blockConfigs;
        }

        private readonly Func<string, Transform, BaseWindow> _windowFactory;
        private readonly BlockConfigs _blockConfigs;

        private List<BaseWindow> _activeWindows = new();
        public List<BaseWindow> ActiveWindows { get { return _activeWindows; } }

        public UnityAction<BaseWindow> OnOpenWindow;
        public UnityAction<BaseWindow> OnCloseWindow;

        public BaseWindow OpenWindow(string windowName, Transform spawnParent = null, object sender = null)
        {
            BaseWindow window = _windowFactory.Invoke(windowName, spawnParent);
            window.SetSender(sender);
            _activeWindows.Add(window);
            OnOpenWindow?.Invoke(window);
            return window;
        }

        public void CloseWindow(string windowName)
        {
            BaseWindow window = null;

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