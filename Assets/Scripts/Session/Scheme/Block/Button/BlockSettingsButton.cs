using GlobalServices.ProjectLifetime;
using Session.Scheme.Windows;
using UnityEngine;

namespace Session.Scheme.Block.Button
{
    public class BlockSettingsButton : BaseBlockButton
    {
        public void ConstructManualy(WindowService windowService, BaseWindow windowToOpen, IBlock block)
        {
            _windowService = windowService;
            _windowToOpen = windowToOpen;
            _block = block;
        }

        private WindowService _windowService;
        private BaseWindow _windowToOpen;
        private IBlock _block;

        public override void Use()
        {
            _windowService.OpenWindow(_windowToOpen.WindowName, sender: _block);
        }
    }
}