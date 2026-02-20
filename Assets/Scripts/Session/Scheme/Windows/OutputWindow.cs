using Session.Scheme.Block.Types;
using Session.Scheme.Variables;
using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Session.Scheme.Windows
{
    public class OutputWindow : BaseWindow
    {
        [Inject]
        public void Construct(WindowFactory windowService)
        {
            _windowService = windowService;
        }

        [Header("UI")]
        [SerializeField] private Button _closeButton;
        [SerializeField] private VariablePickerItem _variablePicker;

        private OutputBlock _outputBlock;
        private WindowFactory _windowService;

        public override void SetSender(object sender)
        {
            try
            {
                _outputBlock = (OutputBlock)sender;
                RebuildUI();
            }

            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void Start()
        {
            _closeButton.onClick.AddListener(() => { _windowService.CloseWindow(this); });
            _variablePicker.OnVariableChanged += OnVariableChoose;
        }

        private void RebuildUI()
        {
            if (_outputBlock.SchemeVariable != null)
                _variablePicker.ChooseVariable(_outputBlock.SchemeVariable);
        }

        private void OnVariableChoose(SchemeVariableBase variable)
        {
            _outputBlock.SetOperation(variable);
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveAllListeners();
            _variablePicker.OnVariableChanged -= OnVariableChoose;
        }
    }
}