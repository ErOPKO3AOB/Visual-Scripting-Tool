using Extensions;
using Session.Scheme.Block;
using Session.Scheme.Block.Types;
using Session.Scheme.Variables;
using System;
using System.Linq;
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
            "class Program\r\n{\r\n    static void Main(string[] args)\r\n    {}\r\n}";

        private string _programmCode;

        public async Task<string> Generate()
        {
            _programmCode = START_PROGRAMM_TEXT;

            // Генерация переменных (вставляются в Main)
            await GenerateVariables();

            // Запуск рекурсивной генерации с корневого блока (Start) и целевого тела Main
            await ProcessBlock((IBlock)_factory.StartBlock.Next, "static void Main(string[] args)");

            return _programmCode;
        }

        /// <summary>
        /// Рекурсивно обрабатывает блоки, начиная с current, вставляя код в тело с именем targetBodyName.
        /// </summary>
        private async Task ProcessBlock(IBlock current, string targetBodyName)
        {
            while (current != null && current != _factory.EndBlock)
            {
                if (current.ConcreteType == IBlock.BlockType.Action)
                {
                    _programmCode = await _factory.PasteCodeIntoBody(_programmCode, targetBodyName,
                        MakeStringActionCodeParts((ActionBlock)current));
                    current = (IBlock)current.Next;
                }

                else if (current.ConcreteType == IBlock.BlockType.Output)
                {
                    _programmCode = await _factory.PasteCodeIntoBody(_programmCode, targetBodyName,
                        MakeStringOutputCodeParts((OutputBlock)current));
                    current = (IBlock)current.Next;
                }

                else if (current.ConcreteType == IBlock.BlockType.Input)
                {
                    _programmCode = await _factory.PasteCodeIntoBody(_programmCode, targetBodyName,
                        await MakeStringInputCodeParts((InputBlock)current));
                    current = (IBlock)current.Next;
                }

                else if (current.ConcreteType == IBlock.BlockType.Condition)
                {
                    var cond = (ConditionBlock)current;

                    string ifHeader = $"if ({cond.Operand1.variableName} {TypeExtensions.GetFriendlyConditionOperatorTypeName(cond.OperatorType)} {cond.Operand2.variableName})";
                    string elseHeader = "else";

                    string ifElseSkeleton = ifHeader + "\n{\n}\n" + elseHeader + "\n{\n}";
                    _programmCode = await _factory.PasteCodeIntoBody(_programmCode, targetBodyName, ifElseSkeleton);

                    cond.CurrentOutputIndex = 1;
                    current = (IBlock)cond.Next;
                    await ProcessBlock(current, ifHeader);

                    cond.CurrentOutputIndex = 0;
                    current = (IBlock)cond.Next;
                    await ProcessBlock(current, elseHeader);
                    break;
                }

                await Task.Delay(10);
            }
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
                    else if (schemeVariable.ValueType == typeof(float)) rawValue = "0.0f";
                    else if (schemeVariable.ValueType == typeof(bool)) rawValue = "false";
                    else rawValue = "default";
                }

                if (schemeVariable.ValueType == typeof(bool))
                {
                    if (rawValue == "True") rawValue = "true";
                    else if (rawValue == "False") rawValue = "false";
                }

                else if (schemeVariable.ValueType == typeof(float))
                {
                    var chars = rawValue.ToCharArray().ToList();
                    for (int i = 0; i < chars.Count; i++)
                    {
                        if (chars[i] == ',')
                            chars[i] = '.';
                    }
                    chars.Add('f');
                    rawValue = "";

                    for (int i = 0; i < chars.Count; i++)
                    {
                        rawValue += chars[i];
                    }
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
            _programmCode = await _factory.PasteCodeIntoBody(_programmCode, "static void Main(string[] args)", "\r\n");

            string ifHeader = $"if ({block.Operand1.variableName} {TypeExtensions.GetFriendlyConditionOperatorTypeName(block.OperatorType)} {block.Operand2.variableName})";
            string elseHeader = "else";

            // Скелет без лишних переводов строк внутри блоков
            string skeleton = ifHeader + "\n{}\n" + elseHeader + "\n{}";

            string code = skeleton;

            // Генерация then-ветки (индекс 1)
            block.CurrentOutputIndex = 1;
            code = await _factory.PasteCodeIntoBody(code, ifHeader,
                await FindBlockTypeAndPasteCode((IBlock)block.Next, code, ifHeader));

            // Генерация else-ветки (индекс 0)
            block.CurrentOutputIndex = 0;
            code = await _factory.PasteCodeIntoBody(code, elseHeader,
                await FindBlockTypeAndPasteCode((IBlock)block.Next, code, elseHeader));

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

            _programmCode = await _factory.PasteCodeIntoBody(_programmCode, "static void Main(string[] args)", "\r\n");
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
