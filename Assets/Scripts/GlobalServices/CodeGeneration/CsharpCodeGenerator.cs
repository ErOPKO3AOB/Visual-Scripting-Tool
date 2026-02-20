using Extensions;
using Session.Scheme.Block;
using Session.Scheme.Block.Types;
using Session.Scheme.Variables;
using System.Threading.Tasks;

namespace GlobalServices.CodeGeneration
{
    public sealed class CsharpCodeGenerator : ICodeGenerator
    {
        public CsharpCodeGenerator(CodeGenerationFactory codeGenerationFactory)
        {
            _factory = codeGenerationFactory;
        }

        private readonly CodeGenerationFactory _factory;

        private const string START_PROGRAMM_TEXT =
            "class Program\r\n{\r\n    static void Main(string[] args)\r\n    {\r\n        \r\n    }\r\n}";

        private string _programmCode;

        public async Task<string> Generate()
        {
            _programmCode = START_PROGRAMM_TEXT;

            await GenerateVariables();

            await _factory.PasteCodeIntoBody(_programmCode, "static void Main(string[] args)", " ");

            IBlock currentBlock = _factory.StartBlock;
            while (currentBlock.Next != _factory.EndBlock)
            {
                _programmCode = await FindBlockTypeAndPasteCode(currentBlock, _programmCode, "static void Main(string[] args)");
                currentBlock = (IBlock)currentBlock.Next;

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

        public async Task<string> MakeStringConditionCodeParts(ConditionBlock block)
        {
            string ifCodeBody = $"if ({block.Operand1.variableName} {TypeExtensions.GetFriendlyConditionOperatorTypeName(block.OperatorType)} {block.Operand2.variableName}))";
            string elseCodeBody = "else";
            string code = ifCodeBody +
                "\n{" +
                "\n" +
                "\n}" +
                "\n" +
                "\n" + elseCodeBody +
                "\n{" +
                "\n" +
                "\n}";

            block.CurrentOutputIndex = 1;
            code = await _factory.PasteCodeIntoBody(code,
                ifCodeBody, 
                await FindBlockTypeAndPasteCode((IBlock)block.Next, code, ifCodeBody));

            block.CurrentOutputIndex = 0;
            code = await _factory.PasteCodeIntoBody(code,
                elseCodeBody,
                await FindBlockTypeAndPasteCode((IBlock)block.Next, code, elseCodeBody));

            return code;
        }

        public async Task<string> MakeStringInputCodeParts(InputBlock block)
        {
            string code;

            if (block.SchemeVariable.ValueType == typeof(string))
                code = $"{block.SchemeVariable.variableName} = Console.ReadLine();";
            else
                code = $"{TypeExtensions.GetFriendlyTypeName(block.SchemeVariable.ValueType)}.TryParse(Console.ReadLine(), out {block.SchemeVariable.variableName});";

            await Task.Delay(5);
            
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
            foreach (var variable in _factory.SchemeVariables)
            {
                _programmCode = await _factory.PasteCodeIntoBody(_programmCode, "static void Main(string[] args)", MakeStringInitializedVariable(variable));
            }
        }

        public async Task<string> FindBlockTypeAndPasteCode(IBlock nextBlock, string fullCode, string codeBody)
        {
            switch (nextBlock.ConcreteType)
            {
                case IBlock.BlockType.Action:
                    fullCode = await _factory.PasteCodeIntoBody(fullCode, codeBody, MakeStringActionCodeParts((ActionBlock)nextBlock));
                    break;
                case IBlock.BlockType.Output:
                    fullCode = await _factory.PasteCodeIntoBody(fullCode, codeBody, MakeStringOutputCodeParts((OutputBlock)nextBlock));
                    break;
                case IBlock.BlockType.Input:
                    fullCode = await _factory.PasteCodeIntoBody(fullCode, codeBody, await MakeStringInputCodeParts((InputBlock)nextBlock));
                    break;
                case IBlock.BlockType.Condition:
                    fullCode = await _factory.PasteCodeIntoBody(fullCode, codeBody, await MakeStringConditionCodeParts((ConditionBlock)nextBlock));
                    break;
            }

            return fullCode;
        }
        #endregion
    }
}