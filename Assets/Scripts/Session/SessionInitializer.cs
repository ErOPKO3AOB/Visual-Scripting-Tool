using GlobalServices.ProjectLifetime;
using Session.Scheme.Windows;
using VContainer.Unity;

namespace Session
{
    public class SessionInitializer : IInitializable
    {
        public SessionInitializer(WindowFactory windowService, BlockConfigs blockConfigs)
        {
            _windowService = windowService;
            _blockConfigs = blockConfigs;
        }

        private readonly WindowFactory _windowService;
        private readonly BlockConfigs _blockConfigs;

        public void Initialize()
        {
            _windowService.OpenWindow(_blockConfigs.WindowPrefabsUI[0].WindowName);
        }
    }
}