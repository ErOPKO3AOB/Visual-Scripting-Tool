using Session.Scheme.Block.Types;
using Session.Scheme.Variables;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Session.Scheme.Windows
{
    public class InputWindowUI : BaseWindowUI
    {
        [Header("UI")]
        [SerializeField] private Button _closeButton;
        [SerializeField] private VariablePickerUI _variablePicker;

        public InputBlock Block { get; set; }

        [Inject]
        public void Construct(WindowService windowService)
        {
            _windowService = windowService;
        }

        private WindowService _windowService;

        private void Start()
        {
            _closeButton.onClick.AddListener(() => { _windowService.CloseWindow(WindowName); });
            _variablePicker.PickType = VariablePickerUI.VariablePickType.Single;
            _variablePicker.VariableList.OnVariableChoose += OnVariableChoose;
        }

        private void OnVariableChoose(SchemeVariableBase variable)
        {
            Block.VariableName = variable.variableName;
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveAllListeners();
        }
    }
}