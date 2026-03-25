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
        public void Construct(WindowFactory windowFactory)
        {
            _windowFactory = windowFactory;
        }

        private WindowFactory _windowFactory;

        [Header("UI")]
        [SerializeField] private Button _closeButton;
        [SerializeField] private VariablePickerItem _variablePicker;

        private OutputBlock _outputBlock;

        public override void SetSender(object sender)
        {
            try
            {
                _outputBlock = (OutputBlock)sender;
            }

            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void Start()
        {
            _closeButton.onClick.AddListener(() => { _windowFactory.CloseWindow(this); });
            _variablePicker.OnVariableChanged += OnVariableChoose;

            RebuildUI();
        }

        private void RebuildUI()
        {
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