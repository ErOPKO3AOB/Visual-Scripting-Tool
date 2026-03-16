using GlobalServices.CodeGeneration;
using Session.Scheme;
using Session.Scheme.Block;
using Session.Scheme.Block.Types;
using Session.Scheme.Variables;
using Session.Scheme.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

namespace GlobalServices
{
    public class CodeGenerationFactory
    {
        public CodeGenerationFactory(VariableService variableService, SchemeBlockFactory schemeBlockFactory, SchemeConsoleService consoleService)
        {
            _variableService = variableService;
            _schemeBlockFactory = schemeBlockFactory;
            _consoleService = consoleService;
        }

        private readonly VariableService _variableService;
        private readonly SchemeBlockFactory _schemeBlockFactory;
        private readonly SchemeConsoleService _consoleService;

        public const int SMALL_OPERATION_DELAY_MS = 20;
        public const int MEDIUM_OPERATION_DELAY_MS = 100;

        public List<SchemeVariableBase> SchemeVariables { get; private set; } = new();

        public StartBlock StartBlock { get; private set; }
        public EndBlock EndBlock { get; private set; }
        public List<ActionBlock> MethodBlocks { get; private set; } = new();
        public List<ConditionBlock> ConditionBlocks { get; private set; } = new();
        public List<OutputBlock> OutputBlocks { get; private set; } = new();
        public List<InputBlock> InputBlocks { get; private set; } = new();

        private async Task GatherBlocks()
        {
            SchemeVariables = _variableService.Variables;
            await Task.Delay(SMALL_OPERATION_DELAY_MS);

            StartBlock = null;
            StartBlock = (StartBlock)_schemeBlockFactory.Blocks.Find(b => b.GetType() == typeof(StartBlock));
            await Task.Delay(SMALL_OPERATION_DELAY_MS);

            EndBlock = null;
            EndBlock = (EndBlock)_schemeBlockFactory.Blocks.Find(b => b.GetType() == typeof(EndBlock));
            await Task.Delay(SMALL_OPERATION_DELAY_MS);

            MethodBlocks.Clear();
            var methodBlocks = _schemeBlockFactory.Blocks.FindAll(b => b.GetType() == typeof(ActionBlock));
            for (int i = 0; i < methodBlocks.Count; i++)
                MethodBlocks.Add((ActionBlock)methodBlocks[i]);
            await Task.Delay(MEDIUM_OPERATION_DELAY_MS);

            ConditionBlocks.Clear();
            var conditionBlocks = _schemeBlockFactory.Blocks.FindAll(b => b.GetType() == typeof(ConditionBlock));
            for (int i = 0; i < conditionBlocks.Count; i++)
                ConditionBlocks.Add((ConditionBlock)conditionBlocks[i]);
            await Task.Delay(MEDIUM_OPERATION_DELAY_MS);

            OutputBlocks.Clear();
            var outputBlocks = _schemeBlockFactory.Blocks.FindAll(b => b.GetType() == typeof(OutputBlock));
            for (int i = 0; i < outputBlocks.Count; i++)
                OutputBlocks.Add((OutputBlock)outputBlocks[i]);
            await Task.Delay(MEDIUM_OPERATION_DELAY_MS);

            InputBlocks.Clear();
            var inputBlocks = _schemeBlockFactory.Blocks.FindAll(b => b.GetType() == typeof(InputBlock));
            for (int i = 0; i < inputBlocks.Count; i++)
                InputBlocks.Add((InputBlock)inputBlocks[i]);
            await Task.Delay(MEDIUM_OPERATION_DELAY_MS);
        }

