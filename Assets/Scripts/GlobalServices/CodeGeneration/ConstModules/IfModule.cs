using System.Collections.Generic;
using System.Linq;
using Session.Scheme.Variables;
using UnityEngine;

namespace GlobalServices.CodeGeneration.ConstModules
{
    public class IfModule
    {
        public string Value { get; private set; } = $"if {CodeModulesConsts.OPENED_BRACE}{CodeModulesConsts.CLOSED_BRACE} {CodeModulesConsts.OPENED_CURLY_BRACE} {CodeModulesConsts.CLOSED_CURLY_BRACE}";
        
        public void PasteExpressionInBraces(string operand1Name, VariableService.ConditionalOperatorType opType, string operand2Name)
        {
            bool symbolFound = false;
            string opTypeString = "";

            switch (opType)
            {
                case VariableService.ConditionalOperatorType.IsEqual:
                    opTypeString = "==";
                    break;
                case VariableService.ConditionalOperatorType.IsNotEqual:
                    opTypeString = "!=";
                    break;
                case VariableService.ConditionalOperatorType.IsGreater:
                    opTypeString = ">";
                    break;
                case VariableService.ConditionalOperatorType.IsGreaterOrEqual:
                    opTypeString = ">=";
                    break;
                case VariableService.ConditionalOperatorType.IsLess:
                    opTypeString = "<";
                    break;
                case VariableService.ConditionalOperatorType.IsLessOrEqual:
                    opTypeString = "<=";
                    break;
            }

            string expression = operand1Name + opTypeString + operand2Name;

            char[] chars = Value.ToCharArray();
            List<char> charsList = chars.ToList<char>();

            for (int i = 0; i < charsList.Count; i++)
            {
                if (symbolFound)
                {
                    for (int j = 0; j < expression.Length; j++)
                    {
                        charsList.Insert(i, expression[i]);
                    }

                    break;
                }

                else if (charsList[i].Equals(CodeModulesConsts.OPENED_BRACE))
                {
                    symbolFound = true;
                }
            }
        }

        public void PasteExpressionInCurlyBraces(string expression)
        {
            bool symbolFound = false;

            List<char> chars = Value.ToCharArray().ToList<char>();

            for (int i = 0; i < chars.Count; i++)
            {
                if (symbolFound)
                {
                    for (int j = 0; j < expression.Length; j++)
                    {
                        chars.Insert(i, expression[i]);
                    }

                    break;
                }

                else if (chars[i].Equals(CodeModulesConsts.OPENED_CURLY_BRACE))
                {
                    symbolFound = true;
                }
            }
        }
    }
}