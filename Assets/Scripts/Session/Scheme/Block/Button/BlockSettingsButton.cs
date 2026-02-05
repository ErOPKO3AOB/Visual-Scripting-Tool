using GlobalServices.ProjectLifetime;
using Session.Scheme.Windows;
using UnityEngine;

namespace Session.Scheme.Block.Button
{
    public class BlockSettingsButton : BaseBlockButton
    {
        public void ConstructManualy(WindowService windowService, BaseWindow windowToOpen, IActionProvider block, BlockConfigs blockConfigs)
        {
            _windowService = windowService;
            _windowToOpen = windowToOpen;
            _block = block;
            _blockConfigs = blockConfigs;
        }

        private WindowService _windowService;
        private BaseWindow _windowToOpen;
        private IActionProvider _block;
        private BlockConfigs _blockConfigs;

        public override void Use()
        {
            _windowService.OpenWindow(_windowToOpen.WindowName, sender: _block);
        }
    }
}