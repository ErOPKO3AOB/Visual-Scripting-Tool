using GlobalServices.ProjectLifetime;
using Session.Scheme.Windows;
using UnityEngine;

namespace Session.Scheme.Block.Button
{
    public class BlockSettingsButton : BaseBlockButton
    {
        public void ConstructManualy(WindowFactory windowService, BaseWindow windowToOpen, IBlock block)
        {
            _windowService = windowService;
            _windowToOpen = windowToOpen;
            _block = block;
        }

        private WindowFactory _windowService;
        private BaseWindow _windowToOpen;
        private IBlock _block;

        public override void Use()
        {
            _windowService.OpenWindow(_windowToOpen.WindowName, sender: _block);
        }
    }
}