using GlobalServices.ProjectLifetime;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Session.Scheme.Windows
{
    public class InputWindowUI : BaseWindowUI
    {
        [Header("UI")]
        [SerializeField] private Button _variableSpawnButton;
        [SerializeField] private Button _closeButton;

        [Inject]
        public void Construct(WindowService windowService, BlockConfigs blockConfigs)
        {
            _windowService = windowService;
            _blockConfigs = blockConfigs;
        }

        private WindowService _windowService;
        private BlockConfigs _blockConfigs;

        private void Start()
        {
            _closeButton.onClick.AddListener(() => { _windowService.CloseWindow(WindowName); });

            _variableSpawnButton.onClick.AddListener(() =>
            {
                _windowService.OpenWindow(_blockConfigs.WindowPrefabsUI[0].WindowName);
            });
        }

        private void OnDestroy()
        {
            _variableSpawnButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();
        }
    }
}