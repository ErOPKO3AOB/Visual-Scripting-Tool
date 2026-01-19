using Session.Scheme.Block.Types;
using Session.Scheme.Variables;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Session.Scheme.Windows
{
    public class OutputWindowUI : BaseWindowUI
    {
        [Header("UI")]
        [SerializeField] private Button _closeButton;
        [SerializeField] private VariablePickerUI _variablePicker;

        private OutputBlock _block;

        [Inject]
        public void Construct(WindowService windowService)
        {
            _windowService = windowService;
        }

        private WindowService _windowService;

        private void Start()
        {
            _closeButton.onClick.AddListener(() => { _windowService.CloseWindow(WindowName); });
            _variablePicker.VariableList.OnVariableChoose += OnVariableChoose;
        }

        private void OnVariableChoose(SchemeVariableBase variable)
        {
            _block.SchemeVariables.Clear();
            for (int i = 0; i < _variablePicker.VariableItems.Count; i++)
            {
                _block.SchemeVariables.Add(_variablePicker.VariableItems[i].SchemeVariable);
            }
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveAllListeners();
        }
    }
}