using Session.Scheme.Variables;
using UnityEngine;

namespace Session.Scheme.Block.Types
{
    public class IfBlock : IActionProvider
    {
        public IfBlock(SchemeBlockFacade facade, VariableService variableService)
        {
            _facade = facade;
            _variableService = variableService;
        }

        private readonly SchemeBlockFacade _facade;
        private readonly VariableService _variableService;

        private SchemeVariableBase _operand1;
        private VariableService.ConditionalOperatorType _conditionalOperatorType;
        private SchemeVariableBase _operand2;

        public IActionProvider Next { get; set; }

        private IActionProvider _trueOutput;
        private IActionProvider _falseOutput;

        public void ProvideAction()
        {
            if (_variableService.UseComparison(_operand1.variableName, _conditionalOperatorType, _operand2.variableName) == true)
            {
                Next = _trueOutput;
            }

            else
            {
                Next = _falseOutput;
            }

            Next.ProvideAction();
        }

        public void SetTrueOutput(IActionProvider output)
        {
            _trueOutput = output;
        }

        public void SetFalseOutput(IActionProvider output)
        {
            _falseOutput = output;
        }

        public void SetOperation(SchemeVariableBase operand1, VariableService.ConditionalOperatorType operatorType, SchemeVariableBase operand2)
        {
            _operand1 = operand1;
            _conditionalOperatorType = operatorType;
            _operand2 = operand2;
        }

        public void Dispose()
        {
            GameObject.Destroy(_facade.gameObject);
        }
    }
}