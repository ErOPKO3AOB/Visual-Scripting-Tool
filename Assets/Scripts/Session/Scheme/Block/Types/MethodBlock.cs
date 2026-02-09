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

        private VariableService.OperatorType _operatorType;
        private SchemeVariableBase _operand1;
        private SchemeVariableBase _operand2;

        public SchemeBlockFacade Facade => _facade;
        public IBlock Next { get; set; }
        public bool SingleInstance { get => _facade.SingleInstance; }

        public void ProvideAction()
        {
            _variableService.UseOperation(_operand1.variableName, _operatorType, _operand2.variableName);
            Next?.ProvideAction();
        }

        public void SetOperation(SchemeVariableBase operand1, VariableService.OperatorType operatorType, SchemeVariableBase operand2)
        {
            _operand1 = operand1;
            _operatorType = operatorType;
            _operand2 = operand2;
         
            Debug.Log($"Trying set operation: {_operand1.variableName} {operatorType} {_operand2.variableName}");

            _facade.Label.SetText($"{_operand1.variableName} {operatorType} {_operand2.variableName}");
        }

        public void Dispose()
        {
            GameObject.Destroy(_facade.gameObject);
        }
    }
}