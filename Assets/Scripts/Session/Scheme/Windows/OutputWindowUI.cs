using GlobalServices.ProjectLifetime;
using Session.Scheme.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Session.Scheme.Windows
{
    public class OutputWindowUI : BaseWindowUI
    {
        [Header("UI")]
        [SerializeField] private Button _variableSpawnButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private TextMeshProUGUI _type;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _value;

        private SchemeVariableBase _variable;

        [Inject]
        public void Construct(WindowService windowService, BlockConfigs blockConfigs)
        {
            _windowService = windowService;
            _blockConfigs = blockConfigs;
        }

        private WindowService _windowService;
        private BlockConfigs _blockConfigs;
        private VariableListWindowUI _variableList;

        private void Start()
        {
            _closeButton.onClick.AddListener(() => { _windowService.CloseWindow(WindowName); });

            _variableSpawnButton.onClick.AddListener(() =>
            {
                _windowService.OpenWindow(_blockConfigs.WindowPrefabsUI[0].WindowName);
                _variableList = FindAnyObjectByType<VariableListWindowUI>();
                _variableList.OnVariableChoose += OnVariableChoose;
            });
        }

        private void OnDestroy()
        {
            _variableSpawnButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();
            _variableList.OnVariableChoose -= OnVariableChoose;
        }

        private void OnVariableChoose(SchemeVariableBase variable)
        {
            _variable = variable;

            _type.text = _variable.ValueType.ToString();
            _name.text = _variable.variableName;
            _value.text = _variable.GetValue().ToString();
        }
    }
}