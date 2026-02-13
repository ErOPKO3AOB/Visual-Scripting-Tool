using Session.Scheme.Variables;
using System;
using UnityEngine;

namespace Session.Scheme.Block.Types
{
    public class MethodBlock : IBlock, IDisposable
    {
        public MethodBlock(SchemeBlockFacade facade, VariableService variableService)
        {
            _facade = facade;
            _variableService = variableService;
        }

        private readonly SchemeBlockFacade _facade;
        private readonly VariableService _variableService;

        private VariableService.MethodOperatorType _operatorType;
        private SchemeVariableBase _operand1;
        private SchemeVariableBase _operand2;

        public VariableService.MethodOperatorType OperatorType => _operatorType;
        public SchemeVariableBase Operand1 => _operand1;
        public SchemeVariableBase Operand2 => _operand2;

        public SchemeBlockFacade Facade => _facade;
        public IBlock Next { get; set; }
        public bool SingleInstance => _facade.SingleInstance;

        public void ProvideAction()
        {
            _variableService.UseOperation(_operand1.variableName, _operatorType, _operand2.variableName);
            Next?.ProvideAction();
        }

        public void SetOperation(SchemeVariableBase operand1, VariableService.MethodOperatorType operatorType, SchemeVariableBase operand2)
        {
            _operand1 = operand1;
            _operatorType = operatorType;
            _operand2 = operand2;

            string displayName = operand1 != null && operand2 != null ?
                $"{_operand1.variableName} {_operatorType} {_operand2.variableName}" 
                : "Значения не установлены!";

            _facade.Label.SetText(displayName);
        }

        public void Dispose()
        {
            GameObject.Destroy(_facade.gameObject);
        }
    }
}