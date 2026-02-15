using GlobalServices.ProjectLifetime;
using Session.Scheme;
using Session.Scheme.Windows;
using VContainer.Unity;

namespace Session
{
    public class SessionInitializer : IInitializable
    {
        public SessionInitializer(WindowFactory windowService, BlockConfigs blockConfigs, SchemeBlockFactory schemeBlockFactory)
        {
            _windowService = windowService;
            _blockConfigs = blockConfigs;
            _schemeBlockFactory = schemeBlockFactory;
        }

        private readonly WindowFactory _windowService;
        private readonly BlockConfigs _blockConfigs;
        private readonly SchemeBlockFactory _schemeBlockFactory;

        public void Initialize()
        {
            _windowService.OpenWindow(_blockConfigs.WindowPrefabsUI.Find(w => w.GetType() == typeof(SessionWindow)));

            _schemeBlockFactory.SpawnBlock("START_BLOCK");
            _schemeBlockFactory.SpawnBlock("END_BLOCK");
        }
    }
}