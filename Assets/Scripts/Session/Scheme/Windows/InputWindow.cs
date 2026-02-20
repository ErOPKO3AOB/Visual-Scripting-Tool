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
        [SerializeField] private VariablePickerItem _variablePicker;

        private InputBlock _inputBlock;

        private WindowFactory _windowService;

        public override void SetSender(object sender)
        {
            try
            {
                _inputBlock = (InputBlock)sender;
                RebuildUI();
            }

            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void Start()
        {
            _variablePicker.OnVariableChanged += OnVariableChoose;
            _closeButton.onClick.AddListener(() => { _windowService.CloseWindow(this); });
        }

        private void RebuildUI()
        {
            if (_inputBlock.SchemeVariable != null)
                _variablePicker.ChooseVariable(_inputBlock.SchemeVariable);
        }

        private void OnVariableChoose(SchemeVariableBase variable)
        {
            _inputBlock.SetOperation(variable);
        }

        private void OnDestroy()
        {
            _variablePicker.OnVariableChanged -= OnVariableChoose;
            _closeButton.onClick.RemoveAllListeners();
        }
    }
}