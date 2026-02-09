using GlobalServices.ProjectLifetime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VContainer;
using VContainer.Unity;

namespace Session.Scheme.Windows
{
    public class WindowFactory
    {
        public WindowFactory(IObjectResolver objectResolver, BlockConfigs blockConfigs)
        {
            _objectResolver = objectResolver;
            _blockConfigs = blockConfigs;
        }

        private readonly IObjectResolver _objectResolver;
        private readonly BlockConfigs _blockConfigs;

        private List<BaseWindow> _activeWindows = new();
        public List<BaseWindow> ActiveWindows { get { return _activeWindows; } }

        public UnityAction<BaseWindow> OnOpenWindow;
        public UnityAction<BaseWindow> OnCloseWindow;

        public BaseWindow OpenWindow(string windowName, Transform spawnParent = null, object sender = null)
        {
            BaseWindow window = _objectResolver.Instantiate(
                // Finding window by name and spawning
                _blockConfigs.WindowPrefabsUI.Find(w => w.WindowName == windowName)
                .gameObject,
                spawnParent).
                // Getting window component
                GetComponent<BaseWindow>();

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