using Session.Scheme.Block.Types;
using Session.Scheme.Variables;
using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Session.Scheme.Windows
{
    public class InputWindow : BaseWindow
    {
        [Inject]
        public void Construct(WindowFactory windowService)
        {
            _windowService = windowService;
        }

        [Header("UI")]
        [SerializeField] private Button _closeButton;
        [SerializeField] private VariablePickerUI _variablePicker;

        private InputBlock _inputBlock;

        private WindowFactory _windowService;

        public override void SetSender(object sender)
        {
            try
            {
                _inputBlock = (InputBlock)sender;
            }

            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void Start()
        {
            _closeButton.onClick.AddListener(() => { _windowService.CloseWindow(WindowName); });
            _variablePicker.PickType = VariablePickerUI.VariablePickType.Single;
        }

        private void OnVariableChoose(SchemeVariableBase variable)
        {
            _inputBlock.VariableName = variable.variableName;
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveAllListeners();
        }
    }
}