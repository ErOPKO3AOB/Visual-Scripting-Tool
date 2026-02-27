using Extensions;
using Session.Scheme.Variables;
using System;
using UnityEngine;

namespace Session.Scheme.Block.Types
{
    public class ActionBlock : IBlock, IDisposable
    {
        public ActionBlock(SchemeBlockFacade facade, VariableService variableService)
        {
            _facade = facade;
            _variableService = variableService;
        }

        private readonly SchemeBlockFacade _facade;
        private readonly VariableService _variableService;

        private int _currentOutputIndex;
        public int CurrentOutputIndex { get { return _currentOutputIndex; } set { _currentOutputIndex = 0; } }

        private IBlock _nextBlock;

        private VariableService.ActionOperatorType _operatorType;
        private SchemeVariableBase _operand1 = null;
        private SchemeVariableBase _operand2 = null;

        public IBlock.BlockType ConcreteType { get => IBlock.BlockType.Action; }
        public VariableService.ActionOperatorType OperatorType => _operatorType;
        public SchemeVariableBase Operand1 => _operand1;
        public SchemeVariableBase Operand2 => _operand2;

        public SchemeBlockFacade Facade => _facade;
        public IActionProvider Next { get => _nextBlock; set => _nextBlock = (IBlock)value; }
        public bool SingleInstance => _facade.SingleInstance;

        public void ProvideAction()
        {
            _variableService.UseOperation(_operand1.variableName, _operatorType, _operand2.variableName);
            Next?.ProvideAction();
        }

        public void SetOperation(SchemeVariableBase operand1, VariableService.ActionOperatorType operatorType, SchemeVariableBase operand2)
        {
            _operand1 = operand1;
            _operatorType = operatorType;
            _operand2 = operand2;

            string displayName = operand1 != null && operand2 != null ?
                $"{_operand1.variableName} {TypeExtensions.GetFriendlyActionOperatorTypeName(_operatorType)} {_operand2.variableName}"
                : "Значения не установлены!";

            _facade.Label.SetText(displayName);
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