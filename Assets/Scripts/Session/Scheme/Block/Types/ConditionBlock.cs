using Session.Scheme.Variables;
using System;
using UnityEngine;

namespace Session.Scheme.Block.Types
{
    public class ConditionBlock : IBlock, IDisposable
    {
        public ConditionBlock(SchemeBlockFacade facade, VariableService variableService)
        {
            _facade = facade;
            _variableService = variableService;
        }

        private readonly SchemeBlockFacade _facade;
        public SchemeBlockFacade Facade => _facade;
        private readonly VariableService _variableService;

        private SchemeVariableBase _operand1;
        private VariableService.ConditionOperatorType _conditionalOperatorType;
        private SchemeVariableBase _operand2;

        private IBlock _trueOutput;
        private IBlock _falseOutput;

        private IBlock _nextBlock;

        public IActionProvider Next { get => _nextBlock; set => _nextBlock = (IBlock)value; }
        public bool SingleInstance { get => _facade.SingleInstance; }

        public VariableService.ConditionOperatorType OperatorType => _conditionalOperatorType;
        public SchemeVariableBase Operand1 => _operand1;
        public SchemeVariableBase Operand2 => _operand2;


        public void ProvideAction()
        {
            bool comparsionValue = _variableService.UseComparsion(_operand1.variableName, _conditionalOperatorType, _operand2.variableName);
            Next = comparsionValue ? _trueOutput : _falseOutput;

            Next?.ProvideAction();
        }

        public void SetTrueOutput(IBlock output)
        {
            _trueOutput = output;
        }

        public void SetFalseOutput(IBlock output)
        {
            _falseOutput = output;
        }

        public void SetOperation(SchemeVariableBase operand1, VariableService.ConditionOperatorType operatorType, SchemeVariableBase operand2)
        {
            _operand1 = operand1;
            _conditionalOperatorType = operatorType;
            _operand2 = operand2;

            string operand1Label = _operand1 != null ? _operand1.variableName : "no variable";
            string operatorTypeLabel = _conditionalOperatorType.ToString();
            string operand2Label = _operand2 != null ? _operand2.variableName : "no variable";

            _facade.Label.SetText($"{operand1Label} {operatorTypeLabel} {operand2Label}");
        }

        public bool CheckForCorrectRelationships()
        {
            return Next != null && _nextBlock.CheckForCorrectRelationships();
        }

        public bool CheckForCorrectValues()
        {
            return _operand1 != null && _operand2 != null && (Next == null || _nextBlock.CheckForCorrectValues());
        }

        public void Dispose()
        {
            GameObject.Destroy(_facade.gameObject);
        }
    }
}