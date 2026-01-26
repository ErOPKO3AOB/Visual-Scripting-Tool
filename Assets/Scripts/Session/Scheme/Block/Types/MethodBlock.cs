using Session.Scheme.Operators;
using Session.Scheme.Variables;
using System;
using UnityEngine;

namespace Session.Scheme.Block.Types
{
    public class MethodBlock : IActionProvider, IDisposable
    {
        public MethodBlock(SchemeBlockFacade facade, VariableOperatorService operatorService)
        {
            _facade = facade;
            _operatorService = operatorService;
        }

        private readonly SchemeBlockFacade _facade;
        private VariableOperatorService _operatorService;

        private SchemeVariableBase _operand1;
        private VariableOperatorService.OperatorType _operatorType;
        private SchemeVariableBase _operand2;

        public IActionProvider Next { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void ProvideAction()
        {
            _operatorService.UseOperation(_operand1, _operatorType, _operand2);
            Next.ProvideAction();
        }

        public void SetOperation(SchemeVariableBase operand1, VariableOperatorService.OperatorType operatorType, SchemeVariableBase operand2)
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