using UnityEngine;

namespace Session.Scheme.Operators
{
    public class VariableOperatorService
    {
        public enum OperatorType
        {
            Plus, Minus, Divide, Multyply
        }

        public VariableOperatorService(OperatorType operatorType)
        {
            Type = operatorType;
        }

        public readonly OperatorType Type;
    }
}