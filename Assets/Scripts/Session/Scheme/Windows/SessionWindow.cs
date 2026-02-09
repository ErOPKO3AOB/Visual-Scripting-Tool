using GlobalServices.ProjectLifetime;
using UnityEngine;
using VContainer;

namespace Session.Scheme.Windows
{
    public class SessionWindow : BaseWindow
    {
        [Inject]
        public void Construct(WindowFactory windowService, BlockConfigs blockConfigs)
        {
            _windowService = windowService;
            _blockConfigs = blockConfigs;
        }

        [SerializeField] private Transform _content;
        [SerializeField] private int[] _essentialWindowIndecies;

        private WindowFactory _windowService;
        private BlockConfigs _blockConfigs;

        private void Start()
        {
            foreach (int item in _essentialWindowIndecies)
            {
                _windowService.OpenWindow(_blockConfigs.WindowPrefabsUI[item].WindowName, _content);
            }
        }
    }
}