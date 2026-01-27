using Session.Scheme.Variables;
using System;
using UnityEngine;

namespace Session.Scheme.Block.Types
{
    public class MethodBlock : IActionProvider, IDisposable
    {
        public MethodBlock(SchemeBlockFacade facade, VariableService variableService)
        {
            _facade = facade;
            _variableService = variableService;
        }

        private readonly SchemeBlockFacade _facade;
        private readonly VariableService _variableService;

        private SchemeVariableBase _operand1;
        private VariableService.OperatorType _operatorType;
        private SchemeVariableBase _operand2;

        public IActionProvider Next { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void ProvideAction()
        {
            _variableService.UseOperation(_operand1.variableName, _operatorType, _operand2.variableName);
            Next.ProvideAction();
        }

        public void SetOperation(SchemeVariableBase operand1, VariableService.OperatorType operatorType, SchemeVariableBase operand2)
        {
            _operand1 = operand1;
            _operatorType = operatorType;
            _operand2 = operand2;
        }

        public void Dispose()
        {
            GameObject.Destroy(_facade.gameObject);
        }
    }
}