        public async Task<string> GenerateCode(/*ICodeGenerator codeGenerator*/)
        {
            await GatherBlocks();

            if (StartBlock == null)
            {
                _consoleService.SpawnMessage("У схемы не обнаружен стартовый блок!");
                return "Ошибка! Детали в консоли";
            }

            if (!StartBlock.CheckForCorrectRelationships())
            {
                _consoleService.SpawnMessage("У схемы не обнаружен конец! Корректно подключите все провода!");
                return "Ошибка! Детали в консоли";
            }

            else if (!StartBlock.CheckForCorrectValues())
            {
                _consoleService.SpawnMessage("У не указаны важные параметры!");
                return "Ошибка! Детали в консоли";
            }

            ICodeGenerator codeGenerator = new CsharpCodeGenerator(this);
            string code = await codeGenerator.Generate();

            return code;
        }

        /// <summary>
        /// Вставляет указанный фрагмент кода в тело конструкции, имя которой передано как bodyName.
        /// Поиск производится по шаблону: bodyName, затем любые пробелы/переводы строк, затем {.
        /// </summary>
        /// <param name="fullCode">Исходный код программы.</param>
        /// <param name="bodyName">Имя тела (например, "Main", "Program", "if").</param>
        /// <param name="codeToPaste">Вставляемый код.</param>
        /// <returns>Код с вставленным фрагментом.</returns>
        public async Task<string> PasteCodeIntoBody(string fullCode, string bodyName, string codeToPaste)
        {
            return await Task.Run(() =>
            {
                // Поиск открывающей скобки тела
                string pattern = $@"{Regex.Escape(bodyName)}\s*{{";
                var match = Regex.Match(fullCode, pattern);
                if (!match.Success)
                    return fullCode;

                int openBraceIndex = match.Index + match.Value.Length - 1; // позиция '{'

                // Поиск парной закрывающей скобки с учётом вложенности
                int depth = 1;
                int closeBraceIndex = -1;
                for (int i = openBraceIndex + 1; i < fullCode.Length; i++)
                {
                    if (fullCode[i] == '{') depth++;
                    else if (fullCode[i] == '}') depth--;
                    if (depth == 0)
                    {
                        closeBraceIndex = i;
                        break;
                    }
                }
                if (closeBraceIndex == -1)
                    return fullCode; // не найдена закрывающая скобка
                
                // Определение базового отступа (отступ строки с '{')
                int lineStart = fullCode.LastIndexOf('\n', openBraceIndex) + 1;
                if (lineStart < 0) lineStart = 0;
                string beforeBraceOnLine = fullCode.Substring(lineStart, openBraceIndex - lineStart);
                string baseIndent = ExtractIndent(beforeBraceOnLine);
                string extraIndent = "\t"; // или 4 пробела

                // Добавление отступа к каждой строке вставляемого кода
                string newLine = DetectNewLine(fullCode);
                string[] lines = codeToPaste.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                var indentedLines = new List<string>();
                foreach (string line in lines)
                {
                    if (string.IsNullOrEmpty(line))
                        indentedLines.Add(line);
                    else
                        indentedLines.Add(baseIndent + extraIndent + line);
                }
                string codeWithIndent = string.Join(newLine, indentedLines);

                // Вставка перед закрывающей скобкой
                string beforeInsert = fullCode.Substring(0, closeBraceIndex);
                string afterInsert = fullCode.Substring(closeBraceIndex);

                // Гарантируем перевод строки перед вставляемым кодом, если его нет
                if (closeBraceIndex > 0 && fullCode[closeBraceIndex - 1] != '\n' && fullCode[closeBraceIndex - 1] != '\r')
                    beforeInsert += newLine;

                //Debug.Log(beforeInsert);
                //Debug.Log(codeWithIndent);
                //Debug.Log(newLine);
                //Debug.Log(afterInsert);

                return beforeInsert + codeWithIndent + newLine + afterInsert;
            }).ConfigureAwait(false);
        }

        private string ExtractIndent(string line)
        {
            var indent = new StringBuilder();
            foreach (char c in line)
            {
                if (c == ' ' || c == '\t')
                    indent.Append(c);
                else
                    break;
            }
            return indent.ToString();
        }

        private string DetectNewLine(string text)
        {
            if (text.Contains("\r\n"))
                return "\r\n";
            if (text.Contains("\n"))
                return "\n";
            return Environment.NewLine;
        }
    }
}