using GlobalServices.ProjectLifetime;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Session.Scheme.Windows
{
    public class InputWindow : BaseWindowUI
    {
        [Header("UI")]
        [SerializeField] private Button _variableSpawnButton;
        //[SerializeField] private Button _closeButton;

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
            //_closeButton 

            _variableSpawnButton.onClick.AddListener(() =>
            {
                if (/*_blockConfigs == null || */_windowService == null) Debug.Log("NUULL");
                _windowService.OpenWindow(_blockConfigs.WindowPrefabsUI[0].WindowName);
            });
        }

        private void OnDestroy()
        {
            _variableSpawnButton.onClick.RemoveAllListeners();
        }

        public override void OnEndEdit()
        {
            throw new System.NotImplementedException();
        }
    }
}