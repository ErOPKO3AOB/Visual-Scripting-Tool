using Session.Scheme.Block.Types;
using Session.Scheme.Variables;
using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Session.Scheme.Windows
{
    public class MethodWindow : BaseWindow
    {
        [Inject]
        public void Construct(WindowFactory windowService)
        {
            _windowService = windowService;
        }

        [Header("UI")]
        [SerializeField] private Button _closeButton;
        [SerializeField] private VariablePickerUI _varPicker1;
        [SerializeField] private OperationItem _operationItem;
        [SerializeField] private VariablePickerUI _varPicker2;

        private SchemeVariableBase _operand1;
        private VariableService.MethodOperatorType _operatorType;
        private SchemeVariableBase _operand2;

        private WindowFactory _windowService;
        private MethodBlock _methodBlock;

        public override void SetSender(object sender)
        {
            try
            {
                _methodBlock = (MethodBlock)sender;
                RebuildUI();
            }

            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void Start()
        {
            _closeButton.onClick.AddListener(() =>
            {
                _windowService.CloseWindow(WindowName);
                SendOperationToMethodBlock();
            });

            _varPicker1.OnVariableChoosed += OnOperand1Choosed;
            _operationItem.OnOperationTypeChoosed += OnOperationTypeChoosed;
            _varPicker2.OnVariableChoosed += OnOperand2Choosed;
        }

        private void RebuildUI()
        {
            //Debug.Log($"Current value is {(int)_methodBlock.OperatorType}");

            _operationItem.OperatorType = OperationItem.OperationType.Method;
            _operationItem.OperationDropDown.value = (int)_methodBlock.OperatorType;
            if (_methodBlock.Operand1 != null)
                _varPicker1.OnVariableChoose(_methodBlock.Operand1);
            if (_methodBlock.Operand2 != null)
                _varPicker2.OnVariableChoose(_methodBlock.Operand2);
        }

        private void OnOperand1Choosed(SchemeVariableBase variable)
        {
            _operand1 = variable;

            SendOperationToMethodBlock();
        }

        private void OnOperationTypeChoosed(object operatorType)
        {
            _operatorType = (VariableService.MethodOperatorType)operatorType;

            SendOperationToMethodBlock();
        }

        private void OnOperand2Choosed(SchemeVariableBase variable)
        {
            _operand2 = variable;

            SendOperationToMethodBlock();
        }

        private void SendOperationToMethodBlock()
        {
            _methodBlock.SetOperation(_operand1, _operatorType, _operand2);
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveAllListeners();

            _varPicker1.OnVariableChoosed -= OnOperand1Choosed;
            _operationItem.OnOperationTypeChoosed -= OnOperationTypeChoosed;
            _varPicker2.OnVariableChoosed -= OnOperand2Choosed;
        }
    }
}