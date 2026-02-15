using Extensions;
using Session.Scheme.Block;
using Session.Scheme.Block.Types;
using System.Collections.Generic;
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

        private readonly List<string> _variablesCodeParts = new();
        private readonly List<string> _methodCodeParts = new();
        private readonly List<string> _conditionCodeParts = new();
        private readonly List<string> _inputCodeParts = new();
        private readonly List<string> _outputCodeParts = new();

        private string _programmCode;

        public async Task<string> Generate()
        {
            _programmCode = START_PROGRAMM_TEXT;

            await MakeStringInitializedVariables();
            await MakeStringMethodCodeParts();
            await MakeStringConditionCodeParts();
            await MakeStringInputCodeParts();
            await MakeStringOutputCodeParts();

            await GenerateVariables();

            for (int i = 0; i < _codeGenerationFactory.; i++)
            {
                
            }
            if (((IBlock)_codeGenerationFactory.StartBlock.Next).ConcreteType == IBlock.BlockType.Method)
            {

            }

            return _programmCode;
        }

        #region String Generation
        public async Task MakeStringInitializedVariables()
        {
            if (_codeGenerationFactory.SchemeVariables.Count == 0) return;

            _variablesCodeParts.Clear();
            await Task.Delay(CodeGenerationFactory.SMALL_OPERATION_DELAY_MS);

            for (int i = 0; i < _codeGenerationFactory.SchemeVariables.Count; i++)
            {
                _variablesCodeParts.Add($"{TypeExtensions.GetFriendlyName(_codeGenerationFactory.SchemeVariables[i].ValueType)} {_codeGenerationFactory.SchemeVariables[i].variableName} = {_codeGenerationFactory.SchemeVariables[i].GetValue() ?? ""};");
            }
        }

        public async Task MakeStringMethodCodeParts()
        {
            if (_codeGenerationFactory.MethodBlocks.Count == 0) return;

            _methodCodeParts.Clear();
            await Task.Delay(CodeGenerationFactory.SMALL_OPERATION_DELAY_MS);

            for (int i = 0; i < _codeGenerationFactory.MethodBlocks.Count; i++)
            {
                MethodBlock block = _codeGenerationFactory.MethodBlocks[i];
                _methodCodeParts.Add($"{block.Operand1.variableName} {block.OperatorType} {block.Operand2.variableName};");

                await Task.Delay(CodeGenerationFactory.SMALL_OPERATION_DELAY_MS);
            }
        }

        public async Task MakeStringConditionCodeParts()
        {
            if (_codeGenerationFactory.ConditionBlocks.Count == 0) return;

            _conditionCodeParts.Clear();
            await Task.Delay(CodeGenerationFactory.SMALL_OPERATION_DELAY_MS);

            for (int i = 0; i < _codeGenerationFactory.ConditionBlocks.Count; i++)
            {
                ConditionBlock block = _codeGenerationFactory.ConditionBlocks[i];
                _conditionCodeParts.Add($"if ({block.Operand1.variableName} {block.OperatorType} {block.Operand2.variableName}))" +
                    "\n{" +
                    "\n" +
                    "\n}");

                await Task.Delay(CodeGenerationFactory.SMALL_OPERATION_DELAY_MS);
            }
        }

        public async Task MakeStringInputCodeParts()
        {
            if (_codeGenerationFactory.InputBlocks.Count == 0) return;

            _inputCodeParts.Clear();
            await Task.Delay(CodeGenerationFactory.SMALL_OPERATION_DELAY_MS);

            for (int i = 0; i < _codeGenerationFactory.InputBlocks.Count; i++)
            {
                InputBlock block = _codeGenerationFactory.InputBlocks[i];

                string finalExpression;

                if (block.SchemeVariable.ValueType == typeof(string))
                    finalExpression = $"{block.SchemeVariable.variableName} = Console.ReadLine();";
                else
                    finalExpression = $"{TypeExtensions.GetFriendlyName(block.SchemeVariable.ValueType)}.TryParse(Console.ReadLine(), out {TypeExtensions.GetFriendlyName(block.SchemeVariable.ValueType)} {block.SchemeVariable.variableName});";

                _inputCodeParts.Add(finalExpression);
                await Task.Delay(CodeGenerationFactory.SMALL_OPERATION_DELAY_MS);
            }
        }

        public async Task MakeStringOutputCodeParts()
        {
            if (_codeGenerationFactory.OutputBlocks.Count == 0) return;

            _outputCodeParts.Clear();
            await Task.Delay(CodeGenerationFactory.SMALL_OPERATION_DELAY_MS);

            for (int i = 0; i < _codeGenerationFactory.OutputBlocks.Count; i++)
            {
                OutputBlock block = _codeGenerationFactory.OutputBlocks[i];
                _outputCodeParts.Add($"Console.Write({block.SchemeVariable.variableName});");
            }
        }
        #endregion

        #region AlgorythmGeneration
        public async Task GenerateVariables()
        {
            foreach (var variable in _variablesCodeParts)
            {
                _programmCode = await _codeGenerationFactory.PasteCodeIntoBody(_programmCode, "static void Main(string[] args)", variable);
                await Task.Delay(10);
            }
        }


        #endregion
    }
}