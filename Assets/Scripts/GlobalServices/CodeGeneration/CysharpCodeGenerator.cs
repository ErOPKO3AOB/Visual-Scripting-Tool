using Extensions;
using Session.Scheme.Block;
using Session.Scheme.Block.Types;
using Session.Scheme.Variables;
using System.Threading.Tasks;

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

            IBlock nextBlock = _codeGenerationFactory.StartBlock;

            for (int i = 0; i < _codeGenerationFactory.AllMultipleInstancesBlocksCount; i++)
            {
                if (((IBlock)nextBlock.Next).ConcreteType == IBlock.BlockType.Action)
                {
                    _programmCode = await _codeGenerationFactory.PasteCodeIntoBody(_programmCode, "static void Main(string[] args)", MakeStringActionCodeParts((ActionBlock)nextBlock.Next));
                }
                else if (((IBlock)nextBlock.Next).ConcreteType == IBlock.BlockType.Output)
                {
                    _programmCode = await _codeGenerationFactory.PasteCodeIntoBody(_programmCode, "static void Main(string[] args)", MakeStringOutputCodeParts((OutputBlock)nextBlock.Next));
                }
                else if (((IBlock)nextBlock.Next).ConcreteType == IBlock.BlockType.Input)
                {
                    _programmCode = await _codeGenerationFactory.PasteCodeIntoBody(_programmCode, "static void Main(string[] args)", MakeStringInputCodeParts((InputBlock)nextBlock.Next));
                }
                //// Сырая концепция
                //else if (((IBlock)nextBlock.Next).ConcreteType == IBlock.BlockType.Condition)
                //{
                //    _programmCode = await _codeGenerationFactory.PasteCodeIntoBody(_programmCode, "static void Main(string[] args)", MakeStringConditionCodeParts((ConditionBlock)nextBlock.Next));
                //}
            }

            return _programmCode;
        }

        #region String Generation
        public string MakeStringInitializedVariable(SchemeVariableBase schemeVariable)
        {
            return $"{TypeExtensions.GetFriendlyName(schemeVariable.ValueType)} {schemeVariable.variableName} = {schemeVariable.GetValue() ?? ""};";
        }

        public string MakeStringActionCodeParts(ActionBlock block)
        {
            return $"{block.Operand1.variableName} {block.OperatorType} {block.Operand2.variableName};";
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
                code = $"{TypeExtensions.GetFriendlyName(block.SchemeVariable.ValueType)}.TryParse(Console.ReadLine(), out {TypeExtensions.GetFriendlyName(block.SchemeVariable.ValueType)} {block.SchemeVariable.variableName});";

            return code;
        }

        public string MakeStringOutputCodeParts(OutputBlock block)
        {
            return $"Console.Write({block.SchemeVariable.variableName});";
        }
        #endregion

        #region Algorythm Generation
        public async Task GenerateVariables()
        {
            foreach (var variable in _codeGenerationFactory.SchemeVariables)
            {
                _programmCode = await _codeGenerationFactory.PasteCodeIntoBody(_programmCode, "static void Main(string[] args)", MakeStringInitializedVariable(variable));
                await Task.Delay(10);
            }
        }
        #endregion
    }
}