using Extensions;
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

        private VariableService.ConditionOperatorType _conditionalOperatorType;
        private SchemeVariableBase _operand1 = null;
        private SchemeVariableBase _operand2 = null;

        private IBlock _trueOutput;
        private IBlock _falseOutput;

        private int _currentOutputIndex;

        public IBlock.BlockType ConcreteType { get => IBlock.BlockType.Condition; }

        public int CurrentOutputIndex
        {
            get
            {
                if (_currentOutputIndex < 0) _currentOutputIndex = 0;
                else if (_currentOutputIndex > Facade.BlockOutputButtons.Length - 1) _currentOutputIndex = Facade.BlockOutputButtons.Length - 1;

                return _currentOutputIndex;
            }

            set
            {
                if (value < 0)
                    _currentOutputIndex = 0;
                else if (value > Facade.BlockOutputButtons.Length - 1)
                    _currentOutputIndex = Facade.BlockOutputButtons.Length - 1;
                else
                    _currentOutputIndex = value;
            }
        }

        public IActionProvider Next
        {
            get
            {
                return _currentOutputIndex == 0 ? _falseOutput : _trueOutput;
            }

            set
            {
                if (_currentOutputIndex == 0)
                {
                    _falseOutput = (IBlock)value;
                }

                else if (_currentOutputIndex == 1)
                {
                    _trueOutput = (IBlock)value;
                }
            }
        }

        public bool SingleInstance { get => _facade.SingleInstance; }

        public VariableService.ConditionOperatorType OperatorType => _conditionalOperatorType;
        public SchemeVariableBase Operand1 => _operand1;
        public SchemeVariableBase Operand2 => _operand2;

        // Новые свойства для доступа к веткам
        public IBlock TrueBranch => _trueOutput;
        public IBlock FalseBranch => _falseOutput;

        public void ProvideAction()
        {
            CurrentOutputIndex = _variableService.UseComparsion(_operand1.variableName, _conditionalOperatorType, _operand2.variableName) ? 1 : 0;

            Next?.ProvideAction();
        }

        public void SetOperation(SchemeVariableBase operand1, VariableService.ConditionOperatorType operatorType, SchemeVariableBase operand2)
        {
            _operand1 = operand1;
            _conditionalOperatorType = operatorType;
            _operand2 = operand2;

            string displayName = operand1 != null && operand2 != null ?
                $"{_operand1.variableName} {TypeExtensions.GetFriendlyConditionOperatorTypeName(_conditionalOperatorType)} {_operand2.variableName}"
                : "Значения не установлены!";

            _facade.Label.SetText(displayName);
        }

        public bool CheckForCorrectRelationships()
        {
            bool correctRelationships = true;

            CurrentOutputIndex = 0;
            for (int i = 0; i < Facade.BlockOutputButtons.Length; i++)
            {
                if (Next == null || !((IBlock)Next).CheckForCorrectRelationships())
                {
                    correctRelationships = false;
                    break;
                }

                CurrentOutputIndex++;
            }

            return correctRelationships;
        }

        public bool CheckForCorrectValues()
        {
            bool value = _operand1 != null && _operand2 != null
                && _trueOutput != null && _trueOutput.CheckForCorrectValues()
                && _falseOutput != null && _falseOutput.CheckForCorrectValues();

            return value;
        }

        public void Dispose()
        {
            GameObject.Destroy(_facade.gameObject);
        }
    }
}