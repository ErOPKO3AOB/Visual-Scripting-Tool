using Session.Scheme.Block.Types;
using Session.Scheme.Variables;
using System;
using UnityEngine;

namespace Session.Scheme.Windows
{
    public class MethodWindow : BaseWindow
    {
        [Header("UI")]
        [SerializeField] private VariablePickerUI _varPicker1;
        [SerializeField] private OperationItem _operationItem;
        [SerializeField] private VariablePickerUI _varPicker2;

        private MethodBlock _methodBlock;

        private SchemeVariableBase _operand1;
        private VariableService.OperatorType _operatorType;
        private SchemeVariableBase _operand2;

        protected override void CastSender()
        {
            try
            {
                _methodBlock = (MethodBlock)sender;
            }

            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void Start()
        {
            _varPicker1.OnVariableChoosed += OnOperand1Choosed;
            _operationItem.OnOperationTypeChoosed += OnOperationTypeChoosed;
            _varPicker2.OnVariableChoosed += OnOperand2Choosed;
        }

        private void OnOperand1Choosed(SchemeVariableBase variable)
        {
            _operand1 = variable;

            SendOperationToMethodBlock();
        }

        private void OnOperationTypeChoosed(VariableService.OperatorType operatorType)
        {
            _operatorType = operatorType;

            SendOperationToMethodBlock();
        }

        private void OnOperand2Choosed(SchemeVariableBase variable)
        {
            _operand2 = variable;

            SendOperationToMethodBlock();
        }

        private void SendOperationToMethodBlock()
        {
            if (_operand1 != null && _operand2 != null)
                _methodBlock.SetOperation(_operand1, _operatorType, _operand2);
        }

        private void OnDestroy()
        {
            _varPicker1.OnVariableChoosed -= OnOperand1Choosed;
            _operationItem.OnOperationTypeChoosed -= OnOperationTypeChoosed;
            _varPicker2.OnVariableChoosed -= OnOperand2Choosed;
        }
    }
}