using Session.Scheme.Block.Types;
using Session.Scheme.Variables;
using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Session.Scheme.Windows
{
    public class ConditionWindow : BaseWindow
    {
        [Inject]
        public void Construct(WindowFactory windowFactory)
        {
            _windowFactory= windowFactory;
        }

        private ConditionBlock _conditionBlock;
        private WindowFactory _windowFactory;

        [Header("UI")]
        [SerializeField] private Button _closeButton;
        [SerializeField] private VariablePickerUI _varPicker1;
        [SerializeField] private OperationItem _operationItem;
        [SerializeField] private VariablePickerUI _varPicker2;

        private SchemeVariableBase _operand1;
        private VariableService.ConditionOperatorType _operatorType;
        private SchemeVariableBase _operand2;


        public override void SetSender(object sender)
        {
            try
            {
                _conditionBlock = (ConditionBlock)sender;
            }

            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void Start()
        {
            _operationItem.OperatorType = OperationItem.OperationType.Condition;
         
            _closeButton.onClick.AddListener(() =>
            {
                _windowFactory.CloseWindow(WindowName);
                SendOperationToConditionBlock();
            });

            _varPicker1.OnVariableChoosed += OnOperand1Choosed;
            _operationItem.OnOperationTypeChoosed += OnOperationTypeChoosed;
            _varPicker2.OnVariableChoosed += OnOperand2Choosed;
        }

        private void OnOperand1Choosed(SchemeVariableBase variable)
        {
            _operand1 = variable;

            SendOperationToConditionBlock();
        }

        private void OnOperationTypeChoosed(object operatorType)
        {
            _operatorType = (VariableService.ConditionOperatorType)operatorType;

            SendOperationToConditionBlock();
        }

        private void OnOperand2Choosed(SchemeVariableBase variable)
        {
            _operand2 = variable;

            SendOperationToConditionBlock();
        }

        private void SendOperationToConditionBlock()
        {
            if (_operand1 != null && _operand2 != null)
                _conditionBlock.SetOperation(_operand1, _operatorType, _operand2);
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