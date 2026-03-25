using GlobalServices.CodeGeneration;
using Session.Scheme;
using Session.Scheme.Block;
using Session.Scheme.Block.Types;
using Session.Scheme.Variables;
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

        // Делаем метод публичным, чтобы генератор мог его вызвать
        public async Task GatherBlocks()
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

        // Новый метод для логирования ошибок
        public void LogError(string message)
        {
            _consoleService.SpawnMessage(message);
        }

        public async Task<string> GenerateCode(/*ICodeGenerator codeGenerator*/)
        {
            await GatherBlocks();

            if (StartBlock == null)
            {
                LogError("У схемы не обнаружен стартовый блок!");
                return "Ошибка! Детали в консоли";
            }

            if (!StartBlock.CheckForCorrectRelationships())
            {
                LogError("У схемы не обнаружен конец! Корректно подключите все провода!");
                return "Ошибка! Детали в консоли";
            }

            else if (!StartBlock.CheckForCorrectValues())
            {
                LogError("У не указаны важные параметры!");
                return "Ошибка! Детали в консоли";
            }

            ICodeGenerator codeGenerator = new CsharpCodeGenerator(this);
            string code = await codeGenerator.Generate();

            return code;
        }

        // Удаляем PasteCodeIntoBody и вспомогательные методы, они больше не нужны
        // Но если они используются где-то ещё, оставьте, они не мешают
    }
}