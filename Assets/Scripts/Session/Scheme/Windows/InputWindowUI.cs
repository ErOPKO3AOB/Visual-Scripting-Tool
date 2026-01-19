using GlobalServices.ProjectLifetime;
using Session.Scheme.Variables;
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
        [SerializeField] private VariableItemUI _variableItem;

        private VariableListWindowUI _variableListWindow;

        [Inject]
        public void Construct(WindowService windowService, BlockConfigs blockConfigs)
        {
            _windowService = windowService;
            if (windowService == null) Debug.Log("WINDOW SERVICE IS NULL");
            _blockConfigs = blockConfigs;
            _windowService.OnCloseWindow += OnCloseWindow;
        }

        private WindowService _windowService;
        private BlockConfigs _blockConfigs;

        private void Start()
        {
            _closeButton.onClick.AddListener(() => { _windowService.CloseWindow(WindowName); });

            _variableSpawnButton.onClick.AddListener(() =>
            {
                _variableListWindow = (VariableListWindowUI)_windowService.OpenWindow(_blockConfigs.WindowPrefabsUI[0].WindowName);
                _variableListWindow.OnVariableChoose += OnVariableChoose;
                _variableListWindow.OnVariableChoose += OnVariableDelete;
            });
        }

        private void OnCloseWindow(BaseWindowUI window)
        {
            if (window.GetType() == typeof(VariableListWindowUI))
            {
                VariableListWindowUI listWindow = (VariableListWindowUI)window;

                listWindow.OnVariableChoose -= OnVariableChoose;
                listWindow.OnVariableChoose -= OnVariableDelete;
            }
        }

        private void OnVariableChoose(SchemeVariableBase schemeVariable)
        {
            _variableItem.gameObject.SetActive(true);
            _variableItem.RebuildUI(schemeVariable);
        }

        private void OnVariableDelete(SchemeVariableBase schemeVariable)
        {
            _variableItem.gameObject.SetActive(false);
            _variableItem.RebuildUI(0, "", null);
        }

        private void OnDestroy()
        {
            _variableSpawnButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();

            _windowService.OnCloseWindow -= OnCloseWindow;
        }
    }
}