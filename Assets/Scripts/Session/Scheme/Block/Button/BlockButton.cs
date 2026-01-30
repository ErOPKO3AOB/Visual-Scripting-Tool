using Session.Scheme.Windows;
using UnityEngine;
using VContainer;

namespace Session.Scheme.Block.Button
{
    public class BlockButton : MonoBehaviour
    {

        public void ConstructManualy(WindowService windowService, BaseWindowUI windowToOpen)
        {
            _windowService = windowService;
            _windowToOpen = windowToOpen;
        }

        private WindowService _windowService;
        private BaseWindowUI _windowToOpen;

        public void Use()
        {
            _windowService.OpenWindow(_windowToOpen.WindowName);
        }
    }
}