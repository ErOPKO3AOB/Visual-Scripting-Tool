using Session.Scheme.Block.Types;
using Session.Scheme.Variables;
using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Session.Scheme.Windows
{
    public class OutputWindowUI : BaseWindow
    {
        [Inject]
        public void Construct(WindowService windowService)
        {
            _windowService = windowService;
        }

        [Header("UI")]
        [SerializeField] private Button _closeButton;
        [SerializeField] private VariablePickerUI _variablePicker;

        private OutputBlock _outputBlock;
        private WindowService _windowService;

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
            _closeButton.onClick.AddListener(() => { _windowService.CloseWindow(WindowName); });
        }

        private void OnVariableChoose(SchemeVariableBase variable)
        {
            _outputBlock.SchemeVariables.Clear();
            for (int i = 0; i < _variablePicker.VariableItems.Count; i++)
            {
                _outputBlock.SchemeVariables.Add(_variablePicker.VariableItems[i].SchemeVariable);
            }
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveAllListeners();
        }
    }
}