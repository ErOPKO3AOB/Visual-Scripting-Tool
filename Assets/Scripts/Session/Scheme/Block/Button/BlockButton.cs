using Session.Scheme.Windows;
using UnityEngine;
using VContainer;

namespace Session.Scheme.Block.Button
{
    public class BlockButton : MonoBehaviour
    {
        public void ConstructManualy(WindowService windowService, BaseWindow windowToOpen, IActionProvider block)
        {
            _windowService = windowService;
            _windowToOpen = windowToOpen;
        }

        private WindowService _windowService;
        private BaseWindow _windowToOpen;
        private IActionProvider _block;

        private void Start()
        {
            transform.localScale = Vector3.one;
            transform.localPosition = Vector3.zero;
        }

        public void Use()
        {
            _windowService.OpenWindow(_windowToOpen.WindowName, sender: _block);
        }
    }
}