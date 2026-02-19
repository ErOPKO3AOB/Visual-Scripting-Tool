using Extensions;
using Session.Scheme.Block;
using Session.Scheme.Block.Types;
using Session.Scheme.Variables;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace GlobalServices.CodeGeneration
{
    public sealed class CysharpCodeGenerator : ICodeGenerator
    {
        public CysharpCodeGenerator(CodeGenerationFactory codeGenerationFactory)
        {
            _codeGenerationFactory = codeGenerationFactory;
        }

        private readonly CodeGenerationFactory _codeGenerationFactory;

        private const string START_PROGRAMM_TEXT =
            "class Program\r\n{\r\n    static void Main(string[] args)\r\n    {\r\n        \r\n    }\r\n}";

        private string _programmCode;

        public async Task<string> Generate()
        {
            _programmCode = START_PROGRAMM_TEXT;

            await GenerateVariables();

            await _codeGenerationFactory.PasteCodeIntoBody(_programmCode, "static void Main(string[] args)", " ");

            IBlock current = _codeGenerationFactory.StartBlock;
            while (current.Next != null && current.Next != _codeGenerationFactory.EndBlock)
            {
                current = (IBlock)current.Next;
                switch (current.ConcreteType)
                {
                    case IBlock.BlockType.Action:
                        _programmCode = await _codeGenerationFactory.PasteCodeIntoBody(_programmCode, "static void Main(string[] args)", MakeStringActionCodeParts((ActionBlock)current));
                        break;
                    case IBlock.BlockType.Output:
                        _programmCode = await _codeGenerationFactory.PasteCodeIntoBody(_programmCode, "static void Main(string[] args)", MakeStringOutputCodeParts((OutputBlock)current));
                        break;
                    case IBlock.BlockType.Input:
                        _programmCode = await _codeGenerationFactory.PasteCodeIntoBody(_programmCode, "static void Main(string[] args)", MakeStringInputCodeParts((InputBlock)current));
                        break;
                    case IBlock.BlockType.Condition:
                        // обработать условие 
                        break;
                }
                await Task.Delay(10);
            }

            return _programmCode;
        }

        #region String Generation
        public string MakeStringInitializedVariable(SchemeVariableBase schemeVariable)
        {
            string variableValueString;
            if (schemeVariable.ValueType == typeof(string))
            {
                string rawValue = schemeVariable.GetStartValue()?.ToString() ?? "";
                string escapedValue = rawValue.Replace("\\", "\\\\").Replace("\"", "\\\"");
                variableValueString = $"\"{escapedValue}\";";
            }
            else
            {
                string rawValue = schemeVariable.GetStartValue()?.ToString();
                if (string.IsNullOrEmpty(rawValue))
                {
                    if (schemeVariable.ValueType == typeof(int)) rawValue = "0";
                    else if (schemeVariable.ValueType == typeof(double)) rawValue = "0.0";
                    else if (schemeVariable.ValueType == typeof(bool)) rawValue = "false";
                    else rawValue = "default";
                }
                variableValueString = $"{rawValue};";
            }
            return $"{TypeExtensions.GetFriendlyTypeName(schemeVariable.ValueType)} {schemeVariable.variableName} = {variableValueString}";
        }

        public string MakeStringActionCodeParts(ActionBlock block)
        {
            return $"{block.Operand1.variableName} {TypeExtensions.GetFriendlyActionOperatorTypeName(block.OperatorType)} {block.Operand2.variableName};";
        }

        public string MakeStringConditionCodeParts(ConditionBlock block)
        {
            return $"if ({block.Operand1.variableName} {block.OperatorType} {block.Operand2.variableName}))" +
                "\n{" +
                "\n" +
                "\n}" +
                "\n" +
                "\nelse" +
                "\n{" +
                "\n" +
                "\n}";
        }

        public string MakeStringInputCodeParts(InputBlock block)
        {
            string code;

            if (block.SchemeVariable.ValueType == typeof(string))
                code = $"{block.SchemeVariable.variableName} = Console.ReadLine();";
            else
                code = $"{TypeExtensions.GetFriendlyTypeName(block.SchemeVariable.ValueType)}.TryParse(Console.ReadLine(), out {block.SchemeVariable.variableName});";

            return code;
        }

        public string MakeStringOutputCodeParts(OutputBlock block)
        {
            return $"Console.WriteLine({block.SchemeVariable.variableName});";
        }
        #endregion

        #region Algorythm Generation
        public async Task GenerateVariables()
        {
            foreach (var variable in _codeGenerationFactory.SchemeVariables)
            {
                _programmCode = await _codeGenerationFactory.PasteCodeIntoBody(_programmCode, "static void Main(string[] args)", MakeStringInitializedVariable(variable));
            }
        }
        #endregion
    }
}