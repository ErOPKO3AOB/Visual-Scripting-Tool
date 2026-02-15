using GlobalServices.CodeGeneration;
using Session.Scheme;
using Session.Scheme.Block.Types;
using Session.Scheme.Variables;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GlobalServices
{
    public class CodeGenerationFactory
    {
        public CodeGenerationFactory(VariableService variableService, SchemeBlockFactory schemeBlockFactory)
        {
            _variableService = variableService;
            _schemeBlockFactory = schemeBlockFactory;
        }

        private readonly VariableService _variableService;
        private readonly SchemeBlockFactory _schemeBlockFactory;

        public const int SMALL_OPERATION_DELAY_MS = 50;
        public const int MEDIUM_OPERATION_DELAY_MS = 150;

        public List<SchemeVariableBase> SchemeVariables { get; private set; } = new();

        public StartBlock StartBlock { get; private set; }
        public EndBlock EndBlock { get; private set; }
        public List<ActionBlock> MethodBlocks { get; private set; } = new();
        public List<ConditionBlock> ConditionBlocks { get; private set; } = new();
        public List<OutputBlock> OutputBlocks { get; private set; } = new();
        public List<InputBlock> InputBlocks { get; private set; } = new();

        public int AllMultipleInstancesBlocksCount
        {
            get
            {
                return MethodBlocks.Count +
                    ConditionBlocks.Count +
                    OutputBlocks.Count +
                    InputBlocks.Count;
            }
        }

        private async Task GatherBlocks()
        {
            SchemeVariables.Clear();
            SchemeVariables = _variableService.Variables;
            await Task.Delay(SMALL_OPERATION_DELAY_MS);

            StartBlock = null;
            StartBlock = (StartBlock)_schemeBlockFactory.Blocks.Find(b => b.GetType() == typeof(StartBlock));
            await Task.Delay(SMALL_OPERATION_DELAY_MS);

            EndBlock = null;
            EndBlock = (EndBlock)_schemeBlockFactory.Blocks.Find(b => b.GetType() == typeof(EndBlock));
            await Task.Delay(SMALL_OPERATION_DELAY_MS);

            //MethodBlocks.Clear();
            //MethodBlocks = ((MethodBlock[])_schemeBlockFactory.Blocks.FindAll(b => b.GetType() == typeof(MethodBlock)).ToArray()).ToList();
            //await Task.Delay(MEDIUM_OPERATION_DELAY_MS);

            //ConditionBlocks.Clear();
            //ConditionBlocks = ((ConditionBlock[])_schemeBlockFactory.Blocks.FindAll(b => b.GetType() == typeof(ConditionBlock)).ToArray()).ToList();
            //await Task.Delay(MEDIUM_OPERATION_DELAY_MS);

            //OutputBlocks.Clear();
            //OutputBlocks = ((OutputBlock[])_schemeBlockFactory.Blocks.FindAll(b => b.GetType() == typeof(OutputBlock)).ToArray()).ToList();
            //await Task.Delay(MEDIUM_OPERATION_DELAY_MS);

            //InputBlocks.Clear();
            //InputBlocks = ((InputBlock[])_schemeBlockFactory.Blocks.FindAll(b => b.GetType() == typeof(InputBlock)).ToArray()).ToList();
            //await Task.Delay(MEDIUM_OPERATION_DELAY_MS);

            await Task.CompletedTask;
        }

        public async Task<string> GenerateCode(/*ICodeGenerator codeGenerator*/)
        {
            await GatherBlocks();

            if (StartBlock == null || EndBlock == null || MethodBlocks == null || ConditionBlocks == null || OutputBlocks == null || InputBlocks == null)
                throw new NullReferenceException("Some of important blocks are missing!");

            string code = string.Empty;

            ICodeGenerator codeGenerator = new CysharpCodeGenerator(this);
            code = await codeGenerator.Generate();

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
            // Асинхронно выполняем тяжёлую работу, чтобы не блокировать поток
            return await Task.Run(() =>
            {
                // Экранируем специальные символы имени и строим регулярку: имя + пробелы/переносы + {
                string pattern = $@"{Regex.Escape(bodyName)}\s*{{";
                var match = Regex.Match(fullCode, pattern);
                if (!match.Success)
                    return fullCode; // тело не найдено – ничего не делаем

                int braceIndex = match.Index + match.Value.Length - 1; // позиция символа '{'

                // Определяем отступ строки, в которой стоит '{'
                int lineStart = fullCode.LastIndexOf('\n', braceIndex) + 1;
                if (lineStart < 0) lineStart = 0;
                string beforeBraceOnLine = fullCode.Substring(lineStart, braceIndex - lineStart);
                string baseIndent = ExtractIndent(beforeBraceOnLine);
                string extraIndent = "\t"; // можно заменить на 4 пробела, если нужно

                // Разбиваем вставляемый код на строки
                string newLine = DetectNewLine(codeToPaste);
                string[] lines = codeToPaste.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                var indentedLines = new List<string>(lines.Length);

                foreach (string line in lines)
                {
                    if (string.IsNullOrEmpty(line))
                        indentedLines.Add(line);
                    else
                        indentedLines.Add(baseIndent + extraIndent + line);
                }
                string codeWithIndent = string.Join(newLine, indentedLines);

                // Проверяем, есть ли перевод строки сразу после '{'
                bool hasNewLineAfterBrace = HasNewLineAt(fullCode, braceIndex + 1, newLine);

                string beforeInsert = fullCode.Substring(0, braceIndex + 1);
                string afterInsert = fullCode.Substring(braceIndex + 1);

                if (!hasNewLineAfterBrace)
                    beforeInsert += newLine;

                string result = beforeInsert + codeWithIndent;
                if (!afterInsert.StartsWith(newLine) && !string.IsNullOrEmpty(afterInsert))
                    result += newLine;

                result += afterInsert;
                return result;
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

        private bool HasNewLineAt(string text, int index, string newLine)
        {
            if (index >= text.Length) return false;
            if (newLine == "\r\n")
                return index + 1 < text.Length && text[index] == '\r' && text[index + 1] == '\n';
            else // "\n"
                return text[index] == '\n';
        }
    }
}