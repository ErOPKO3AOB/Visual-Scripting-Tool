using Cysharp.Threading.Tasks;
using Extensions;
using Session.Scheme.Block;
using Session.Scheme.Block.Types;
using Session.Scheme.Variables;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalServices.CodeGeneration
{
    public sealed class CsharpCodeGenerator : ICodeGenerator
    {
        private readonly CodeGenerationFactory _factory;

        public CsharpCodeGenerator(CodeGenerationFactory codeGenerationFactory)
        {
            _factory = codeGenerationFactory;
        }

        public async UniTask<string> Generate()
        {
            await _factory.GatherBlocks();

            if (_factory.StartBlock == null)
            {
                _factory.LogError("The starting block was not detected in the scheme!");
                return "Error! Details in console";
            }

            if (!_factory.StartBlock.CheckForCorrectRelationships())
            {
                _factory.LogError("The scheme has no end! Connect all the wires correctly!");
                return "Error! Details in console";
            }

            if (!_factory.StartBlock.CheckForCorrectValues())
            {
                _factory.LogError("Important parameters are not specified in the scheme!");
                return "Error! Details in console";
            }

            var codeBuilder = new StringBuilder();
            codeBuilder.AppendLine("using System;");
            codeBuilder.AppendLine();
            codeBuilder.AppendLine("class Program");
            codeBuilder.AppendLine("{");
            codeBuilder.AppendLine("    static void Main(string[] args)");
            codeBuilder.AppendLine("    {");

            foreach (var variable in _factory.SchemeVariables)
            {
                codeBuilder.AppendLine($"        {MakeStringInitializedVariable(variable)}");
            }
            codeBuilder.AppendLine();

            IBlock current = (IBlock)_factory.StartBlock.Next;
            codeBuilder.Append(GenerateCodeForBlock(current, 1));

            codeBuilder.AppendLine("    }");
            codeBuilder.AppendLine("}");

            return codeBuilder.ToString();
        }

        private string GenerateCodeForBlock(IBlock block, int indentLevel)
        {
            if (block == null || block is EndBlock)
                return "";

            var sb = new StringBuilder();
            string indent = new ('\t', indentLevel);

            switch (block.ConcreteType)
            {
                case IBlock.BlockType.Action:
                    sb.Append(indent + MakeStringActionCodeParts((ActionBlock)block) + "\n");
                    sb.Append(GenerateCodeForBlock((IBlock)block.Next, indentLevel));
                    break;

                case IBlock.BlockType.Output:
                    sb.Append(indent + MakeStringOutputCodeParts((OutputBlock)block) + "\n");
                    sb.Append(GenerateCodeForBlock((IBlock)block.Next, indentLevel));
                    break;

                case IBlock.BlockType.Input:
                    sb.Append(indent + MakeStringInputCodeParts((InputBlock)block) + "\n");
                    sb.Append(GenerateCodeForBlock((IBlock)block.Next, indentLevel));
                    break;

                case IBlock.BlockType.Condition:
                    var cond = (ConditionBlock)block;
                    string condition = $"{cond.Operand1.variableName} {TypeExtensions.GetFriendlyConditionOperatorTypeName(cond.OperatorType)} {cond.Operand2.variableName}";

                    sb.Append(indent + $"if ({condition})\n");
                    sb.Append(indent + "{\n");
                    sb.Append(GenerateCodeForBlock(cond.TrueBranch, indentLevel + 1));
                    sb.Append(indent + "}\n");

                    sb.Append(indent + "else\n");
                    sb.Append(indent + "{\n");
                    sb.Append(GenerateCodeForBlock(cond.FalseBranch, indentLevel + 1));
                    sb.Append(indent + "}\n");
                    break;
            }

            return sb.ToString();
        }

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
                        if (chars[i] == ',') chars[i] = '.';
                    chars.Add('f');
                    rawValue = new string(chars.ToArray());
                }

                variableValueString = $"{rawValue};";
            }
            return $"{TypeExtensions.GetFriendlyTypeName(schemeVariable.ValueType)} {schemeVariable.variableName} = {variableValueString}";
        }

        public string MakeStringActionCodeParts(ActionBlock block)
        {
            return $"{block.Operand1.variableName} {TypeExtensions.GetFriendlyActionOperatorTypeName(block.OperatorType)} {block.Operand2.variableName};";
        }

        public string MakeStringInputCodeParts(InputBlock block)
        {
            if (block.SchemeVariable.ValueType == typeof(string))
                return $"{block.SchemeVariable.variableName} = Console.ReadLine();";
            else
                return $"{TypeExtensions.GetFriendlyTypeName(block.SchemeVariable.ValueType)}.TryParse(Console.ReadLine(), out {block.SchemeVariable.variableName});";
        }

        public string MakeStringOutputCodeParts(OutputBlock block)
        {
            return $"Console.WriteLine({block.SchemeVariable.variableName});";
        }
    }
}